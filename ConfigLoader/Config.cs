using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConfigLoader
{

    public static class Config
    {
        private static bool IsValidProperty(PropertyInfo property)
        {
            var t = property.PropertyType;

            // We only want to support strings and primatives.
            return t.IsPrimitive || t.IsValueType || t == typeof(string);
        }

        private static string GetValueFromConfig(string key)
        {
            // we have a default convention for databases. They start with DB.
            // this could be DB, DBDriver, DB_{key}, DBDriver_{key}
            // if it's DB or DBDriver use the last index.
            if (key.Equals("DB"))
            {
                // get the connection string for the last index.
                var index = ConfigurationManager.ConnectionStrings.Count - 1;
                var conn = ConfigurationManager.ConnectionStrings[index];
                return conn == null ? null : conn.ConnectionString;
            }
            else if (key.Equals("DBDriver"))
            {
                // get the provider from the last index
                var index = ConfigurationManager.ConnectionStrings.Count - 1;
                var conn = ConfigurationManager.ConnectionStrings[index];
                return conn == null ? null : conn.ProviderName;
            }
            else if (key.StartsWith("DB_"))
            {
                // get the key
                var sub = key.Substring(3);
                var conn = ConfigurationManager.ConnectionStrings[sub];
                return conn == null ? null : conn.ConnectionString;
            }
            else if (key.StartsWith("DBDriver_"))
            {
                // get the key
                var sub = key.Substring(9);
                var conn = ConfigurationManager.ConnectionStrings[sub];
                return conn == null ? null : conn.ProviderName;
            }

            // It's not a database convention.. just go direct to app settings
            return ConfigurationManager.AppSettings[key];
        }

        // We don't know if the path is absolute or not. But we do need one.
        private static string GetPhysicalPath(string path)
        {
            if (Path.IsPathRooted(path)) return path;
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
        }

        /// <summary>
        /// Creates a new config for the templated type
        /// </summary>
        /// <typeparam name="T">Config type</typeparam>
        /// <param name="configFiles">Json file paths</param>
        /// <returns>A new config</returns>
        public static T New<T>(params string[] configFiles) where T : class, new()
        {
            // create a new obj
            var config = new T();
            Populate(config, configFiles);

            return config;
        }

        /// <summary>
        /// Populate an existing config
        /// </summary>
        /// <param name="config">The config object</param>
        /// <param name="configFiles">Json file paths</param>
        public static void Populate(object config, params string[] configFiles)
        {
            // guard against a null object
            if (config == null) return;

            // get the properties via reflection
            var properties = config.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(IsValidProperty);

            // try to find the properties from the web/app config
            foreach (var prop in properties)
            {
                var t = prop.PropertyType;
                var key = prop.Name;

                // get the value from the configuration file
                var value = GetValueFromConfig(key);

                // if we don't have a value return
                if (value == null) continue;

                // convert the value.
                var val = Convert.ChangeType(value, t);

                // set the value
                prop.SetValue(config, val, null);
            }

            // populate from json configs
            foreach (var configFile in configFiles)
            {
                // try load the file.
                // Below could throw an exception. Maybe in the future handle this better.
                var path = GetPhysicalPath(configFile);
                var contents = File.ReadAllText(path);
                JsonConvert.PopulateObject(contents, config);
            }

            // try to fill all properties!
            foreach (var prop in properties)
            {
                var t = prop.PropertyType;
                var key = prop.Name;

                // try to load the environment variable for the property. 
                var variable = Environment.GetEnvironmentVariable(key);

                // if the value is null don't do anything
                if (variable == null) continue;

                // convert the value.
                var val = Convert.ChangeType(variable, t);

                // set the value
                prop.SetValue(config, val, null);
            }

        }
    }
}
