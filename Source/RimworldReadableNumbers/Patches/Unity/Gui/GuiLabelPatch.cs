using System;
using HarmonyLib;
using RimworldReadableNumbers.Utility;
using Unity.Mathematics;
using UnityEngine;
using Text = Verse.Text;

namespace RimworldReadableNumbers.Patches.Unity.Gui
{
    [HarmonyPatch]
    public static class GuiLabelPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(UnityEngine.GUI), nameof(UnityEngine.GUI.Label),
            new Type[] { typeof(Rect), typeof(GUIContent) })]
        public static bool Prefix(Rect position, GUIContent content)
        {
            if (Utility.Patching.DisableReadableNumberFormatting) return true;
            if (Utility.Patching.IsAlreadyReadableNumberFormatted)
            {
                Utility.Patching.IsAlreadyReadableNumberFormatted = false;
                return true;
            }

            Patching.FormatGuiContentText(ref content);
            return true;
        }


        [HarmonyPrefix]
        [HarmonyPatch(typeof(UnityEngine.GUI), nameof(UnityEngine.GUI.Label),
            new Type[] { typeof(Rect), typeof(GUIContent), typeof(GUIStyle) })]
        public static bool Prefix(Rect position, GUIContent content, GUIStyle style)
        {
            if (Utility.Patching.DisableReadableNumberFormatting) return true;
            if (Utility.Patching.IsAlreadyReadableNumberFormatted)
            {
                Utility.Patching.IsAlreadyReadableNumberFormatted = false;
                return true;
            }

            Patching.FormatGuiContentText(ref content);
            return true;
        }


        [HarmonyPrefix]
        [HarmonyPatch(typeof(UnityEngine.GUI), nameof(UnityEngine.GUI.Label),
            new Type[] { typeof(Rect), typeof(string), typeof(GUIStyle) })]
        public static bool Prefix(Rect position, ref string text, GUIStyle style)
        {
            if (Utility.Patching.DisableReadableNumberFormatting) return true;
            if (Utility.Patching.IsAlreadyReadableNumberFormatted)
            {
                Utility.Patching.IsAlreadyReadableNumberFormatted = false;
                return true;
            }

            Utility.Processing.ProcessLabel(ref text);
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(UnityEngine.GUI), nameof(UnityEngine.GUI.Label),
            new Type[] { typeof(Rect), typeof(string) })]
        public static bool Prefix(Rect position, ref string text)
        {
            if (Utility.Patching.DisableReadableNumberFormatting) return true;
            if (Utility.Patching.IsAlreadyReadableNumberFormatted)
            {
                Utility.Patching.IsAlreadyReadableNumberFormatted = false;
                return true;
            }

            Utility.Processing.ProcessLabel(ref text);
            return true;
        }
    }
}
