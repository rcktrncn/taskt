namespace taskt.Core
{
    /// <summary>
    /// LocalListenerSettings
    /// </summary>
    public sealed class SafeLocalListenerSettings: ILocalListenerSettings
    {
        /// <summary>
        /// LocalListenerSettings for protect
        /// </summary>
        private readonly LocalListenerSettings localListenerSettings;

        public SafeLocalListenerSettings(LocalListenerSettings localListenerSettings)
        {
            this.localListenerSettings = localListenerSettings;
        }

        public bool StartListenerOnStartup
        {
            get
            {
                return localListenerSettings.StartListenerOnStartup;
            }
        }
        public bool LocalListeningEnabled
        {
            get
            {
                return localListenerSettings.LocalListeningEnabled;
            }
        }
        public bool RequireListenerAuthenticationKey
        {
            get
            {
                return localListenerSettings.RequireListenerAuthenticationKey;
            }
        }
        public int ListeningPort
        {
            get
            {
                return localListenerSettings.ListeningPort;
            }
        }
        public string AuthKey
        {
            get
            {
                return localListenerSettings.AuthKey;
            }
        }
        public bool EnableWhitelist
        {
            get
            {
                return localListenerSettings.EnableWhitelist;
            }
        }
        public string IPWhiteList
        {
            get
            {
                return localListenerSettings.IPWhiteList;
            }
        }
    }
}
