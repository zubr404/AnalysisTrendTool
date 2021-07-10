using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace AnalisesTrendTool.Services
{
    public class ConfigurationService
    {
        public static Dictionary<string, string> Get()
        {
            var allSettings = new Dictionary<string, string>();
            var appSettings = ConfigurationManager.AppSettings;

            if (appSettings.Count > 0)
            {
                foreach (var key in appSettings.AllKeys)
                {
                    allSettings.Add(key, appSettings[key]);
                }
            }
            return allSettings;
        }

        public static string Get(string key)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                return appSettings[key];
            }
            catch (ConfigurationErrorsException)
            {
                throw;
            }
        }

        public static void Update(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                throw;
            }
        }
    }
}
