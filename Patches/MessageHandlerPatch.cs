﻿using BeatTogether.Providers;
using HarmonyLib;
using MasterServer;
using BeatTogether.Providers;

namespace BeatTogether.Patches
{
    [HarmonyPatch(typeof(MessageHandler), "Dispose")]
    internal class DisposePatch
    {
        internal static void Prefix(MessageHandler __instance)
        {
            var provider = BeatTogetherCore.instance.InstanceProvider;
            if (provider.UserMessageHandler != __instance)
                return;

            provider.UserMessageHandler = null;
        }
    }
}
