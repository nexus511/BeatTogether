using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BeatTogether.Models.Interfaces;
using BeatTogether.Providers.Interfaces;
using UnityEngine;

namespace BeatTogether.Providers
{
    internal class ServerStatusFetcher
    {
        private readonly List<IServerDetails> _serverDetails;
        private readonly ServerStatusProvider _provider;

        public ServerStatusFetcher(List<IServerDetails> servers, IServerStatusProvider provider)
        {
            _serverDetails = servers;
            _provider = provider as ServerStatusProvider;
        }

        public async Task FetchAll()
        {
            var result = await Task.WhenAll(
                _serverDetails
                    .Where(server => server.StatusUri != null)
                    .Select(server => FetchSingle(server))
            );

            foreach (var kvp in result.Where(kvp => kvp.Value != null))
                _provider.SetServerStatus(kvp.Key, kvp.Value);
        }

        #region Private Methods

        private async Task<KeyValuePair<string, MasterServerAvailabilityData>> FetchSingle(IServerDetails server)
        {
            var url = server.StatusUri;
            Plugin.Logger.Debug($"Fetching status for '{server.ServerName}' from '{url}'.");
            var httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromSeconds(30.0);

            try
            {
                var response = await httpClient.GetStringAsync(url);
                Plugin.Logger.Debug($"Finished fetching status for '{server.ServerName}' from '{url}'.");

                return new KeyValuePair<string, MasterServerAvailabilityData>(
                    server.ServerName,
                    JsonUtility.FromJson<MasterServerAvailabilityData>(response)
                );
            }
            catch (Exception e)
            {
                Plugin.Logger.Warn($"Failed to fetch status for '{server.ServerName}' from '{url}'. {e.Message}");
            }
            return new KeyValuePair<string, MasterServerAvailabilityData>(server.ServerName, null);
        }

        #endregion
    }
}
