using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigLoader.UnitTests
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class TestConfigEnvVarsAttribute : Attribute, ITestAction
    {
        public void AfterTest(ITest test)
        {
            Environment.SetEnvironmentVariable("DB", null);
            Environment.SetEnvironmentVariable("DBDriver", null);
        }

        public void BeforeTest(ITest test)
        {
            Environment.SetEnvironmentVariable("DB", "done!");
            Environment.SetEnvironmentVariable("DBDriver", "done!!");
        }

        public ActionTargets Targets
        {
            get { return ActionTargets.Test; }
        }
    }
}
