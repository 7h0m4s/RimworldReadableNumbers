using System.Collections.Generic;
using HarmonyLib;
using Verse;

namespace RimworldReadableNumbers.Patches.Verse.GenMapUI
{
    [HarmonyPatch]
    [HarmonyPatch(typeof(global::Verse.GenMapUI), "GetPawnLabel")]
    public static class GenMapUIGetPawnLabelPatch
    {
        [HarmonyPostfix]
        public static void Postfix(ref string __result)
        {
            bool previousState = Utility.Patching.DisableReadableNumberFormatting;
            Utility.Patching.DisableReadableNumberFormatting = false;
            Utility.Processing.ProcessLabel(ref __result);
            Utility.Patching.DisableReadableNumberFormatting = previousState;
        }
    }
}