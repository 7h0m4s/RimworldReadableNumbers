using System.Collections.Generic;
using HarmonyLib;
using Verse;

namespace RimworldReadableNumbers.Patches.Verse.GenMapUI
{
    
    public static class GenMapUIGetPawnLabelPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(global::Verse.GenMapUI), "GetPawnLabel")]
        public static void Postfix(ref string __result)
        {
            Utility.Patching.SkipReadableNumberFormatting = false;
            Utility.Processing.ProcessLabel(ref __result);
            Utility.Patching.SkipReadableNumberFormatting = true;
        }
        
    }
}