using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
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
        
        public static IEnumerable<CodeInstruction> TranspileTranslatorFormattedStringExtensionsTranslate(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo methodToFind = AccessTools.Method(typeof(Verse.TranslatorFormattedStringExtensions), nameof(Verse.TranslatorFormattedStringExtensions.Translate), new Type[] { typeof(string), typeof(NamedArgument) });
            MethodInfo methodToCall = AccessTools.Method(typeof(RimworldReadableNumbers.Utility.Text), nameof(RimworldReadableNumbers.Utility.Text.TranslateWithFormatting), new Type[] { typeof(string), typeof(NamedArgument) });
        
            // Find every occurrence of methodToFind and replace with methodToCall
            return instructions.MethodReplacer(methodToFind, methodToCall).ToList();
        }
    }
}