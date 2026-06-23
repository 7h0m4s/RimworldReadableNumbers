using System;
using System.Linq;
using HarmonyLib;
using RimworldReadableNumbers.Utility;
using Verse;

namespace RimworldReadableNumbers.Patches.Translator
{
    //[HarmonyPatch(typeof(Verse.TranslatorFormattedStringExtensions), nameof(Verse.TranslatorFormattedStringExtensions.Translate), new Type[] {typeof(string), typeof(NamedArgument[])}) ]
    public class TranslatorFormattedStringExtensionsPatch
    {
        public static void Postfix(ref TaggedString __result, object[] __args)
        {
            if (__result == null
                || __args.Length < 2
                || __args[0] == null
                || __args[1] == null
                || Current.ProgramState != ProgramState.Playing
                || Current.Game.CurrentMap == null
                || __result.Length <= 3 // skip if result string is too short to need a separator
               ) return;

            if (Validation.IsAllowedResult(__result.RawText) == false) return;

            var processingResults = Utility.Processing.ProcessPatchArguments(ref __args);
            if (processingResults.isSuccess)
            {
                // Rerun TranslatorFormattedStringExtensions.Translate()
                string translateKey = (string)processingResults.modifiedObjects[0];
                NamedArgument[] translateArg = new NamedArgument[processingResults.modifiedObjects.Length - 1];
                Array.Copy(processingResults.modifiedObjects, 1, translateArg, 0, translateArg.Length);
                __result = ReverseTranslatorFormattedStringExtensionsPatch.OriginalTranslate(translateKey,
                    translateArg);
                
            }
        }
    }
}