using HarmonyLib;
using RimworldReadableNumbers.Utility;
using RimWorldWaylith.Imgui;
using UnityEngine;

namespace RimworldReadableNumbers.Patches.RimworldWaylith;


[HarmonyPatch] 
public static class ImGuiGuiBackendPatch
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(ImGuiGuiBackend), nameof(ImGuiGuiBackend.DrawText))] 
    public static bool Prefix(Rect rect, ref string text, int fontIndex, float fontSize, FontStyle fontStyle, TextAnchor anchor, bool wordWrap, UnityEngine.Color color, Rect clipRect, bool richText = true)
    {
        if (!Utility.Patching.DisableReadableNumberFormatting)
        {
            Processing.ProcessLabel(ref text);
        }
        
        return true;
    }
    
    [HarmonyPrefix]
    [HarmonyPatch(typeof(ImGuiGuiBackend), nameof(ImGuiGuiBackend.MeasureText))] 
    public static bool Prefix(int fontIndex, float fontSize, FontStyle fontStyle, ref string text, float wrapWidth, bool richText = true)
    {
        if (!Utility.Patching.DisableReadableNumberFormatting)
        {
            Processing.ProcessLabel(ref text);
        }
        
        return true;
    }
}