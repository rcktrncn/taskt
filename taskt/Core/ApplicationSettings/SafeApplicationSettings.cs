namespace taskt.Core
{
    /// <summary>
    /// Safe Application Settings, Most parameters are readonly
    /// </summary>
    public sealed class SafeApplicationSettings : IApplicationSettings
    {
        /// <summary>
        /// appSettings for protect
        /// </summary>
        private readonly ApplicationSettings applicationSettings;

        public IClientSettings ClientSettings { get; private set; }

        public IEngineSettings EngineSettings { get; private set; }

        public IServerSettings ServerSettings { get; private set; }

        public ILocalListenerSettings ListenerSettings { get; private set; }

        public SafeApplicationSettings(ApplicationSettings appSettings) 
        { 
            this.applicationSettings = appSettings;

            this.ClientSettings = new SafeClientSettings(appSettings.GetClientSettings());
            this.EngineSettings = new SafeEngineSettings(appSettings.GetEngineSettings());
            this.ServerSettings = new SafeServerSettings(appSettings.GetServerSettings());
            this.ListenerSettings = new SafeLocalListenerSettings(appSettings.GetLocalListenerSettings());
        }

        /// <summary>
        /// get ClientSettings As SafeClientSettings
        /// </summary>
        /// <returns></returns>
        public SafeClientSettings GetClientSettings()
        {
            return (SafeClientSettings)ClientSettings;
        }

        /// <summary>
        /// Get EngineSettings as SafeEngineSettings
        /// </summary>
        /// <returns></returns>
        public SafeEngineSettings GetEngineSettings()
        {
            return (SafeEngineSettings)EngineSettings;
        }

        /// <summary>
        /// get ServerSettings as SafeServerSettings
        /// </summary>
        /// <returns></returns>
        public SafeServerSettings GetServerSettings()
        {
            return (SafeServerSettings)ServerSettings;
        }

        /// <summary>
        /// get LocalListenerSettings as SafeLocalListenerSettings
        /// </summary>
        /// <returns></returns>
        public SafeLocalListenerSettings GetLocalListenerSettings()
        {
            return (SafeLocalListenerSettings)ListenerSettings;
        }
    }   
}
