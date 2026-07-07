using System;
using HarmonyLib;
using UnityEngine;

namespace RimworldReadableNumbers.Patches.Verse.Widgets
{
    [HarmonyPatch]
    public static class WidgetsLabelCacheHeightPatch
    {
        // [HarmonyPatch(typeof(global::Verse.Widgets),
        //     nameof(global::Verse.Widgets.LabelCacheHeight),
        //     new Type[]
        //     {
        //         typeof(Rect),
        //         typeof(string),
        //         typeof(bool),
        //         typeof(bool)
        //     }, 
        //     new ArgumentType[] { 
        //         ArgumentType.Ref, 
        //         ArgumentType.Normal,
        //         ArgumentType.Normal, 
        //         ArgumentType.Normal
        //         
        //     })]
        // public static bool Prefix(ref Rect rect, ref string label, bool renderLabel = true, bool forceInvalidation = false)
        // {
        //     if (Utility.Patching.DisableReadableNumberFormatting) return true;
        //     if (Utility.Patching.IsAlreadyReadableNumberFormatted)
        //     {
        //         Utility.Patching.IsAlreadyReadableNumberFormatted = false;
        //         return true;
        //     }
        //     Utility.Processing.ProcessLabel(ref label);
        //     return true;
        // }
    }
}