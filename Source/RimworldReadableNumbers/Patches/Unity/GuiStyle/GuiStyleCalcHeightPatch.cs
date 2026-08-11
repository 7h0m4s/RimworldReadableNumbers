using System;
using HarmonyLib;
using RimworldReadableNumbers.Utility;
using UnityEngine;

namespace RimworldReadableNumbers.Patches.Unity.GuiStyle
{
    [HarmonyPatch]
    public static class GuiStyleCalcHeightPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(UnityEngine.GUIStyle), nameof(GUIStyle.CalcHeight))]
        public static bool Prefix(GUIContent content, float width)
        {
            if (Utility.Patching.DisableReadableNumberFormatting) return true;
            Patching.FormatGuiContentText( ref content);
            return true;
        }
    }
}