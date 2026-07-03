using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using Verse;

namespace RimworldReadableNumbers.Patches.Verse.GenMapUI
{
    [HarmonyPatch(typeof(global::Verse.GenMapUI), nameof(global::Verse.GenMapUI.DrawPawnLabel),new Type[] { typeof(Pawn), typeof(Rect), typeof(float), typeof(float), typeof(Dictionary<string, string>), typeof(GameFont), typeof(bool), typeof(bool) })]
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