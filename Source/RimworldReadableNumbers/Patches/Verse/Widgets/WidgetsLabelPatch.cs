using System;
using HarmonyLib;
using UnityEngine;

namespace RimworldReadableNumbers.Patches.Verse.Widgets
{
    [HarmonyPatch]
    public class WidgetsPatch
    {
        [HarmonyPatch(typeof(global::Verse.Widgets), "Label", new Type[] { typeof(Rect), typeof(string) })]
        public static bool  Prefix(Rect rect, ref string label)
        {
            if (Utility.Patching.DisableReadableNumberFormatting) return true;
            if (Utility.Patching.IsAlreadyReadableNumberFormatted)
            {
                Utility.Patching.IsAlreadyReadableNumberFormatted = false;
                return true;
            }
            Utility.Processing.ProcessLabel(ref label);
            Utility.Patching.IsAlreadyReadableNumberFormatted = true;
            return true;
        }
    }
}