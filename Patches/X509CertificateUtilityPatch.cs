using HarmonyLib;
using BeatTogether.Providers;

namespace BeatTogether.Patches
{
    [HarmonyPatch(typeof(X509CertificateUtility), "ValidateCertificateChain")]
    internal class ValidateCertificateChainPatch
    {
        internal static bool Prefix()
        {

            var server = BeatTogetherCore.instance.SelectedServer;
            if (server.IsOfficial)
                return true;

            // It'd be best if we do certificate validation here...
            // but for now we'll just skip it.
            return false;
        }
    }
}
