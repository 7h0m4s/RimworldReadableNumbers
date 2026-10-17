using HarmonyLib;
using UnityEngine;

namespace RimworldReadableNumbers.Patches.Gui;

// Patch to prevent editable text boxes from getting formatted
[HarmonyPatch(typeof(GUI),"TextFieldImpl")] 
public static class GuiTextFieldImplPatch
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