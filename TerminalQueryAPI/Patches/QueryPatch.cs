using HarmonyLib;
using LevelGeneration;
using Interpreter = LevelGeneration.LG_ComputerTerminalCommandInterpreter;
using TerminalQueryAPI.Modules;

namespace TerminalQueryAPI.Patches
{
    [HarmonyPatch]
    internal class QueryPatch
    {
        [HarmonyPatch(typeof(Interpreter), nameof(Interpreter.Query))]
        [HarmonyPrefix]
        public static bool QueryOverride(Interpreter __instance, string param1)
        {
            if (!LG_LevelInteractionManager.TryGetTerminalInterface(param1.ToUpper(), __instance.m_terminal.SpawnNode.m_dimension.DimensionIndex, out iTerminalItem target)) return true;

            return !TerminalItemManager.TryDoQueryOutput(target, __instance);
        }
    }
}
