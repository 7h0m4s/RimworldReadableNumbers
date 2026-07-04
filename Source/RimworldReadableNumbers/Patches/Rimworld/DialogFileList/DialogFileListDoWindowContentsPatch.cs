using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace RimworldReadableNumbers.Patches.Rimworld.DialogFileList
{
    [HarmonyPatch(typeof(RimWorld.Dialog_FileList), nameof(RimWorld.Dialog_FileList.DoWindowContents),new Type[] { typeof(Rect) })]
    public static class DialogFileListDoWindowContentsPatch
    {
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            return Utility.Patching.TranspileReversePatchWidgetLabel(instructions);
        }
    }
}