using HarmonyLib;

namespace RimworldReadableNumbers.Patches.Rimworld.DevGui
{
    [HarmonyPatch(typeof(LudeonTK.DevGUI), // Target type
        nameof(LudeonTK.DevGUI.Label))] //Target method, can be written as a plain string if private
    public static class DevGuiLabelPatch
    {
        [HarmonyPrefix]
        public static bool Prefix()
        {
            Utility.Patching.DisableReadableNumberFormatting = true;
            return true;
        }

        [HarmonyPostfix]
        public static void Postfix()
        {
            Utility.Patching.DisableReadableNumberFormatting = false;
        }
    }
}