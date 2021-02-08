using System.Threading.Tasks;
using BeatTogether.Providers;
using HarmonyLib;
using UnityEngine;

namespace BeatTogether.Patches
{
    [HarmonyPatch(typeof(MasterServerAvailabilityModel), "GetAvailabilityAsyncInternal")]
    class GetAvailabilityAsyncInternalPatch
    {
        internal static void Postfix(ref Task<MasterServerAvailabilityData> __result)
        {
            var detailsProvider = BeatTogetherCore.instance.ServerDetailProvider;
            var statusProvider = BeatTogetherCore.instance.StatusProvider;
            var serverStatusFetcher = new ServerStatusFetcher(detailsProvider?.Servers, statusProvider);
            _ = serverStatusFetcher.FetchAll();
            __result = Task.FromResult(new MasterServerAvailabilityData()
            {
                minimumAppVersion = Application.version,
                status = MasterServerAvailabilityData.AvailabilityStatus.Online
            });
        }
    }
}
