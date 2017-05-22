using DAL;
using System;
using System.Linq;
using System.Text;
using Common;
using System.Threading.Tasks;
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
        private Encoding _encoding;
        private IEnumerable<IAuthRule> _rules;
        private ILogger<AuthService> _logger;
        private IEnumerable<IGrantProvider> _grantProviders;

        public AuthService(IDataService dataService,
            Encoding encoding,
            HashAlgorithm hash,
            IEnumerable<IAuthRule> rules,
            IEnumerable<IGrantProvider> grantProviders,
            ILogger<AuthService> logger)
        {
            _logger = logger;
            _encoding = encoding;
            _dataService = dataService;
            _usersRepo = _dataService.GetRepository<User>();
            _hash = hash;
            _rules = rules;
            _grantProviders = grantProviders;
        }

        public Result<User> GetUser(string token)
        {
            try
            {
                var reader = new TokenReader(token);
                return Result<User>.Success(reader.ToUser());
            }
            catch (ArgumentNullException)
            {
                return Result<User>.Error(ErrorCodes.NOT_AUTH);
            }
            catch (ArgumentException)
            {
                return Result<User>.Error(ErrorCodes.NOT_AUTH);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return Result<User>.Error(ErrorCodes.UNEXPECTED);
        }

        public async Task<Result<AuthResponse>> SignIn(SignInRequest request)
        {
            try
            {
                var grantProvider = _grantProviders.FirstOrDefault(p => p.By.Equals(request.GrantType));
                if (grantProvider == null)
                {
                    return Result<AuthResponse>.Error(ErrorCodes.AUTH_INVALID_GRANT_TYPE);
                }
                return await grantProvider.Grant(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return Result<AuthResponse>.Error(ErrorCodes.UNEXPECTED);
        }

        public async Task<Result> Register(RegistrationRequest request)
        {
            try
            {
                var result = Result.Success();
                foreach (var rule in _rules)
                {
                    result.AddErrors(rule.Validate(request).ErrorCodes);
                }
                if (!result.IsSuccess)
                {
                    return Result.Error(result.ErrorCodes);
                }
                var user = new User()
                {
                    Email = request.Email,
                    Password = _hash.ComputeHash(_encoding.GetBytes(request.Password))
                };
                await _usersRepo.Add(user);
                _dataService.SaveChanges();
                _logger.LogInformation($"User {user} registered.");
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return Result.Error(ErrorCodes.UNEXPECTED);
        }
    }
}
