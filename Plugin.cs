using System.Reflection;
using BeatTogether.Configuration;
using BeatTogether.Providers;
using HarmonyLib;
using IPA;
using IPA.Config;
using IPA.Config.Stores;
using IPALogger = IPA.Logging.Logger;
using UnityEngine;

namespace BeatTogether
{

    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        private const string _harmonyId = "com.Python.BeatTogether";
        internal static Harmony Harmony { get; private set; }
        private static PluginConfiguration Configuration { get; set; }
        internal static IPALogger Logger { get; private set; }
        private BeatTogetherCore Factory { get; set; }

        [OnStart]
        public void OnApplicationStart()
        {
        }

        [Init]
        public void Init(
            IPALogger logger,
            Config config)
        {
            Harmony = new Harmony(_harmonyId);

            Configuration = config.Generated<PluginConfiguration>();
            Logger = logger;

            Factory = new GameObject("BeatTogetherModelFactory").AddComponent<BeatTogetherCore>();
            Factory.Init(Configuration);
        }

        [OnEnable]
        public void OnEnable()
            => Harmony.PatchAll(Assembly.GetExecutingAssembly());

        [OnDisable]
        public void OnDisable()
            => Harmony.UnpatchAll(_harmonyId);
    }
}
