using BeatTogether.Models;

namespace BeatTogether.Configuration
{
    internal class ServerConfig
    {
        public readonly static int DEFAULT_PORT = 2328;

        public string ServerName { get; set; }

        public string HostName { get; set; }

        public int Port { get; set; } = DEFAULT_PORT;

        public string StatusUri { get; set; }

        public ConfigFlags.ServerFeatures ServerFeatures { get; set; }
            = ConfigFlags.ServerFeatures.JoinGame | ConfigFlags.ServerFeatures.CreateGame;
    }
}
