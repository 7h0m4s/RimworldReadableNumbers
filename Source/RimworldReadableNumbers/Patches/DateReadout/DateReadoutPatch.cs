using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;
using Verse;

namespace RimworldReadableNumbers.Patches.DateReadout
{
    [HarmonyPatch]
    public static class DateReadoutPatch
    {
        [HarmonyPatch(typeof(RimWorld.DateReadout), // Target type
            nameof(RimWorld.DateReadout.DateOnGUI))] //Target method, can be written as a plain string if private
        public static class MyPatch
        {
            [HarmonyTranspiler]
            public static IEnumerable<CodeInstruction> Transpiler(
                IEnumerable<CodeInstruction> instructions, // This contains all of the instructions in order.
                ILGenerator ilGenerator)
            {
                MethodInfo methodToFind =
                    AccessTools.Method(typeof(Verse.Widgets), nameof(Verse.Widgets.Label), new Type[] { typeof(Rect),  typeof(string) });
                MethodInfo methodToCall = AccessTools.Method(typeof(Widgets.WidgetsReversePatch),
                    nameof(Widgets.WidgetsReversePatch.OriginalLabel), new Type[] { typeof(Rect),  typeof(string) });
                
                // Find every occurance of methodToFind and replace with methodToCall
                return instructions.MethodReplacer(methodToFind, methodToCall).ToList();
            }
        }
    }
}



