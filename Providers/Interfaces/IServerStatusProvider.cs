using BeatTogether.Models.Interfaces;

namespace BeatTogether.Providers.Interfaces
{
    /// <summary>
    /// Provides information on the server status. This information is fetched
    /// from the status endpoints.
    ///
    /// The status information is only available for servers, where the request
    /// has been successful.
    /// </summary>
    public interface IServerStatusProvider
    {
        /// <summary>
        /// Returns the server status information for a single server.
        ///
        /// The value can be requested using the IServerDetails object. If no
        /// status is available for the given server, this method will return
        /// null.
        /// </summary>
        /// <param name="server">The server to request the status from.</param>
        /// <returns>Status for the server or NULL, if not available.</returns>
        MasterServerAvailabilityData GetServerStatus(IServerDetails server);
    }
}
