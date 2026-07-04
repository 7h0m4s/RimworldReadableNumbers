using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using Verse;

namespace RimworldReadableNumbers.Patches.Rimworld.DialogFileList
{
    [HarmonyPatch(typeof(RimWorld.Dialog_FileList), nameof(RimWorld.Dialog_FileList.DrawDateAndVersion),new Type[] { typeof(SaveFileInfo), typeof(Rect) })]
    public static class DialogFileListDrawDateAndVersionPatch
    {
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            return Utility.Patching.TranspileReversePatchWidgetLabel(instructions);
        }
    }
}