namespace taskt.Core
{
    public interface ILocalListenerSettings
    {
        /// <summary>
        /// start automatically LocalListener on Start Up
        /// </summary>
        bool StartListenerOnStartup { get; }

        /// <summary>
        /// enabled Local Listneing
        /// </summary>
        bool LocalListeningEnabled { get; }

        /// <summary>
        /// Require LocalListner Authentication Key
        /// </summary>
        bool RequireListenerAuthenticationKey { get; }

        /// <summary>
        /// LocalListener Port
        /// </summary>
        int ListeningPort { get; }

        /// <summary>
        /// Local Listener Authentication Key
        /// </summary>
        string AuthKey { get; }

        /// <summary>
        /// Enable LocalListener White List
        /// </summary>
        bool EnableWhitelist { get; }

        /// <summary>
        /// LocalListner IP White list
        /// </summary>
        string IPWhiteList { get; }
    }
}
