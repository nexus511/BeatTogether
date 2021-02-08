using System.Collections.Generic;
using System.Linq;
using BeatTogether.Models;

namespace BeatTogether.Providers
{
    public class ServerDetailProvider
    {
        public static ServerDetailProvider Instance { get; }
            = new ServerDetailProvider();

        public List<ServerDetails> Servers { get; private set; }

        public ServerDetails SelectedServer { get; set; }

        public ServerDetailProvider()
        {
            Servers = Plugin.Configuration.Servers
                .Append(ServerDetails.CreateOfficialInstance())
                .ToList();
            SelectedServer = Servers.FirstOrDefault(server => server.Is(Plugin.Configuration.SelectedServer));
            if (SelectedServer == null)
                SelectedServer = Servers.First();
        }
    }
}
