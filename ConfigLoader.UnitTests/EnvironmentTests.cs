using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigLoader.UnitTests
{
    [TestFixture]
    public class EnvironmentTests
    {
        [Test, TestConfigEnvVars]
        public void CanLoadFromEnv()
        {
            // lets get the configuration values.
            var config = ConfigLoader.LoadConfig<TestConfig>();

            Assert.IsNotNull(config);
            Assert.AreEqual("done!", config.DB);
            Assert.AreEqual("done!!", config.DBDriver);
        }

        [Test, TestConfigEnvVars]
        public void DoesNotThrowExceptionOnMissingEnv()
        {
            // Set it to null to remove the environment var
            Environment.SetEnvironmentVariable("DBDriver", "");

            // lets get the configuration values.
            var config = ConfigLoader.LoadConfig<TestConfig>();

            Assert.IsNotNull(config);
            Assert.AreEqual("done!", config.DB);
            Assert.AreNotEqual("done!!", config.DBDriver);
        }
    }
}
