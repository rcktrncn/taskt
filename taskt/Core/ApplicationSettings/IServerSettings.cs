namespace taskt.Core
{
    public interface IServerSettings
    {
        /// <summary>
        /// Server Connection Enabled
        /// </summary>
        bool ServerConnectionEnabled { get; }

        /// <summary>
        /// Connect to Server on Startup
        /// </summary>
        bool ConnectToServerOnStartup { get; }

        /// <summary>
        /// Retry Server connection on Fail
        /// </summary>
        bool RetryServerConnectionOnFail { get; }

        /// <summary>
        /// Bypass Certification validation when connect server
        /// </summary>
        bool BypassCertificateValidation { get; }

        /// <summary>
        /// Server URL to connect
        /// </summary>
        string ServerURL { get; }

        /// <summary>
        /// Server Public Key to connect
        /// </summary>
        string ServerPublicKey { get; }

        /// <summary>
        /// HTTP Server URL
        /// </summary>
        string HTTPServerURL { get; }

        /// <summary>
        /// HTTP Uid
        /// </summary>
        System.Guid HTTPGuid { get; }
    }
}
