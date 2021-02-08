using BeatTogether.Configuration.Interfaces;

namespace BeatTogether.Models
{
    internal class ServerConfig : IServerConfig
    {
        public readonly static int DEFAULT_PORT = 2328;

        public string ServerName { get; set; }

        public string HostName { get; set; }

        public int Port { get; set; } = DEFAULT_PORT;

        public string StatusUri { get; set; }
    }
}
