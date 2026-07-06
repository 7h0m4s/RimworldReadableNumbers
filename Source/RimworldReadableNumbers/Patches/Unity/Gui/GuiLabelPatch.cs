using System;
using HarmonyLib;
using UnityEngine;

namespace RimworldReadableNumbers.Patches.Unity.Gui
{
    [HarmonyPatch]
    public class GuiLabelPatch
    {
        
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
        
        [HarmonyPatch(typeof(UnityEngine.GUI), nameof(UnityEngine.GUI.Label), new Type[] { typeof(Rect), typeof(GUIContent) })]
        public static bool Prefix(Rect position, GUIContent content)
        {
            if (Utility.Patching.SkipReadableNumberFormatting) return true;
            string contentText = content.text;
            Utility.Processing.ProcessLabel(ref contentText);
            content.text = contentText;
            return true;
        }
        
        // Redirects to (Rect position, GUIContent content, GUIStyle style)
        // [HarmonyPatch(typeof(UnityEngine.GUI), nameof(UnityEngine.GUI.Label), new Type[] { typeof(Rect), typeof(string), typeof(GUIStyle) })]
        // public static bool Prefix(Rect position, string text, GUIStyle style)
        // {
        //     Utility.Processing.ProcessLabel(ref text);
        //     return true;
        // }
        
        [HarmonyPatch(typeof(UnityEngine.GUI), nameof(UnityEngine.GUI.Label), new Type[] { typeof(Rect), typeof(GUIContent), typeof(GUIStyle) })]
        public static bool Prefix(Rect position, GUIContent content, GUIStyle style)
        {
            if (Utility.Patching.SkipReadableNumberFormatting) return true;
            string contentText = content.text;
            Utility.Processing.ProcessLabel(ref contentText);
            content.text = contentText;
            return true;
        }
    }
}