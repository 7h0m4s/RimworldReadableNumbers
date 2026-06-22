using System;
using HarmonyLib;
using RimworldReadableNumbers.Utility;
using Verse;
using RimworldReadableNumbers.Patches;

namespace RimworldReadableNumbers.Patches.Translator
{
    [HarmonyPatch(typeof(Verse.Translator), nameof(Verse.Translator.Translate), new Type[] {typeof(string), typeof(object[])}) ]
    public class TranslatorPatch
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="__result"></param>
        /// <param name="__0">this string key</param>
        /// <param name="__1">params object[] args</param>
        public static void Postfix(ref string __result, string __0, object[] __1)
        {
            if (__result == null
                || __0 == null
                || __1 == null
                || __1.Length == 0
                || Current.ProgramState != ProgramState.Playing
                || Current.Game.CurrentMap == null
                || __result.Length <= 3 // skip if result string is too short to need a separator
               ) return;
            
            object[] modifiedArgs = Utility.Processing.ProcessArguments(ref __1);
            if (modifiedArgs != null) return; // Return if no changes are required
            string translated = Traverse.Create(typeof(Verse.Translator)).Field("translated").GetValue<string>(); 
            string result = translated;
            try
            {
                __result  = string.Format(translated, modifiedArgs);
            }
            catch (Exception ex)
            {
                Log.ErrorOnce(string.Concat("Exception translating '" + translated + "': ", ex?.ToString()), Gen.HashCombineInt(__0.GetHashCode(), 394878901));
            }
            
        }
    }
}