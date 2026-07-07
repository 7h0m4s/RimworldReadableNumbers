using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using Verse;

namespace RimworldReadableNumbers.Patches.Rimworld.DateReadout
{
    [HarmonyPatch(typeof(RimWorld.DateReadout), // Target type
        nameof(RimWorld.DateReadout.DateOnGUI))] //Target method, can be written as a plain string if private
    public static class DateReadoutPatch
    {
        [HarmonyPrefix]
        public static bool Prefix()
        {
            Utility.Patching.DisableReadableNumberFormatting = true;
            return true;
        }

        [HarmonyPostfix]
        public static void Postfix()
        {
            Utility.Patching.DisableReadableNumberFormatting = false;
        }
    }
}



