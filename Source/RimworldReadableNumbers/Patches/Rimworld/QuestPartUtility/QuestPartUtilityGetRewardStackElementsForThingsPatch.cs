using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;

namespace RimworldReadableNumbers.Patches.Rimworld.QuestPartUtility
{
    public static class QuestPartUtilityGetRewardStackElementsForThingsPatch
    {
        [HarmonyPatch(typeof(RimWorld.QuestPartUtility),
            nameof(RimWorld.QuestPartUtility.GetRewardStackElementsForThings),
            new Type[] { typeof(IEnumerable<Reward_Items.RememberedItem>) })]
        [HarmonyPatch(MethodType.Enumerator)]
        public static class EnumerableRememberedItemsPatch
        {
            [HarmonyTranspiler]
            public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                MethodInfo methodToFind = AccessTools.Method(typeof(global::Verse.Text), nameof(global::Verse.Text.CalcSize));
                MethodInfo methodToCall = AccessTools.Method(typeof(RimworldReadableNumbers.Utility.Patching), nameof(RimworldReadableNumbers.Utility.Patching.TextCalcSizeWithFormatting));
        
                // Find every occurrence of methodToFind and replace with methodToCall
                return instructions.MethodReplacer(methodToFind, methodToCall).ToList();
            }
        }

        [HarmonyPatch(typeof(RimWorld.QuestPartUtility),
            nameof(RimWorld.QuestPartUtility.GetRewardStackElementsForThings),
            new Type[] { typeof(IEnumerable<Thing>), typeof(bool) })]
        [HarmonyPatch(MethodType.Enumerator)]
        public static class EnumerableThingPatch
        {
            [HarmonyTranspiler]
            public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                MethodInfo methodToFind = AccessTools.Method(typeof(global::Verse.Text), nameof(global::Verse.Text.CalcSize));
                MethodInfo methodToCall = AccessTools.Method(typeof(RimworldReadableNumbers.Utility.Patching), nameof(RimworldReadableNumbers.Utility.Patching.TextCalcSizeWithFormatting));
        
                // Find every occurrence of methodToFind and replace with methodToCall
                return instructions.MethodReplacer(methodToFind, methodToCall).ToList();
            }
        }
    }
}