using MasterServer;
using BeatTogether.Providers.Interfaces;

namespace BeatTogether.Providers
{
    internal class GameClassInstanceProvider : IGameClassInstanceProvider
    {
        public UserMessageHandler UserMessageHandler { get; set; }

        public MultiplayerModeSelectionViewController MultiplayerModeSelectionViewController { get; set; }
    }
}
