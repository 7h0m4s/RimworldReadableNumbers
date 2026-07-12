using HarmonyLib;

namespace RimworldReadableNumbers.Patches.Rimworld.DialogModSettings
{
    [HarmonyPatch(typeof(RimWorld.Dialog_ModSettings), // Target type
        nameof(RimWorld.Dialog_ModSettings.DoWindowContents))] //Target method, can be written as a plain string if private
    public static class DialogModSettingsDoWindowContentsPatch
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