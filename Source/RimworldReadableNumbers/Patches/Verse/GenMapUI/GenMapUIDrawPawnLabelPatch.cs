using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using Verse;

namespace RimworldReadableNumbers.Patches.Verse.GenMapUI
{
    // Would be too difficult to fix pawn names in every possible location.
    // Recommended to use the Blacklist in setting instead, if a pawn's name is being formatted
    
    // [HarmonyPatch(typeof(global::Verse.GenMapUI), nameof(global::Verse.GenMapUI.DrawPawnLabel),new Type[] { typeof(Pawn), typeof(Rect), typeof(float), typeof(float), typeof(Dictionary<string, string>), typeof(GameFont), typeof(bool), typeof(bool) })]
    // public static class GenMapUIDrawPawnLabelPatch
    // {
    //     [HarmonyTranspiler]
    //     public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    //     {
    //         return Utility.Patching.TranspileReversePatchWidgetLabel(instructions);
    //     }
    //
    // }
}