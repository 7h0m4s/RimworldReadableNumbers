using System.Collections.Generic;
using HarmonyLib;
using Verse;

namespace RimworldReadableNumbers.Patches.Verse.GenMapUI
{
    [HarmonyPatch]
    [HarmonyPatch(typeof(global::Verse.GenMapUI), "GetPawnLabel")]
    public static class GenMapUIGetPawnLabelPatch
    {
        // [HarmonyPrefix]
        // public static bool Prefix(Pawn pawn, float truncateToWidth, Dictionary<string, string> truncatedLabelsCache, GameFont font)
        // {
        //     return true;
        // }
        
        [HarmonyPostfix]
        public static void Postfix(ref string __result)
        {
            Utility.Patching.SkipReadableNumberFormatting = false;
            Utility.Processing.ProcessLabel(ref __result);
            Utility.Patching.SkipReadableNumberFormatting = true;
        }
        
    }
}