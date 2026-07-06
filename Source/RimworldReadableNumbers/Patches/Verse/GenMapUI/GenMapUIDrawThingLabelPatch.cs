using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace RimworldReadableNumbers.Patches.Verse.GenMapUI
{
    [HarmonyPatch(typeof(global::Verse.GenMapUI), nameof(global::Verse.GenMapUI.DrawThingLabel),new Type[] { typeof(Vector2), typeof(string), typeof(Color) })]
    public static class GenMapUIDrawThingLabelPatch
    {
        
        [HarmonyPrefix]
        public static bool Prefix(Vector2 screenPos, ref string text, Color textColor)
        {
            Utility.Processing.ProcessLabel(ref text);
            Utility.Patching.SkipReadableNumberFormatting = true;
            return true;
        }

        [HarmonyPostfix]
        public static void Postfix()
        {
            Utility.Patching.SkipReadableNumberFormatting = false;
        }
    }
}