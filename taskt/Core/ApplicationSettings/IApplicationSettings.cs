namespace taskt.Core
{
    public interface IApplicationSettings
    {
        /// <summary>
        /// taskt Script Engine settings
        /// </summary>
        IEngineSettings EngineSettings { get; }

        /// <summary>
        /// taskt Client Settings
        /// </summary>
        IClientSettings ClientSettings { get; }

        /// <summary>
        /// Server settings
        /// </summary>
        IServerSettings ServerSettings { get; }

        /// <summary>
        /// LocalListener Settings
        /// </summary>
        ILocalListenerSettings ListenerSettings { get; }
    }
}
