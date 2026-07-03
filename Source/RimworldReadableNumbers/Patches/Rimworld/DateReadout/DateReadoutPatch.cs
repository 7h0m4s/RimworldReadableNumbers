using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;

namespace RimworldReadableNumbers.Patches.Rimworld.DateReadout
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
                IEnumerable<CodeInstruction> instructions, ILGenerator ilGenerator)
            {
                return Utility.Patching.TranspileReversePatchWidgetLabel(instructions);
            }
        }
    }
}



