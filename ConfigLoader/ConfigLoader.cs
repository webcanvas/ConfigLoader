using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConfigLoader
{

    public static class ConfigLoader
    {
        public static T LoadConfig<T>(params string[] configFiles) where T : class, new()
        {
            // create a new obj
            var config = new T();

            // populate from json configs
            foreach (var configFile in configFiles)
            {
                // try load the file.
                // Below could throw an exception. Maybe in the future handle this better.
                var contents = File.ReadAllText(configFile);
                JsonConvert.PopulateObject(contents, config);
            }

            // get the properties via reflection
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // try to fill all properties!
            foreach (var prop in properties)
            {
                var t = prop.PropertyType;

                // We only want to support strings and primatives.
                if (!t.IsPrimitive && !t.IsValueType && t != typeof(string))
                {
                    // the value is not supported. 
                    continue;
                }

                // try getting it from the local config file. 
                // TODO maybe we shouldn't be using this if the value has been set. 
                // Easiest thing to do is loop through the props twice once before config files and here.

                // if it starts with DB (TODO Maybe check for case). Then get it from the connection strings. 
                if (prop.Name.StartsWith("DB"))
                {
                    // woot. connection string. 
                }
                else
                {
                    // get it from the app settings.
                }

                // try to load the environment variable for the property. 
                var variable = Environment.GetEnvironmentVariable(prop.Name);

                // if the value is null don't do anything
                if (variable == null)
                {
                    continue;
                }

                // convert the value.
                var val = Convert.ChangeType(variable, t);

                // set the value
                prop.SetValue(config, val);

            }

            return config;
        }
    }
}
