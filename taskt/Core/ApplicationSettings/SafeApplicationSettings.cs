namespace taskt.Core
{
    /// <summary>
    /// Safe Application Settings, Most parameters are readonly
    /// </summary>
    public sealed class SafeApplicationSettings
    {
        /// <summary>
        /// appSettings for protect
        /// </summary>
        private readonly ApplicationSettings applicationSettings;

        public SafeClientSettings ClientSettings { get; private set; }

        public SafeEngineSettings EngineSettings { get; private set; }

        public SafeServerSettings ServerSettings { get; private set; }

        public SafeLocalListenerSettings LocalListenerSettings { get; private set; }

        public SafeApplicationSettings(ApplicationSettings appSettings) 
        { 
            this.applicationSettings = appSettings;

            this.ClientSettings = new SafeClientSettings(appSettings.ClientSettings);
            this.EngineSettings = new SafeEngineSettings(appSettings.EngineSettings);
            this.ServerSettings = new SafeServerSettings(appSettings.ServerSettings);
            this.LocalListenerSettings = new SafeLocalListenerSettings(appSettings.ListenerSettings);
        }
    }   
}
