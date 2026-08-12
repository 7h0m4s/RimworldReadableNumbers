using System;
using HarmonyLib;
using UnityEngine;

namespace RimworldReadableNumbers.Patches.Unity.Gui
{
    [HarmonyPatch]
    public class GuiLabelReversePatch
    {
        [HarmonyReversePatch]
        [HarmonyPatch(typeof(UnityEngine.GUI), nameof(UnityEngine.GUI.Label),
            new Type[] { typeof(Rect), typeof(GUIContent) })]
        public static void LabelOriginal(Rect position, GUIContent content)
        {
            // Harmony replaces this body with the original IL at runtime
            throw new NotImplementedException("Harmony reverse patch stub");
        }


        [HarmonyReversePatch]
        [HarmonyPatch(typeof(UnityEngine.GUI), nameof(UnityEngine.GUI.Label),
            new Type[] { typeof(Rect), typeof(GUIContent), typeof(GUIStyle) })]
        public static void LabelOriginal(Rect position, GUIContent content, GUIStyle style)
        {
            // Harmony replaces this body with the original IL at runtime
            throw new NotImplementedException("Harmony reverse patch stub");
        }
    }
}