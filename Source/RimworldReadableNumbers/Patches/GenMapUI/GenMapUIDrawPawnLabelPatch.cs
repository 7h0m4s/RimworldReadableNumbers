using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;
using Verse;

namespace RimworldReadableNumbers.Patches.GenMapUI
{
    [HarmonyPatch(typeof(Verse.GenMapUI), nameof(Verse.GenMapUI.DrawPawnLabel),new Type[] { typeof(Pawn), typeof(Rect), typeof(float), typeof(float), typeof(Dictionary<string, string>), typeof(GameFont), typeof(bool), typeof(bool) })]
    public static class GenMapUIDrawPawnLabelPatch
    {
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            return Utility.Patching.TranspileReversePatchWidgetLabel(instructions);
        }

    }
    
    // TODO add DrawPawnLabel patch
    // TODO add DrawText patch !!!!!!
}