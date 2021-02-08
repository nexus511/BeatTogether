using BeatTogether.Models.Interfaces;

namespace BeatTogether.Models
{
    /// <summary>
    /// Object describing a server.
    /// </summary>
    public class CustomServerDetails : IServerDetails
    {

        /// <summary>
        /// Default port used by the multiplayer master server.
        /// </summary>
        public readonly static int DEFAULT_PORT = 2328;

        public CustomServerDetails()
        {
        }

        internal CustomServerDetails(ServerConfig config)
        {
            ServerName = config.ServerName;
            HostName = config.HostName;
            Port = config.Port;
            StatusUri = config.StatusUri;
        }

        public override string ToString() => ServerName;

        #region IServerDetails
        public bool IsOfficial => false;

        public string Identifier => ServerName;

        public string ServerName { get; set; }

        public string HostName { get; set; }

        public int Port { get; set; }

        public string StatusUri { get; set; }

        public MasterServerEndPoint GetEndPoint()
            => new MasterServerEndPoint(HostName, Port);

        public bool Is(string serverId) => serverId.Equals(Identifier);
        #endregion
    }
}
