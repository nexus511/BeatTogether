namespace BeatTogether.Models.Interfaces
{
    /// <summary>
    /// Defining extended detail helpers on a server object.
    /// </summary>
    public interface IExtendedDetails
    {
        /// <summary>
        /// Returns true, if the implemented server object references the
        /// official servers.
        /// </summary>
        bool IsOfficial { get; }

        /// <summary>
        /// Creates and returns an instance of the MasterServerEndPoint
        /// that will be used to connect to the server.
        /// 
        /// For the official servers, this will always be null.
        /// </summary>
        /// <returns>Server endpoint to be used on connect.</returns>
        MasterServerEndPoint GetEndPoint();

        /// <summary>
        /// Checks, if the given server identifier matches the server.
        /// 
        /// The identifier currently equals to the human readable name of
        /// the server, but you should use the Identifier property to make
        /// sure that your code still works in the future.
        /// </summary>
        /// <param name="serverId">Identifier of the server.</param>
        /// <returns>True, if the identifier is matched.</returns>
        bool Is(string serverId);

        /// <summary>
        /// Returns the identifier for the server.
        /// </summary>
        string Identifier { get; }

        /// <summary>
        /// The ToString method needs to be overwritten, as this ensures that
        /// the name of the server is properly displayed in the server
        /// selection.
        /// </summary>
        /// <returns>Returns the name of the server.</returns>
        string ToString();
    }
}
