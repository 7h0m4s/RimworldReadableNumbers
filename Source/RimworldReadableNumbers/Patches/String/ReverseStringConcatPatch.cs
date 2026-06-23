using System;
using HarmonyLib;

namespace RimworldReadableNumbers.Patches.String
{
    [HarmonyPatch]
    public class ReverseStringConcatPatch
    {
        #region Reverse Patches

        [HarmonyReversePatch]
        [HarmonyPatch(typeof(string), "Concat", new Type[] { typeof(object[]) })]
        public static string OriginalConcat(params object[] values)
        {
            // Harmony replaces this body with the original IL at runtime
            throw new NotImplementedException("Harmony reverse patch stub");
        }
        
        #endregion Reverse Patches
    }
}