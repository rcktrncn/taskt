using System;

namespace taskt.Core
{
    /// <summary>
    /// ServerSettings
    /// </summary>
    public sealed class SafeServerSettings : IServerSettings
    {
        /// <summary>
        /// ServerSettings for protect
        /// </summary>
        private readonly ServerSettings serverSettings;

        public SafeServerSettings(ServerSettings serverSettings)
        {
            this.serverSettings = serverSettings;
        }

        public bool ServerConnectionEnabled
        {
            get
            {
                return serverSettings.ServerConnectionEnabled;
            }
        }
        public bool ConnectToServerOnStartup
        {
            get
            {
                return serverSettings.ConnectToServerOnStartup;
            }
        }
        public bool RetryServerConnectionOnFail
        {
            get
            {
                return serverSettings.RetryServerConnectionOnFail;
            }
        }
        public bool BypassCertificateValidation
        {
            get
            {
                return serverSettings.BypassCertificateValidation;
            }
        }
        public string ServerURL
        {
            get
            {
                return serverSettings.ServerURL;
            }
        }
        public string ServerPublicKey
        {
            get
            {
                return serverSettings.ServerPublicKey;
            }
        }
        public string HTTPServerURL
        {
            get
            {
                return serverSettings.HTTPServerURL;
            }
        }
        public Guid HTTPGuid
        {
            get
            {
                return serverSettings.HTTPGuid;
            }
        }
    }
}
