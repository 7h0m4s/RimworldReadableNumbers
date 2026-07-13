using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using RimworldReadableNumbers.Utility;
using UnityEngine;
using Verse;

namespace RimworldReadableNumbers.Patches.Rimworld.MainTabWindowHistory
{
    [HarmonyPatch(typeof(RimWorld.MainTabWindow_History), "DoArchivableRow")]
    public static class MainTabWindowHistoryDoArchivableRowPatch
    {
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo methodToFind = AccessTools.Method(typeof(global::Verse.Widgets), "Label", new Type[] { typeof(Rect), typeof(string) });
            MethodInfo methodToCall = AccessTools.Method(typeof(RimworldReadableNumbers.Utility.Patching), nameof(RimworldReadableNumbers.Utility.Patching.SkipFormattingWidgetsLabel), new Type[] { typeof(Rect), typeof(string) });
        
            // Find every occurrence of methodToFind and replace with methodToCall
            return instructions.FirstMethodReplacer(methodToFind, methodToCall).ToList();
        }
        
    }
}