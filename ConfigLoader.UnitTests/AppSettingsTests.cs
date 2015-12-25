using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigLoader.UnitTests
{
    [TestFixture]
    public class AppSettingsTests
    {
        [Test]
        public void CanLoadFromAppConfig()
        {
            // We have some conventions at play. 
            // IE.. if it looks like a database then load it from the connection strings. 

            // lets get the configuration values.
            var config = ConfigLoader.LoadConfig<TestConfig>();

            Assert.IsNotNull(config);
            Assert.AreEqual("something!", config.DB);
            Assert.AreEqual("something!!", config.DBDriver);
            Assert.AreEqual("something!!!", config.Setting1);
        }
    }
}
