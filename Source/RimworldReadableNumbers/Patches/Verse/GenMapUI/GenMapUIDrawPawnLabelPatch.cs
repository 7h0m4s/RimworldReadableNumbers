using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using Verse;

namespace RimworldReadableNumbers.Patches.Verse.GenMapUI
{
    [HarmonyPatch(typeof(global::Verse.GenMapUI), nameof(global::Verse.GenMapUI.DrawPawnLabel),new Type[] { typeof(Pawn), typeof(Rect), typeof(float), typeof(float), typeof(Dictionary<string, string>), typeof(GameFont), typeof(bool), typeof(bool) })]
    public static class GenMapUIDrawPawnLabelPatch
    {

        [HarmonyPrefix]
        public static bool Prefix(Pawn pawn,
            Rect bgRect,
            float alpha = 1f,
            float truncateToWidth = 9999f,
            Dictionary<string, string> truncatedLabelsCache = null,
            GameFont font = GameFont.Tiny,
            bool alwaysDrawBg = true,
            bool alignCenter = true)
        {
            
            return true;
        }

        [HarmonyPostfix]
        public static void Postfix()
        {
            
        }
    
    }
}