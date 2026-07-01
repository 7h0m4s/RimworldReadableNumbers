using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using Verse;

namespace RimworldReadableNumbers.Utility
{
    public static class Patching
    {
        public static IEnumerable<CodeInstruction> TranspileReversePatchWidgetLabel(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo methodToFind = AccessTools.Method(typeof(Verse.Widgets), nameof(Verse.Widgets.Label), new Type[] { typeof(Rect), typeof(string) });
            MethodInfo methodToCall = AccessTools.Method(typeof(Patches.Widgets.WidgetsReversePatch), nameof(Patches.Widgets.WidgetsReversePatch.OriginalLabel), new Type[] { typeof(Rect), typeof(string) });
        
            // Find every occurrence of methodToFind and replace with methodToCall
            return instructions.MethodReplacer(methodToFind, methodToCall).ToList();
        }
    }
}