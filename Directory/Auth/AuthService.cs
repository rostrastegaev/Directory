using DAL;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Common;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Auth
{
    public class AuthService : IAuthService
    {
        private AuthConfig _config;
        private IDataService _dataService;
        private IRepository<User> _usersRepo;
        private HashAlgorithm _hash;
        private SecurityKey _key;
        private Encoding _encoding;
        private IEnumerable<IAuthRule> _rules;
        private ILogger<AuthService> _logger;

        public AuthService(AuthConfig config,
            IDataService dataService,
            Encoding encoding,
            HashAlgorithm hash,
            IEnumerable<IAuthRule> rules,
            ILogger<AuthService> logger)
        {
            _logger = logger;
            _encoding = encoding;
            _config = config;
            _dataService = dataService;
            _usersRepo = _dataService.GetRepository<User>();
            _hash = hash;
            _key = new SymmetricSecurityKey(_encoding.GetBytes(_config.Key));
            _rules = rules;
        }

        public Result<int> GetId(ClaimsPrincipal principal)
        {
            try
            {
                Claim idClaim = principal.FindFirst(ClaimTypes.Sid);
                if (!int.TryParse(idClaim.Value, out int id))
                {
                    return Result<int>.Error(ErrorCodes.NOT_AUTH);
                }
                return Result<int>.Success(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return Result<int>.Error(ErrorCodes.UNEXPECTED);
        }

        public async Task<Result<IAuthResponse>> Register(IAuthRequest request, ClaimsPrincipal principal)
        {
            try
            {
                var user = new User()
                {
                    Email = request.Email,
                    Password = GetHash(request.Password)
                };
                Result<User> userResult = await _usersRepo.Get(
                    u => u.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase));
                if (userResult.Data != null)
                {
                    userResult.AddError(ErrorCodes.AUTH_EMAIL_IN_USE);
                }
                foreach (var rule in _rules)
                {
                    var ruleResult = rule.Validate(request);
                    userResult.AddErrors(ruleResult.ErrorCodes);
                }
                if (!userResult.IsSuccess)
                {
                    return Result<IAuthResponse>.Error(userResult.ErrorCodes);
                }
                await _usersRepo.Add(user);
                _dataService.SaveChanges();
                _logger.LogInformation($"User {user} registered.");
                return await SignIn(request, principal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return Result<IAuthResponse>.Error(ErrorCodes.UNEXPECTED);
        }

        public async Task<Result<IAuthResponse>> SignIn(IAuthRequest request, ClaimsPrincipal principal)
        {
            try
            {
                Result<User> userResult = await _usersRepo.Get(
                u => request.Email.Equals(u.Email, StringComparison.OrdinalIgnoreCase));
                User user = userResult.Data;
                if (user == null)
                {
                    userResult.AddError(ErrorCodes.AUTH_EMAIL_NOT_FOUND);
                    return Result<IAuthResponse>.Error(userResult.ErrorCodes);
                }
                if (!user.Password.SequenceEqual(GetHash(request.Password)))
                {
                    userResult.AddError(ErrorCodes.AUTH_EMAIL_OR_PASSWORD_INCORRECT);
                    return Result<IAuthResponse>.Error(userResult.ErrorCodes);
                }
                var claims = new Claim[]
                {
                    new Claim(ClaimTypes.Sid, user.Id.ToString())
                };
                var identity = new ClaimsIdentity(claims);
                principal.AddIdentity(identity);

                var now = DateTime.UtcNow;
                JwtSecurityToken token = new JwtSecurityToken(
                    issuer: _config.Issuer,
                    audience: _config.Audience,
                    claims: identity.Claims,
                    notBefore: now,
                    expires: now.Add(TimeSpan.FromHours(_config.Lifetime)),
                    signingCredentials: new SigningCredentials(_key, SecurityAlgorithms.HmacSha256));
                _logger.LogInformation($"User {user} signed in.");
                return Result<IAuthResponse>.Success(
                    new AuthResponse(new JwtSecurityTokenHandler().WriteToken(token)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return Result<IAuthResponse>.Error(ErrorCodes.UNEXPECTED);
        }

        private byte[] GetHash(string password) =>
            _hash.ComputeHash(_encoding.GetBytes(password));
    }
}
