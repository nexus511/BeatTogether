using BeatTogether.Providers;
using HarmonyLib;
using MasterServer;

namespace BeatTogether.Patches
{
    [HarmonyPatch(typeof(MessageHandler), "Dispose")]
    internal class DisposePatch
    {
        internal static void Prefix(MessageHandler __instance)
        {
            var provider = BeatTogetherCore.instance.InstanceProvider as GameClassInstanceProvider;
            if (provider.UserMessageHandler != __instance)
                return;

            provider.UserMessageHandler = null;
        }
    }
}
