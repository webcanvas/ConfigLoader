using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigLoader.UnitTests
{
    [TestFixture]
    public class JsonTests
    {
        [Test]
        public void CanLoadFromFile()
        {
            // lets get the configuration values.
            var config = Config.New<TestConfig>(".\\testdata.json");

            Assert.IsNotNull(config);
            Assert.AreEqual("full!", config.DB);
            Assert.AreEqual("full!!", config.DBDriver);
            Assert.AreEqual("full!!!", config.Setting1);
        }
    }
}
