using System;
using System.Runtime.CompilerServices;
using System.Text;
using HarmonyLib;
using UnityEngine;
using Verse;

namespace RimworldReadableNumbers.Patches.Widgets
{
    
    [HarmonyPatch]
    public class WidgetsPatch
    {
        // public static void Label(float x, ref float curY, float width, string text, TipSignal tip = default(TipSignal))
        [HarmonyPatch(typeof(Verse.Widgets), "Label", new Type[] { typeof(Rect), typeof(string) })]
        public static bool  Prefix(Rect rect, ref string label)
        {
            
            Utility.Processing.ProcessStringReference(ref label);
            return true;
        }
        
        
        // [HarmonyPatch(typeof(Verse.WidgetsPatch), "Label", new Type[] { typeof(Rect), typeof(GUIContent) })]
        // public static bool  Prefix(Rect rect, ref GUIContent content)
        // {
        //     string temp = content.text;
        //     Utility.Processing.ProcessStringReference(ref temp);
        //     content.text = temp;
        //     return true;
        // }
        

    }
}