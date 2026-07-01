using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;

namespace RimworldReadableNumbers.Patches.GenMapUI
{
    [HarmonyPatch(typeof(Verse.GenMapUI), nameof(Verse.GenMapUI.DrawThingLabel),new Type[] { typeof(Vector2), typeof(string), typeof(Color) })]
    public static class GenMapUIDrawThingLabelPatch
    {
        
        [HarmonyPrefix]
        public static bool Prefix(Vector2 screenPos, ref string text, Color textColor)
        {
            Utility.Processing.ProcessLabel(ref text);
            return true;
        }
        
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo methodToFind = AccessTools.Method(typeof(Verse.Widgets), nameof(Verse.Widgets.Label), new Type[] { typeof(Rect), typeof(string) });
            MethodInfo methodToCall = AccessTools.Method(typeof(Widgets.WidgetsReversePatch), nameof(Widgets.WidgetsReversePatch.OriginalLabel), new Type[] { typeof(Rect), typeof(string) });
        
            // Find every occurrence of methodToFind and replace with methodToCall
            return instructions.MethodReplacer(methodToFind, methodToCall).ToList();
        }

    }
    
    // TODO add DrawPawnLabel patch
    // TODO add DrawText patch !!!!!!
}