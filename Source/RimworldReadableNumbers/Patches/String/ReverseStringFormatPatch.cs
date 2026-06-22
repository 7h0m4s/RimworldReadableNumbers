using System;
using HarmonyLib;

namespace RimworldReadableNumbers.Patches.String
{
    [HarmonyPatch]
    public class ReverseStringFormatPatch
    {
        #region Reverse Patches

        [HarmonyReversePatch]
        [HarmonyPatch(typeof(string), "Format", new Type[] { typeof(string), typeof(object[]) })]
        public static string OriginalFormat(string format, params object[] args)
        {
            // Harmony replaces this body with the original IL at runtime
            throw new NotImplementedException("Harmony reverse patch stub");
        }

        [HarmonyReversePatch]
        [HarmonyPatch(typeof(string), "Format",
            new Type[] { typeof(IFormatProvider), typeof(string), typeof(object[]) })]
        public static string OriginalFormat(IFormatProvider provider, string format, params object[] args)
        {
            // Harmony replaces this body with the original IL at runtime
            throw new NotImplementedException("Harmony reverse patch stub");
        }

        #endregion Reverse Patches
    }
}