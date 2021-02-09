using BeatTogether.Models;

namespace BeatTogether.Configuration.Interfaces
{
    /// <summary>
    /// Interface defining the minimal details avialable to have a server
    /// properly configured.
    /// </summary>
    public interface IServerConfig
    {
        /// <summary>
        /// Human readable name of the server. This will be shown in the server
        /// selection view.
        /// </summary>
        string ServerName { get; }

        /// <summary>
        /// Hostname or IP address of the server.
        /// </summary>
        string HostName { get; }

        /// <summary>
        /// Port the server is running on.
        /// </summary>
        int Port { get; }

        /// <summary>
        /// URI to request the server status. If no server status endpoint
        /// is available, this can be null instead.
        /// </summary>
        string StatusUri { get; }

        /// <summary>
        /// Features the server has enabled.
        /// </summary>
        ConfigFlags.ServerFeatures ServerFeatures { get; }
    }
}
