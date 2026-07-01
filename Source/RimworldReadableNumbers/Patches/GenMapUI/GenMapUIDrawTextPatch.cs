using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;

namespace RimworldReadableNumbers.Patches.GenMapUI
{
    [HarmonyPatch(typeof(Verse.GenMapUI), nameof(Verse.GenMapUI.DrawText),new Type[] { typeof(Vector2), typeof(string), typeof(Color) })]
    public static class GenMapUIDrawTextPatch
    {
        [HarmonyPrefix]
        public static bool Prefix(Vector2 worldPos, ref string text, Color textColor)
        {
            Utility.Processing.ProcessLabel(ref text);
            return true;
        }
        
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            return Utility.Patching.TranspileReversePatchWidgetLabel(instructions);
        }

    }
    
    // TODO add DrawPawnLabel patch
    // TODO add DrawText patch !!!!!!
}