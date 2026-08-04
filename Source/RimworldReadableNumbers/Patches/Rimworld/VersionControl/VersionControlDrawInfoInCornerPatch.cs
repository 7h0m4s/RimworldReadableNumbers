using System;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using Verse;

namespace RimworldReadableNumbers.Patches.Rimworld.VersionControl
{
    [HarmonyPatch(typeof(RimWorld.VersionControl), nameof(RimWorld.VersionControl.DrawInfoInCorner))]
    public static class VersionControlDrawInfoInCornerPatch
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