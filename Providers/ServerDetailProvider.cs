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


        public bool AddTemporaryServer(IServerDetails server, bool select = false)
        {
            if (ConfiguredServerIds.Contains(server.Identifier))
                return false;

            if (Servers.Find(s => s.Identifier == server.Identifier) != null)
                return false;

            Plugin.Logger.Info("Adding new server (" +
                $"name='{server.ServerName}', " +
                $"hostname='{server.HostName}, " +
                $"port='{server.Port}', " +
                $"select='{select})"
            );

            Servers.Append(server);
            ServerListChanged?.Invoke(this, Servers);

            if (select)
                SetSelectedServer(server);

            return true;
        }

        public bool RemoveTemporaryServer(IServerDetails server)
        {
            if (ConfiguredServerIds.Contains(server.Identifier))
                return false;

            Plugin.Logger.Info($"Removing server {server}");
            if (SelectedServer == server)
                SetSelectedServer(Servers.First());

            if (Servers.Remove(server))
            {
                ServerListChanged?.Invoke(this, Servers);
                return true;
            }
            return false;
        }

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
