using BepInEx;
using BepInEx.Unity.IL2CPP;
using GTFO.API;
using HarmonyLib;
using System.Linq;
using TerminalQueryAPI.Modules;

namespace TerminalQueryAPI
{
    [BepInPlugin(QueryableAPI.PLUGIN_GUID, "TerminalQueryAPI", VersionInfo.Version)]
    [BepInDependency("dev.gtfomodding.gtfo-api", BepInDependency.DependencyFlags.HardDependency)]
    internal class EntryPoint : BasePlugin
    {
        private Harmony _Harmony = null;

        public override void Load()
        {
            _Harmony = new Harmony($"{VersionInfo.RootNamespace}.Harmony");
            _Harmony.PatchAll();

            GTFO.API.LevelAPI.OnLevelCleanup += TerminalItemManager.Clear;
        }

        public override bool Unload()
        {
            _Harmony.UnpatchSelf();
            return base.Unload();
        }
    }
}
