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
        private static bool _disableReadableNumberFormatting = false;
        public static bool DisableReadableNumberFormatting
        {
            get => _disableReadableNumberFormatting;
            set => _disableReadableNumberFormatting = value;
        }
        
        
        [ThreadStatic]
        private static bool _isAlreadyReadableNumberFormatted = false;
        
        public static bool IsAlreadyReadableNumberFormatted
        {
            get => _isAlreadyReadableNumberFormatted;
            set => _isAlreadyReadableNumberFormatted = value;
        }
    }
}