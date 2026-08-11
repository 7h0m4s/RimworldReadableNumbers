using System;
using HarmonyLib;
using RimworldReadableNumbers.Utility;
using UnityEngine;

namespace RimworldReadableNumbers.Patches.Unity.GuiStyle
{
    [HarmonyPatch]
    public static class GuiStyleCalcMinMaxWidthPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(UnityEngine.GUIStyle), nameof(GUIStyle.CalcMinMaxWidth),
            new Type[] { typeof(GUIContent), typeof(float), typeof(float)},
            new ArgumentType[]
            {
                ArgumentType.Normal,
                ArgumentType.Out,
                ArgumentType.Out
            })]
        public static bool Prefix(GUIContent content, out float minWidth, out float maxWidth)
        {
            minWidth = 0f;
            maxWidth = 0f;
            if (Utility.Patching.DisableReadableNumberFormatting) return true;
            Patching.FormatGuiContentText( ref content);
            return true;
        }
    }
}