using Auth;
using Common;
using DAL;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Cryptography;
using System.Text;
using Tests.Common;
using Tests.DAL;

namespace Tests.Auth
{
    [TestClass]
    public class AuthTests
    {
        public static IAuthService AuthService { get; private set; }

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            ILoggerFactory factory = new LoggerFactory().AddDebug();
            ConfigurationServiceTests.ClassInit(null);
            IConfigurationService configService = ConfigurationServiceTests.ConfigService;
            AuthConfig config = configService.GetConfig<AuthConfig>("Auth");
            HashAlgorithm hash = SHA256.Create();
            var dataService = new DataServiceMock();
            AuthService = new AuthService(dataService,
                Encoding.Unicode,
                SHA256.Create(),
                new IAuthRule[] { new EmailRule(), new PasswordRule(config), new EmailConflictRule(dataService) },
                new IGrantProvider[]
                {
                    new PasswordGrantProvider(Encoding.Unicode, hash, dataService, config),
                    new RefreshTokenGrantProvider(config)
                },
                new Logger<AuthService>(factory));

        }

        [TestMethod]
        public void RegistrationTest()
        {
            Result resultResponse = AuthService.Register(new RegistrationRequest()
            {
                Email = "regTest@test.com",
                Password = "123456",
                Confirmation = "123456"
            }).Result;
            Assert.IsTrue(resultResponse.IsSuccess);
        }

        [TestMethod]
        public void SignInPasswordTest()
        {
            Result<AuthResponse> resultResponse = AuthService.SignIn(new SignInRequest()
            {
                Email = "test@test.com",
                Password = " 1",
                GrantType = "password"
            }).Result;
            Assert.IsTrue(resultResponse.IsSuccess);
            Assert.IsNotNull(resultResponse.Data.AccessToken);
            Assert.IsNotNull(resultResponse.Data.RefreshToken);
        }

        [TestMethod]
        public void SignInRefreshTest()
        {
            Result<AuthResponse> resultResponse = AuthService.SignIn(new SignInRequest()
            {
                Email = "test@test.com",
                Password = " 1",
                GrantType = "password"
            }).Result;
            Assert.IsTrue(resultResponse.IsSuccess);
            Assert.IsNotNull(resultResponse.Data.AccessToken);
            Assert.IsNotNull(resultResponse.Data.RefreshToken);

            Result<AuthResponse> refreshResponse = AuthService.SignIn(new SignInRequest()
            {
                GrantType = "refresh_token",
                Token = resultResponse.Data.RefreshToken
            }).Result;
            Assert.IsTrue(refreshResponse.IsSuccess);
            Assert.IsNotNull(refreshResponse.Data.AccessToken);
        }

        [TestMethod]
        public void GetIdTest()
        {
            Result<AuthResponse> resultResponse = AuthService.SignIn(new SignInRequest()
            {
                Email = "test@test.com",
                Password = " 1",
                GrantType = "password"
            }).Result;
            Assert.IsTrue(resultResponse.IsSuccess);
            Assert.IsNotNull(resultResponse.Data.AccessToken);
            Assert.IsNotNull(resultResponse.Data.RefreshToken);

            Result<User> userResult = AuthService.GetUser(resultResponse.Data.AccessToken);
            Assert.IsTrue(userResult.IsSuccess);
            Assert.AreEqual(1, userResult.Data.Id);
        }

        [TestMethod]
        public void RegistrationFailTest()
        {
            // email in use
            {
                Result response = AuthService.Register(new RegistrationRequest()
                {
                    Email = "test@test.com",
                    Password = "123456",
                    Confirmation = "123456"
                }).Result;
                Assert.IsFalse(response.IsSuccess);
            }
            // password length
            {
                Result response = AuthService.Register(new RegistrationRequest()
                {
                    Email = "passLength@test.com",
                    Password = "123",
                    Confirmation = "123"
                }).Result;
                Assert.IsFalse(response.IsSuccess);
            }
            // confirmation matching
            {
                Result response = AuthService.Register(new RegistrationRequest()
                {
                    Email = "confirm@test.com",
                    Password = "12345678",
                    Confirmation = "123456"
                }).Result;
                Assert.IsFalse(response.IsSuccess);
            }
            // invalid email
            {
                Result response = AuthService.Register(new RegistrationRequest()
                {
                    Email = ".123",
                    Password = "123456",
                    Confirmation = "123456"
                }).Result;
                Assert.IsFalse(response.IsSuccess);
            }
        }

        [TestMethod]
        public void SignInFailTest()
        {
            Result<AuthResponse> response = AuthService.SignIn(new SignInRequest()
            {
                Email = "test@test.com",
                Password = " 123",
                GrantType = "password"
            }).Result;
            Assert.IsFalse(response.IsSuccess);
            Assert.IsNull(response.Data);
        }

        [TestMethod]
        public void GetIdFailTest()
        {
            Result<User> resultUser = AuthService.GetUser(string.Empty);
            Assert.IsFalse(resultUser.IsSuccess);
        }
    }
}
