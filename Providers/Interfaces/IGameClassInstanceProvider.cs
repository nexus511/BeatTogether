using MasterServer;

namespace BeatTogether.Providers.Interfaces
{
    /// <summary>
    /// Helper class to provide access to some of the games internal object
    /// instances.
    /// </summary>
    public interface IGameClassInstanceProvider
    {
        /// <summary>
        /// Instance of the UserMessageHandler.
        ///
        /// This is the class handling the network traffic between the game and
        /// the master server.
        /// </summary>
        UserMessageHandler UserMessageHandler { get; }

        /// <summary>
        /// Instance of the MultiplayerModeSelectionViewController providing
        /// the controller to the view where the user can select to create a
        /// new lobby, join an existing one with a code or going for
        /// quickplay.
        /// </summary>
        MultiplayerModeSelectionViewController MultiplayerModeSelectionViewController { get; }
    }
}
