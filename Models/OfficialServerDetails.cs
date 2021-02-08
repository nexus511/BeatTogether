using BeatTogether.Models.Interfaces;

namespace BeatTogether.Models
{
    /// <summary>
    /// Object used as a dummy for the official servers.
    ///
    /// We try to avoid messing with the values ourselves, so we just impelemt
    /// a dummy here, that we can mainly add to the list of servers to have
    /// it show up in the user settings.
    /// </summary>
    class OfficialServerDetails : IServerDetails
    {
        internal readonly static string OFFICIAL_SERVER_NAME = "Official Servers";

        internal static string OfficialStatusUri { set; get; }

        public override string ToString() => ServerName;

        #region IServerDetails
        public bool IsOfficial => true;

        public string Identifier => ServerName;

        public string ServerName => OFFICIAL_SERVER_NAME;

        public string HostName => null;

        public int Port => CustomServerDetails.DEFAULT_PORT;

        public string StatusUri { get => OfficialStatusUri; }

        public MasterServerEndPoint GetEndPoint() => null;

        public bool Is(string serverId) => serverId.Equals(Identifier);
        #endregion
    }
}
