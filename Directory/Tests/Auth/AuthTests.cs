using Auth;
using Common;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Claims;
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
            AuthService = new AuthService(config,
                new DataServiceMock(),
                Encoding.Unicode,
                SHA256.Create(),
                new IAuthRule[] { new EmailRule(), new PasswordRule(config) },
                new Logger<AuthService>(factory));

        }

        [TestMethod]
        public void RegistrationTest()
        {
            Result<IAuthResponse> resultResponse = AuthService.Register(new AuthRequest()
            {
                Email = "regTest@test.com",
                Password = "123456",
                Confirmation = "123456"
            }, new ClaimsPrincipal()).Result;
            Assert.IsTrue(resultResponse.IsSuccess);
            Assert.IsNotNull(resultResponse.Data.AccessToken);
        }

        [TestMethod]
        public void SignInTest()
        {
            Result<IAuthResponse> resultResponse = AuthService.SignIn(new AuthRequest()
            {
                Email = "test@test.com",
                Password = " 1"
            }, new ClaimsPrincipal()).Result;
            Assert.IsTrue(resultResponse.IsSuccess);
            Assert.IsNotNull(resultResponse.Data.AccessToken);
        }

        [TestMethod]
        public void GetIdTest()
        {
            ClaimsPrincipal principal = new ClaimsPrincipal();
            Result<IAuthResponse> resultResponse = AuthService.SignIn(new AuthRequest()
            {
                Email = "test@test.com",
                Password = " 1"
            }, principal).Result;
            Assert.IsTrue(resultResponse.IsSuccess);
            Assert.IsNotNull(resultResponse.Data.AccessToken);

            Result<int> resultId = AuthService.GetId(principal);
            Assert.IsTrue(resultId.IsSuccess);
            Assert.AreEqual(1, resultId.Data);
        }

        [TestMethod]
        public void RegistrationFailTest()
        {
            // email in use
            {
                Result<IAuthResponse> response = AuthService.Register(new AuthRequest()
                {
                    Email = "test@test.com",
                    Password = "123456",
                    Confirmation = "123456"
                }, new ClaimsPrincipal()).Result;
                Assert.IsFalse(response.IsSuccess);
                Assert.IsNull(response.Data);
            }
            // password length
            {
                Result<IAuthResponse> response = AuthService.Register(new AuthRequest()
                {
                    Email = "passLength@test.com",
                    Password = "123",
                    Confirmation = "123"
                }, new ClaimsPrincipal()).Result;
                Assert.IsFalse(response.IsSuccess);
                Assert.IsNull(response.Data);
            }
            // confirmation matching
            {
                Result<IAuthResponse> response = AuthService.Register(new AuthRequest()
                {
                    Email = "confirm@test.com",
                    Password = "12345678",
                    Confirmation = "123456"
                }, new ClaimsPrincipal()).Result;
                Assert.IsFalse(response.IsSuccess);
                Assert.IsNull(response.Data);
            }
            // invalid email
            {
                Result<IAuthResponse> response = AuthService.Register(new AuthRequest()
                {
                    Email = ".123",
                    Password = "123456",
                    Confirmation = "123456"
                }, new ClaimsPrincipal()).Result;
                Assert.IsFalse(response.IsSuccess);
                Assert.IsNull(response.Data);
            }
        }

        [TestMethod]
        public void SignInFailTest()
        {
            Result<IAuthResponse> response = AuthService.SignIn(new AuthRequest()
            {
                Email = "test@test.com",
                Password = " 123"
            }, new ClaimsPrincipal()).Result;
            Assert.IsFalse(response.IsSuccess);
            Assert.IsNull(response.Data);
        }

        [TestMethod]
        public void GetIdFailTest()
        {
            Result<int> resultId = AuthService.GetId(new ClaimsPrincipal());
            Assert.IsFalse(resultId.IsSuccess);
        }
    }
}
