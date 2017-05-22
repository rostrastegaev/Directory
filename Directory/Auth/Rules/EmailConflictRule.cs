using System;
using Common;
using DAL;

namespace Auth
{
    public class EmailConflictRule : IAuthRule
    {
        private IRepository<User> _usersRepo;

        public EmailConflictRule(IDataService dataService)
        {
            _usersRepo = dataService.GetRepository<User>();
        }

        public Result Validate(RegistrationRequest request)
        {
            User user = _usersRepo.Get(
                u => u.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase)).Result;
            if (user != null)
            {
                return Result.Error(ErrorCodes.AUTH_EMAIL_IN_USE);
            }
            return Result.Success();
        }
    }
}
