using System;
using HarmonyLib;
using RimworldReadableNumbers.Utility;
using UnityEngine;

namespace RimworldReadableNumbers.Patches.Unity.GuiStyle
{
    [HarmonyPatch]
    public static class GuiStyleCalcSizePatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(UnityEngine.GUIStyle), nameof(GUIStyle.CalcSize))]
        public static bool Prefix(GUIContent content)
        {
            if (Utility.Patching.DisableReadableNumberFormatting) return true;
            Patching.FormatGuiContentText( ref content);
            return true;
        }
    }
}