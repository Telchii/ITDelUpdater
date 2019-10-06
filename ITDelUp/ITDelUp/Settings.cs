using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.ComponentModel;

namespace ITDelUp
{
    static class Settings
    {
        private static readonly string SettingsFile = "Settings.json";
        public static string BundlerScriptFile { get; private set; }
        public static string UrlsFile { get; private set; }
        public static string ChromePath { get; private set; }

        public static string OutputDirectory { get; private set; }
        public static string ZipOutputDirectory { get; private set; }
        public static string OutputFileName { get; private set; }
        public static bool TimestampOutput { get; private set; }

        static Settings()
        {
            //Apply default values. These will be overwritten if they exist in the settings.
            BundlerScriptFile = @"Bundler.bat";
            UrlsFile = @"Urls.txt";
            ChromePath = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe";
            OutputDirectory = $@"C:\Users\{Environment.UserName}\Downloads\";
            ZipOutputDirectory = $@"C:\Users\{Environment.UserName}\Desktop\";
            OutputFileName = "IT Delete";
            TimestampOutput = true;

            if (File.Exists(SettingsFile))
            {
                try
                {
                    SettingsModel SettingsFromFile = JsonConvert.DeserializeObject<SettingsModel>(File.ReadAllText(SettingsFile));

                    if (!string.IsNullOrEmpty(SettingsFromFile.BundlerScript.Trim()))
                        BundlerScriptFile = SettingsFromFile.BundlerScript;

                    if (!string.IsNullOrEmpty(SettingsFromFile.UrlsFile.Trim()))
                        UrlsFile = SettingsFromFile.UrlsFile;

                    if (!string.IsNullOrEmpty(SettingsFromFile.LocalChromeExe.Trim()))
                        ChromePath = SettingsFromFile.LocalChromeExe;

                    if (!string.IsNullOrEmpty(SettingsFromFile.OutputDirectory.Trim()))
                        OutputDirectory = SettingsFromFile.OutputDirectory;

                    if (!string.IsNullOrEmpty(SettingsFromFile.ZipOutputDirectory.Trim()))
                        ZipOutputDirectory = SettingsFromFile.ZipOutputDirectory;

                    if (!string.IsNullOrEmpty(SettingsFromFile.OutputFileName.Trim()))
                        OutputFileName = SettingsFromFile.OutputFileName;

                    //TimestampOutput: Newtonsoft + SettingModel's default value should handle this not having a value.

                }
                catch (Exception e)
                {
                    //Quietly do nothing on exception.
                }
            }
        }
    }

    class SettingsModel
    {
        [DefaultValue("")] //Uses "" instead of string.Empty, as a constant value is requried and "" is const at compile time and string.Empty is not.
        public string BundlerScript { get; set; }
        [DefaultValue("")]
        public string UrlsFile { get; set; }
        [DefaultValue("")]
        public string LocalChromeExe { get; set; }
        [DefaultValue("")]
        public string OutputDirectory { get; set; }
        [DefaultValue("")]
        public string ZipOutputDirectory { get; set; }
        [DefaultValue("")]
        public string OutputFileName { get; set; }
        [DefaultValue(true)]
        public bool TimestampOutput { get; set; }
    }
}
