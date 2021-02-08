using System.Collections.Generic;
using BeatTogether.Configuration;
using BeatTogether.Models.Interfaces;

namespace BeatTogether.Providers
{
    /// <summary>
    /// Factory creating and providing access to the core components of the
    /// BeatTogether mod.
    /// 
    /// This would be the entry point when acessing things like the server
    /// selection or hooking connection update events.
    /// </summary>
    public class BeatTogetherCore : PersistentSingleton<BeatTogetherCore>
    {
        /// <summary>
        /// Use this property to retrieve information on the current and
        /// possible server selections as well as adding new servers to
        /// the list.
        /// </summary>
        public ServerDetailProvider ServerDetailProvider { get; private set; }

        /// <summary>
        /// Use the status provider to access information on the online
        /// status of the different servers.
        /// </summary>
        public ServerStatusProvider StatusProvider { get; private set; }

        /// <summary>
        /// Instance of the GameClassInstanceProvider to allow acessing
        /// some of the games internal structures hooked by the mod.
        /// </summary>
        public GameClassInstanceProvider InstanceProvider { get; private set; }

        /// <summary>
        /// Shortcut to ServerDetailsProvider.Selected.
        /// 
        /// Returns the server currently selected.
        /// </summary>
        public IServerDetails SelectedServer => ServerDetailProvider?.SelectedServer;

        /// <summary>
        /// Shortcut to ServerDetailsProvider.Servers.
        /// 
        /// Returns the list of available servers for selection.
        /// </summary>
        public List<IServerDetails> Servers => ServerDetailProvider?.Servers;

        #region internal methods

        internal void Init(PluginConfiguration configuration)
        {
            Plugin.Logger?.Info("Initializing BeatTogetherCore");
            ServerDetailProvider = new ServerDetailProvider(configuration);
            StatusProvider = new ServerStatusProvider();
            InstanceProvider = new GameClassInstanceProvider();
        }

        #endregion

        #region private

        private BeatTogetherCore()
        {
        }

        protected override void OnDestroy()
        {
            Plugin.Logger?.Info("Destroying BeatTogetherCore");
            base.OnDestroy();
        }
        #endregion
    }
}
