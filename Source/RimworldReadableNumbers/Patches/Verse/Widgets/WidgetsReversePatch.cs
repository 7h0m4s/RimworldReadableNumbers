using System;
using HarmonyLib;
using UnityEngine;

namespace RimworldReadableNumbers.Patches.Verse.Widgets
{
    [HarmonyPatch]
    public class WidgetsReversePatch
    {
        [HarmonyReversePatch]
        [HarmonyPatch(typeof(global::Verse.Widgets), "Label", new Type[] { typeof(Rect),  typeof(GUIContent) })]
        public static void OriginalLabel(Rect rect, GUIContent content)
        {
            // Harmony replaces this body with the original IL at runtime
            throw new NotImplementedException("Harmony reverse patch stub");
        }
        
        [HarmonyReversePatch]
        [HarmonyPatch(typeof(global::Verse.Widgets), "Label", new Type[] { typeof(Rect),  typeof(string) })]
        public static void OriginalLabel(Rect rect, string label)
        {
            // Harmony replaces this body with the original IL at runtime
            throw new NotImplementedException("Harmony reverse patch stub");
        }
        
        // [HarmonyReversePatch]
        // [HarmonyPatch(typeof(Verse.Widgets), "Label", new Type[] { typeof(Rect),  typeof(TaggedString) })]
        // public static void OriginalLabel(Rect rect, TaggedString label)
        // {
        //     // Harmony replaces this body with the original IL at runtime
        //     throw new NotImplementedException("Harmony reverse patch stub");
        // }
        
        // [HarmonyReversePatch]
        // [HarmonyPatch(typeof(Verse.Widgets), "Label", new Type[] { typeof(float),  typeof(float),  typeof(string),  typeof(TipSignal) })]
        // public static void OriginalLabel(float x, ref float curY, float width, string text, TipSignal tip = default(TipSignal))
        // {
        //     // Harmony replaces this body with the original IL at runtime
        //     throw new NotImplementedException("Harmony reverse patch stub");
        // }
        
        // [HarmonyReversePatch]
        // [HarmonyPatch(typeof(Verse.Widgets), "Label", new Type[] { typeof(Rect),  typeof(float),  typeof(string),  typeof(TipSignal) })]
        // public static void OriginalLabel(Rect rect, ref float y, string text, TipSignal tip = default(TipSignal))
        // {
        //     // Harmony replaces this body with the original IL at runtime
        //     throw new NotImplementedException("Harmony reverse patch stub");
        // }
    }
}