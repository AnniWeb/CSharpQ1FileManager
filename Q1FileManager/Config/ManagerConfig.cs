using System; 
using System.Configuration; 

namespace Q1FileManager.Config
{
    public class ManagerConfig
    {
        public delegate string GetUserValueDelegate ();
        
        public static string ReadOrCreateSetting(string key, GetUserValueDelegate userValue)
        {
            var value = ReadSetting(key);
            if (value == null)
            {
                value = userValue();
                AddUpdateAppSettings(key, value);
            }

            return value;
        }
        
        public static string ReadSetting(string key)  
        {  
            try  
            {  
                var appSettings = ConfigurationManager.AppSettings;  
                return appSettings[key];  
            }  
            catch (ConfigurationErrorsException e)
            {
                Logger.LogError(e);
                throw new ConfigurationErrorsException("Системный сбой в получении данных конфигурации");
            }
        } 
        
        public static void AddUpdateAppSettings(string key, string value)  
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
            catch (ConfigurationErrorsException e)
            {
                Logger.LogError(e);
                throw new ConfigurationErrorsException("Системный сбой при записи данных конфигурации");
            }  
        }  
    }
}