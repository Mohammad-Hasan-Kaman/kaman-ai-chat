using System;
using System.Configuration;
using System.IO;

namespace chatai
{
    /// <summary>
    /// Resolves the API key at runtime from sources that are never committed to
    /// source control, and persists it in the per-user application data folder.
    ///
    /// Lookup order:
    ///   1. Environment variable KAMAN_AI_CHAT_API_KEY
    ///   2. &lt;app&gt;.config  -&gt;  appSettings/OpenModelApiKey
    ///   3. %APPDATA%\kaman-ai-chat\settings.txt
    /// </summary>
    internal static class Settings
    {
        public const string EnvironmentVariableName = "KAMAN_AI_CHAT_API_KEY";
        public const string AppSettingsKey = "OpenModelApiKey";
        private const string Placeholder = "YOUR_API_KEY_HERE";

        public static string SettingsDirectory
        {
            get
            {
                string root = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                return Path.Combine(root, "kaman-ai-chat");
            }
        }

        public static string SettingsFile
        {
            get { return Path.Combine(SettingsDirectory, "settings.txt"); }
        }

        /// <summary>
        /// Returns the configured API key, or null when no key is configured.
        /// </summary>
        public static string GetApiKey()
        {
            string key = Environment.GetEnvironmentVariable(EnvironmentVariableName);
            if (IsUsable(key)) return key.Trim();

            key = ReadFromAppConfig();
            if (IsUsable(key)) return key.Trim();

            key = ReadFromUserFile();
            if (IsUsable(key)) return key.Trim();

            return null;
        }

        public static void SaveApiKey(string apiKey)
        {
            if (string.IsNullOrWhiteSpace(apiKey)) return;

            Directory.CreateDirectory(SettingsDirectory);
            File.WriteAllText(SettingsFile, apiKey.Trim());
        }

        public static bool HasApiKey()
        {
            return GetApiKey() != null;
        }

        private static bool IsUsable(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) return false;

            string trimmed = key.Trim();
            return trimmed.Length > 0
                   && !trimmed.Equals(Placeholder, StringComparison.OrdinalIgnoreCase)
                   && !trimmed.StartsWith("<", StringComparison.Ordinal);
        }

        private static string ReadFromAppConfig()
        {
            try
            {
                var settings = System.Configuration.ConfigurationManager.AppSettings;
                if (settings != null)
                {
                    string value = settings[AppSettingsKey];
                    if (!string.IsNullOrWhiteSpace(value)) return value;
                }
            }
            catch (ConfigurationErrorsException)
            {
                // A malformed app.config must not stop the app from starting.
            }

            return null;
        }

        private static string ReadFromUserFile()
        {
            try
            {
                if (!File.Exists(SettingsFile)) return null;
                return File.ReadAllText(SettingsFile);
            }
            catch (IOException)
            {
                return null;
            }
            catch (UnauthorizedAccessException)
            {
                return null;
            }
        }
    }
}
