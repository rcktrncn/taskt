using System.Diagnostics;
using System.Windows.Forms;
using taskt.Core;

namespace taskt
{
    public static class App
    {
        /// <summary>
        /// taskt location
        /// </summary>
        public static string Taskt_Location { get; private set; }
        /// <summary>
        /// taskt version info
        /// </summary>
        public static FileVersionInfo Taskt_VersionInfo { get; private set; }
        /// <summary>
        /// taskt settings file path
        /// </summary>
        public static string Taskt_Settings_File_Path { get; private set; }
        /// <summary>
        /// application settings
        /// </summary>
        public static ApplicationSettings Taskt_Settings { get; set; }

        /// <summary>
        /// update location, version info
        /// </summary>
        public static void UpdateLocationAndVersionInfo()
        {
            Taskt_Location = System.Reflection.Assembly.GetExecutingAssembly().Location;
            Taskt_VersionInfo = FileVersionInfo.GetVersionInfo(Taskt_Location);
        }

        /// <summary>
        /// update settings
        /// </summary>
        /// <param name="settingsFilePath"></param>
        /// <returns></returns>
        public static bool UpdateSettings(string settingsFilePath)
        {
            // update settings file path
            if (Program.AllowChangeSettingsFilePathSwitch)
            {
                if (!string.IsNullOrEmpty(settingsFilePath))
                {
                    Taskt_Settings_File_Path = settingsFilePath;
                }
                else
                {
                    Taskt_Settings_File_Path = Core.IO.Files.GetSettigsFilePath();
                }
            }

            // update settings
            try
            {
                // when the settings file does not exists, create the file automatically
                Taskt_Settings = ApplicationSettings.GetOrCreateApplicationSettings(Taskt_Settings_File_Path);
                return true;
            }
            catch
            {
                MessageBox.Show($"Fail load settings file.\r\nPath: {Taskt_Settings_File_Path}", Taskt_VersionInfo.ProductName);
                return false;
            }
        }

        /// <summary>
        /// update all
        /// </summary>
        /// <param name="settingsFilePath"></param>
        public static bool UpdateAll(string settingsFilePath)
        {
            UpdateLocationAndVersionInfo();
            return UpdateSettings(settingsFilePath);
        }
    }
}
