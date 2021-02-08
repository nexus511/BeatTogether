using System.Collections.Generic;
using System.Linq;
using BeatTogether.Models;
using BeatTogether.Models.Interfaces;
using BeatTogether.Configuration;

namespace BeatTogether.Providers
{
    public class ServerDetailProvider
    {
        public List<IServerDetails> Servers { get; private set; }

        public IServerDetails SelectedServer { get; private set; }

        public void SetSelectedServer(IServerDetails server)
        {
            SelectedServer = server;
            // TODO:
            // Plugin.Configuration.SelectedServer = details.ServerName;
        }

        #region internal
        internal ServerDetailProvider(PluginConfiguration configuration)
        {
            Servers = configuration.Servers
                .ConvertAll<IServerDetails>(s => new ServerDetails(s))
                .Append(new OfficialServerDetails())
                .ToList();

            SelectedServer = Servers.FirstOrDefault(server => server.Is(configuration.SelectedServer));
            if (SelectedServer == null)
                SelectedServer = Servers.First();
        }
        #endregion
    }
}
