using System.Collections.Generic;
using BeatTogether.Models.Interfaces;

namespace BeatTogether.Providers
{
    /// <summary>
    /// Provides information on the server status. This information is fetched
    /// from the status endpoints.
    ///
    /// The status information is only available for servers, where the request
    /// has been successful.
    /// </summary>
    public class ServerStatusProvider
    {
        private readonly Dictionary<string, MasterServerAvailabilityData> _serverStatus;

        /// <summary>
        /// Returns the server status information for a single server.
        ///
        /// The value can be requested using the IServerDetails object. If no
        /// status is available for the given server, this method will return
        /// null.
        /// </summary>
        /// <param name="server"></param>
        /// <returns></returns>
        public MasterServerAvailabilityData GetServerStatus(IServerDetails server)
        {
            if (_serverStatus.TryGetValue(server.Identifier, out var status))
                return status;
            return null;
        }

        #region internal
        internal ServerStatusProvider()
        {
            _serverStatus = new Dictionary<string, MasterServerAvailabilityData>();
        }

        internal void SetServerStatus(string identifier, MasterServerAvailabilityData status) =>
            _serverStatus[identifier] = status;
        internal void SetServerStatus(IServerDetails server, MasterServerAvailabilityData status) =>
            SetServerStatus(server.Identifier, status);
        #endregion
    }
}
