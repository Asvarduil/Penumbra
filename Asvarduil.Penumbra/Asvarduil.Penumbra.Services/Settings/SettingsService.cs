using System;
using System.Configuration;
using AppEnvironment = Asvarduil.Penumbra.Services.Settings.Environment;

namespace Asvarduil.Penumbra.Services
{
    public class SettingsService
    {
        /// <summary>
        /// Attempts to read the given setting from the Configuration file.
        /// </summary>
        /// <param name="settingName">Setting to try to read</param>
        /// <param name="throwExceptionIfMissing">If true, and the setting not present, throw an ApplicationException.</param>
        /// <returns>Value of the configuration setting if present, nothing if not.</returns>
        public static string Read(string settingName, bool throwExceptionIfMissing = false)
        {
            string result = ConfigurationManager.AppSettings[settingName];
            if (string.IsNullOrWhiteSpace(result))
                if (throwExceptionIfMissing)
                    throw new ApplicationException($"App Setting '{settingName}' is not set in this application's App.config/Web.config file.");

            return result;
        }

        /// <summary>
        /// For a setting that's appended with D/T/P (Dev, Test, Prod), find the correct key and return that value.
        /// This functions return value is based upon the 'Environment' key that must be included in App/Web.config.
        /// </summary>
        /// <param name="settingName">Environment-specific app setting to read</param>
        /// <param name="throwExceptionIfMissing">If true, and the environment setting not present, throw an ApplicationException.</param>
        /// <returns>Value of the configuration setting if present, nothing if not.</returns>
        public static string ReadEnvironment(string settingName, bool throwExceptionIfMissing = false)
        {
            string envValue = Read("Environment", true);
            AppEnvironment environment = envValue.ToEnum<AppEnvironment>();

            // Determine suffix of environment setting.  Default to "D" for DEV.
            string suffix = " D";
            switch(environment)
            {
                case AppEnvironment.DEV:
                    break;

                case AppEnvironment.TEST:
                    suffix = " T";
                    break;

                case AppEnvironment.PROD:
                    suffix = " P";
                    break;

                default:
                    break;
            }

            string environmentSettingName = settingName + suffix;
            string result = Read(environmentSettingName, throwExceptionIfMissing);

            return result;
        }
    }
}
