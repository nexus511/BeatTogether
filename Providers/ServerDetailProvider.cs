using System;
using System.Collections.Generic;
using System.Linq;
using BeatTogether.Models;
using BeatTogether.Models.Interfaces;
using BeatTogether.Configuration;
using BeatTogether.Providers.Interfaces;

namespace BeatTogether.Providers
{
    internal class ServerDetailProvider : IServerDetailProvider
    {
        private HashSet<string> ConfiguredServerIds { get; set; }

        private readonly PluginConfiguration _config;

        #region IServerDetailsProvider Methods

        public event EventHandler<IServerDetails> ServerSelectionChanged;

        public event EventHandler<List<IServerDetails>> ServerListChanged;

        public List<IServerDetails> Servers { get; private set; }

        public IServerDetails SelectedServer { get; private set; }

        public void SetSelectedServer(IServerDetails server)
            => UpdateServerSelectionInt?.Invoke(this, server);
        #endregion

        #region Internal Methods

        internal EventHandler<IServerDetails> UpdateServerSelectionInt;

        internal void OnSetSelectedServer(object sender, IServerDetails server)
        {
            if (SelectedServer == server)
                return;

            SelectedServer = server;
            if (ConfiguredServerIds.Contains(server.Identifier))
                _config.SelectedServer = server.Identifier;

            ServerSelectionChanged?.Invoke(this, server);
        }

        internal ServerDetailProvider(PluginConfiguration config)
        {
            _config = config;
            Servers = config.Servers
                .ConvertAll<IServerDetails>(s => new CustomServerDetails(s))
                .Append(new OfficialServerDetails())
                .ToList();

            ConfiguredServerIds = Servers
                .ConvertAll(s => s.Identifier)
                .ToHashSet();

            SelectedServer = Servers.FirstOrDefault(server => server.Is(config.SelectedServer));
            if (SelectedServer == null)
                SelectedServer = Servers.First();

            UpdateServerSelectionInt += OnSetSelectedServer;
        }
        #endregion
    }
}
