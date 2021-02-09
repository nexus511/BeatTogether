using System.Linq;
using BeatTogether.Providers;
using BeatTogether.UI;
using HarmonyLib;

namespace BeatTogether.Patches
{
    [HarmonyPatch(typeof(MultiplayerModeSelectionViewController), "DidActivate")]
    internal class DidActivatePatch2
    {
        internal static void Postfix(MultiplayerModeSelectionViewController __instance, bool firstActivation)
        {
            GameEventDispatcher.Instance.OnMultiplayerViewEntered(__instance);

            if (firstActivation)
            {
                var provider = BeatTogetherCore.instance.InstanceProvider as GameClassInstanceProvider;
                provider.MultiplayerModeSelectionViewController = __instance;
                AddServerSelection(__instance);
            }
        }

        private static void AddServerSelection(MultiplayerModeSelectionViewController __instance)
        {
            var servers = BeatTogetherCore.instance.Servers;
            var serverSelection = UIFactory.CreateServerSelectionView(__instance);

            var detailsProvider = BeatTogetherCore.instance;
            serverSelection.values = servers.ToList<object>();
            serverSelection.Value = detailsProvider?.SelectedServer;
        }
    }
}
