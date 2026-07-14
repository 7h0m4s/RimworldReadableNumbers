using System;
using HarmonyLib;
using UnityEngine;
using Verse;

namespace RimworldReadableNumbers.Patches.Verse.Widgets
{
    [HarmonyPatch]
    public class WidgetsLabelPatch
    {
        [HarmonyPatch(typeof(global::Verse.Widgets), "Label", new Type[] { typeof(Rect), typeof(string) })]
        public static bool Prefix(Rect rect, ref string label)
        {
            if (Utility.Patching.DisableReadableNumberFormatting || Utility.Patching.IsAlreadyReadableNumberFormatted) return true;
            Utility.Processing.ProcessLabel(ref label);
            Utility.Patching.IsAlreadyReadableNumberFormatted = true;
            return true;
        }
        

        [HarmonyPatch(typeof(global::Verse.Widgets), "Label", new Type[] { typeof(Rect),  typeof(GUIContent) })]
        public static bool Prefix(Rect rect, GUIContent content)
        {
            if (Utility.Patching.DisableReadableNumberFormatting || Utility.Patching.IsAlreadyReadableNumberFormatted) return true;
            string contentText = content.text;
            Utility.Processing.ProcessLabel(ref contentText);
            content.text = contentText;
            Utility.Patching.IsAlreadyReadableNumberFormatted = true;
            return true;
        }
        

        // [HarmonyPatch(typeof(global::Verse.Widgets), "Label", new Type[] { typeof(Rect),  typeof(TaggedString) })]
        // public static bool Prefix(Rect rect, TaggedString label)
        // {
        //     if (Utility.Patching.DisableReadableNumberFormatting || Utility.Patching.IsAlreadyReadableNumberFormatted) return true;
        //     string contentText = label.RawText;
        //     Utility.Processing.ProcessLabel(ref contentText);
        //     label.RawText = contentText;
        //     Utility.Patching.IsAlreadyReadableNumberFormatted = true;
        //     return true;
        // }
        

        [HarmonyPatch(typeof(global::Verse.Widgets), "Label", new Type[] { typeof(float),  typeof(float),  typeof(float),  typeof(string),  typeof(TipSignal) }, new ArgumentType[]
            {
                ArgumentType.Normal,
                ArgumentType.Ref,
                ArgumentType.Normal,
                ArgumentType.Normal,
                ArgumentType.Normal,
            })]
        public static bool Prefix(float x, ref float curY, float width, string text, TipSignal tip = default(TipSignal))
        {
            if (Utility.Patching.DisableReadableNumberFormatting || Utility.Patching.IsAlreadyReadableNumberFormatted) return true;
            Utility.Processing.ProcessLabel(ref text);
            Utility.Patching.IsAlreadyReadableNumberFormatted = true;
            return true;
        }
        
        //
        // [HarmonyPatch(typeof(global::Verse.Widgets), "Label", new Type[] { typeof(Rect),  typeof(float),  typeof(string),  typeof(TipSignal) })]
        // public static bool Prefix(Rect rect, ref float y, string text, TipSignal tip = default(TipSignal))
        // {
        //     // Harmony replaces this body with the original IL at runtime
        //     throw new NotImplementedException("Harmony reverse patch stub");
        // }
    }
}