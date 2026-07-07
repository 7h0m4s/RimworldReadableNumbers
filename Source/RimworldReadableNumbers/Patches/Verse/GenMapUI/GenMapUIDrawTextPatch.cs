using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace RimworldReadableNumbers.Patches.Verse.GenMapUI
{
    [HarmonyPatch(typeof(global::Verse.GenMapUI), nameof(global::Verse.GenMapUI.DrawText),new Type[] { typeof(Vector2), typeof(string), typeof(Color) })]
    public static class GenMapUIDrawTextPatch
    {
        [HarmonyPrefix]
        public static bool Prefix(Vector2 worldPos, ref string text, Color textColor)
        {
            Utility.Processing.ProcessLabel(ref text);
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