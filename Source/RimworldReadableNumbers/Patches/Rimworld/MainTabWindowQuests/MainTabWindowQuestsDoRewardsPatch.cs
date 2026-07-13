using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace RimworldReadableNumbers.Patches.Rimworld.MainTabWindowQuests
{
    [HarmonyPatch(typeof(RimWorld.MainTabWindow_Quests), "DoRewards")]
    public static class MainTabWindowQuestsDoRewardsPatch
    {
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            return Utility.Patching.TranspileTranslatorFormattedStringExtensionsTranslate(instructions);
        }
    }
}