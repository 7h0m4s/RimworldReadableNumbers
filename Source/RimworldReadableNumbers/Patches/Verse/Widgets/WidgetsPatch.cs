using System;
using HarmonyLib;
using UnityEngine;

namespace RimworldReadableNumbers.Patches.Verse.Widgets
{
    
    [HarmonyPatch]
    public class WidgetsPatch
    {
        // public static void Label(float x, ref float curY, float width, string text, TipSignal tip = default(TipSignal))
        [HarmonyPatch(typeof(global::Verse.Widgets), "Label", new Type[] { typeof(Rect), typeof(string) })]
        public static bool  Prefix(Rect rect, ref string label)
        {
            
            Utility.Processing.ProcessLabel(ref label);
            return true;
        }
        

        [HarmonyPatch(typeof(global::Verse.Widgets), "Label", new Type[] { typeof(Rect), typeof(GUIContent) })]
        public static bool  Prefix(Rect rect, ref GUIContent content)
        {
            string contentText = content.text;
            Utility.Processing.ProcessLabel(ref contentText);
            return true;
        }
        

    }
}