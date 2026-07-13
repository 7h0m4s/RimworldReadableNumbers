using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
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
            MethodInfo methodToCall = AccessTools.Method(typeof(RimworldReadableNumbers.Utility.Patching), nameof(RimworldReadableNumbers.Utility.Patching.TranslateWithFormatting), new Type[] { typeof(string), typeof(NamedArgument) });
        
            // Find every occurrence of methodToFind and replace with methodToCall
            return instructions.MethodReplacer(methodToFind, methodToCall).ToList();
        }
        
        // Use to override .Translate() to output correctly resolved and formatted text
        // as pre-emptively formatting text before translation can lead to weird <color> tag behavior
        public static TaggedString TranslateWithFormatting(this string key, NamedArgument arg1)
        {
            TaggedString translateResult = Verse.TranslatorFormattedStringExtensions.Translate(key, arg1);
            string resolvedTaggedString = translateResult.Resolve();
            Processing.ProcessLabel(ref resolvedTaggedString);
            TaggedString newFormattedTaggedString = new TaggedString(resolvedTaggedString);
            return newFormattedTaggedString;
        }


        public static void SkipFormattingWidgetsLabel(Rect rect, string label)
        {
            Utility.Patching.DisableReadableNumberFormatting = true;
            Verse.Widgets.Label(rect, label);
            Utility.Patching.DisableReadableNumberFormatting = false;
            
        }
        
        public static IEnumerable<CodeInstruction> FirstMethodReplacer(this IEnumerable<CodeInstruction> instructions, MethodBase from, MethodBase to)
        {
            bool hasReplacedFirst = false;
            if (from is null)
                throw new ArgumentException("Unexpected null argument", nameof(from));
            if (to is null)
                throw new ArgumentException("Unexpected null argument", nameof(to));

            foreach (var instruction in instructions)
            {
                var method = instruction.operand as MethodBase;
                if (method == from && hasReplacedFirst == false)
                {
                    instruction.opcode = to.IsConstructor ? OpCodes.Newobj : OpCodes.Call;
                    instruction.operand = to;
                    hasReplacedFirst = true;
                }
                yield return instruction;
            }
        }
    }
}