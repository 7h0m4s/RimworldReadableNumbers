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


        #region Redundant Label Patches

        // Overrides of GUI.Label below all transitively call one of the overloads above, so are not needed.

        // [HarmonyPatch(typeof(UnityEngine.GUI), "DoLabel",new Type[] { typeof(Rect), typeof(GUIContent), typeof(GUIStyle) })]
        // public static bool Prefix(Rect position, GUIContent content, GUIStyle style)
        // {
        //     string guiContentText = content.text;
        //     Utility.Processing.ProcessLabel(ref guiContentText);
        //     return true;
        // }
        
        // Redirects to (Rect position, GUIContent content, GUIStyle style)
        // [HarmonyPatch(typeof(UnityEngine.GUI), nameof(UnityEngine.GUI.Label),new Type[] { typeof(Rect), typeof(string) })]
        // public static bool Prefix(Rect position, string text)
        // {
        //     Utility.Processing.ProcessLabel(ref text);
        //     return true;
        // }


        // Redirects to (Rect position, GUIContent content, GUIStyle style)
        // [HarmonyPatch(typeof(UnityEngine.GUI), nameof(UnityEngine.GUI.Label), new Type[] { typeof(Rect), typeof(string), typeof(GUIStyle) })]
        // public static bool Prefix(Rect position, string text, GUIStyle style)
        // {
        //     Utility.Processing.ProcessLabel(ref text);
        //     return true;
        // }

        #endregion Redundant Label Patches
    }
}