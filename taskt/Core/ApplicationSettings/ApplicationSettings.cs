using System;
using System.IO;
using System.Xml.Serialization;
using taskt.Core.IO;

namespace taskt.Core
{
    /// <summary>
    /// Defines settings for the entire application
    /// </summary>
    [Serializable]
    public sealed class ApplicationSettings
    {
        #region xml properties
        public ServerSettings ServerSettings { get; set; } = new ServerSettings();
        public EngineSettings EngineSettings { get; set; } = new EngineSettings();
        public ClientSettings ClientSettings { get; set; } = new ClientSettings();
        public LocalListenerSettings ListenerSettings { get; set; } = new LocalListenerSettings();
        #endregion

        public ApplicationSettings()
        {
        }

        /// <summary>
        /// save taskt settigs file as XML
        /// </summary>
        public void Save()
        {
            //create file path
            var filePath = Files.GetSettigsFilePath();

            SaveProcess(this, filePath);
        }

        /// <summary>
        /// Save taskt settigs file as XML, the file name is specified by a argument
        /// </summary>
        /// <param name="fileName"></param>
        public void Save(string fileName)
        {
            var filePath = Files.GetSettigsFilePath(fileName);

            SaveProcess(this, filePath);
        }

        /// <summary>
        /// File Save Process
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="filePath"></param>
        private static void SaveProcess(ApplicationSettings settings, string filePath)
        {
            var settigsDir = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(settigsDir))
            {
                Directory.CreateDirectory(settigsDir);
            }

            // output to xml file
            SaveAs(settings, filePath);
        }

        /// <summary>
        /// save taskt settigs xml file, settigs is specified
        /// </summary>
        /// <param name="settings"></param>
        public static void Save(ApplicationSettings settings)
        {
            //create file path
            var filePath = Files.GetSettigsFilePath();

            SaveProcess(settings, filePath);
        }

        /// <summary>
        /// save taskt settigs xml file, settigs and fileName are specified
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="fileName"></param>
        public static void Save(ApplicationSettings settings, string fileName)
        {
            var filePath = Files.GetSettigsFilePath(fileName);

            SaveProcess(settings, filePath);
        }

        /// <summary>
        /// Save settigs file as XML
        /// </summary>
        /// <param name="appSettings"></param>
        /// <param name="filePath"></param>
        private static void SaveAs(ApplicationSettings appSettings, string filePath)
        {
            using (FileStream fileStream = File.Create(filePath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ApplicationSettings));
                serializer.Serialize(fileStream, appSettings);
                fileStream.Close();
            }
        }

        /// <summary>
        /// get taskt settigs from file or create taskt settigs
        /// </summary>
        /// <returns></returns>
        public static ApplicationSettings GetOrCreateApplicationSettings()
        {
            //create file path
            var filePath = Files.GetSettigsFilePath();

            return GetOrCreateApplicationSettingsProcess(filePath);
        }

        /// <summary>
        /// get taskt settigs from file or create taskt settigs, fileName is specified
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static ApplicationSettings GetOrCreateApplicationSettings(string fileName)
        {
            var filePath = Files.GetSettigsFilePath(fileName);

            return GetOrCreateApplicationSettingsProcess(filePath);
        }

        /// <summary>
        /// get or create taskt settigs process
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private static ApplicationSettings GetOrCreateApplicationSettingsProcess(string filePath)
        {
            ApplicationSettings appSettings;
            if (File.Exists(filePath))
            {
                try
                {
                    appSettings = Open(filePath);
                }
                catch
                {
                    appSettings = new ApplicationSettings();
                }
            }
            else
            {
                appSettings = new ApplicationSettings();
            }

            return appSettings;
        }


        /// <summary>
        /// Open taskt settigs file and convert instance
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static ApplicationSettings Open(string filePath)
        {
            ApplicationSettings appSettings = null;
            using (FileStream fileStream = File.Open(filePath, FileMode.Open))
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(ApplicationSettings));
                    appSettings = (ApplicationSettings)serializer.Deserialize(fileStream);
                }
                catch (Exception ex)
                {
                    //appSettings = new ApplicationSettings();
                    throw ex;
                }
                finally
                {
                    fileStream.Close();
                }
            }
            return appSettings;
        }
    }
}
