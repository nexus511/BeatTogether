using System.Collections.Generic;
using System.Linq;
using BeatTogether.Models;
using BeatTogether.Models.Interfaces;
using BeatTogether.Configuration;

namespace BeatTogether.Providers
{
    /// <summary>
    /// Class providing access to the server selection and the list of
    /// selectable servers.
    /// </summary>
    public class ServerDetailProvider
    {
        private HashSet<string> ConfiguredServerIds { get; set; }

        private readonly PluginConfiguration _config;

        /// <summary>
        /// Contains the list of all configured servers that can be selected
        /// by the user.
        /// </summary>
        public List<IServerDetails> Servers { get; private set; }

        /// <summary>
        /// The server that has been currently selected by the user.
        /// </summary>
        public IServerDetails SelectedServer { get; private set; }

        public void SetSelectedServer(IServerDetails server)
        {
            SelectedServer = server;
            if (ConfiguredServerIds.Contains(server.Identifier))
                _config.SelectedServer = server.Identifier;
        }

        #region internal
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
        }
        #endregion
    }
}
