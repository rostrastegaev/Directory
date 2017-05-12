using Common;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Tests.Common
{
    [TestClass]
    public class ConfigurationServiceTests
    {
        public static IConfigurationService ConfigService { get; private set; }

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            DirectoryInfo directory = new DirectoryInfo("../../../Common");
            var builder = new ConfigurationBuilder()
                .SetBasePath(directory.FullName)
                .AddJsonFile("testsettings.json", optional: false);
            ConfigService = new ConfigurationService(builder.Build());
        }

        [TestMethod]
        public void ConfigServiceTest()
        {
            var config = ConfigService.GetConfig<TestConfig>("Test");
            Assert.AreEqual(25, config.PropertyInt);
            Assert.AreEqual("test", config.PropertyString);
        }

        [TestMethod]
        public void ConfigServiceSectionTest()
        {
            var config = ConfigService.GetConfig<TestConfig>("Test:TestSection");
            Assert.AreEqual(30, config.PropertyInt);
            Assert.AreEqual("test1", config.PropertyString);
        }

        private class TestConfig
        {
            public int PropertyInt { get; private set; }
            public string PropertyString { get; private set; }
        }
    }
}
