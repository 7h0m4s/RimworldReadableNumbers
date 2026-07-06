using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using RimworldReadableNumbers.Patches.Unity.Gui;
using UnityEngine;
using Verse;

namespace RimworldReadableNumbers.Utility
{
    public static class Patching
    {
        [ThreadStatic]
        private static bool _skipReadableNumberFormatting = false;

        public static bool SkipReadableNumberFormatting
        {
            get => _skipReadableNumberFormatting;
            set => _skipReadableNumberFormatting = value;
        }
    }
}