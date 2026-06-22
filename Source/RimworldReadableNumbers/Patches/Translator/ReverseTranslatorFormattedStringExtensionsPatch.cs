using System;
using HarmonyLib;
using Verse;

namespace RimworldReadableNumbers.Patches.Translator
{
    [HarmonyPatch]
    public class ReverseTranslatorFormattedStringExtensionsPatch
    {
        #region Reverse Patches

        [HarmonyReversePatch]
        [HarmonyPatch(typeof(TranslatorFormattedStringExtensions), "Translate", new Type[] { typeof(string), typeof(NamedArgument[]) })]
        public static TaggedString OriginalTranslate(string key, params NamedArgument[] args)
        {
            // Harmony replaces this body with the original IL at runtime
            throw new NotImplementedException("Harmony reverse patch stub");
        }

        #endregion Reverse Patches
    }
}