using System.IO;

namespace taskt.Core.IO
{
    public static class Files
    {
        /// <summary>
        /// taskt settings file name
        /// </summary>
        public const string TASKT_SETTINGS_FILE_NAME = "AppSettings.xml";

        /// <summary>
        /// get taskt settigs file path
        /// </summary>
        /// <returns></returns>
        public static string GetSettigsFilePath()
        {
            return Path.Combine(Folders.GetSettingsFolderPath(), TASKT_SETTINGS_FILE_NAME);
        }

        /// <summary>
        /// get tast settigs file path
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetSettigsFilePath(string fileName)
        {
            if (Path.IsPathRooted(fileName))
            {
                return fileName;
            }
            else
            {
                return Path.Combine(Folders.GetSettingsFolderPath(), fileName);
            }
        }
    }
}
