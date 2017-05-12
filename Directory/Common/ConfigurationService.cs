using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;

namespace Common
{
    public class ConfigurationService : IConfigurationService
    {
        private IConfiguration _config;

        public ConfigurationService(IConfiguration config)
        {
            _config = config;
        }

        public T GetConfig<T>(string section)
        {
            IConfigurationSection moduleSection = _config.GetSection(section);
            return Cast<T>(moduleSection);
        }

        private T Cast<T>(IConfigurationSection section)
        {
            Type type = typeof(T);
            T instance = (T)type.GetConstructor(Type.EmptyTypes).Invoke(null);
            PropertyInfo[] properties = type.GetProperties();
            foreach (var property in properties)
            {
                string configValue = section[property.Name];
                if (string.IsNullOrEmpty(configValue))
                {
                    continue;
                }
                object value = configValue;
                if (property.PropertyType.Equals(typeof(int)))
                {
                    value = int.Parse(configValue);
                }
                property.SetValue(instance, value);
            }

            return instance;
        }
    }
}
