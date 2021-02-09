using System.Collections.Generic;
using BeatTogether.Models.Interfaces;
using BeatTogether.Providers.Interfaces;

namespace BeatTogether.Providers
{
    internal class ServerStatusProvider : IServerStatusProvider
    {
        private readonly Dictionary<string, MasterServerAvailabilityData> _serverStatus;

        #region IServerStatusProvider

        public MasterServerAvailabilityData GetServerStatus(IServerDetails server)
        {
            if (_serverStatus.TryGetValue(server.Identifier, out var status))
                return status;
            return null;
        }
        #endregion

        #region Internal Methods

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
