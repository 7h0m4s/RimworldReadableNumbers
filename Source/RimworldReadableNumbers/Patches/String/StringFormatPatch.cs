using System;
using RimworldReadableNumbers.Utility;
using Verse;

namespace RimworldReadableNumbers.Patches.String
{
    public class StringFormatPatch
    {
        public static void Postfix(ref string __result, object[] __args)
        {
            if (__result == null
                || __args.Length < 2
                || __args[0] == null
                || __args[1] == null
                || Current.ProgramState != ProgramState.Playing
                || Current.Game.CurrentMap == null
                || __result.Length <= 3 // skip if result string is too short to need a separator
                ) return;

            if (Validation.IsAllowedFormatPattern(__args) == false) return;
            if (Validation.IsAllowedResult(__result) == false) return;
            
            var processingResults = Utility.Processing.ProcessPatchArguments(ref __args);
            if (processingResults.isSuccess)
            {
                if (processingResults.modifiedObjects[0] is IFormatProvider)
                {
                    IFormatProvider stringFormatFormat = (IFormatProvider)processingResults.modifiedObjects[0];
                    string stringFormatKey = (string)processingResults.modifiedObjects[1];
                    object[] stringFormatArg = new object[processingResults.modifiedObjects.Length - 2];
                    // Make a copy of the __args without the first 2 elements
                    Array.Copy(processingResults.modifiedObjects, 2, stringFormatArg, 0, stringFormatArg.Length);
                    
                    // Rerun ReversePatched String.Format()
                    __result = ReverseStringFormatPatch.OriginalFormat(stringFormatFormat, stringFormatKey, stringFormatArg);
                }
                else
                {
                    string stringFormatKey = (string)processingResults.modifiedObjects[0];
                    object[] stringFormatArg = new object[processingResults.modifiedObjects.Length - 1];
                    // Make a copy of the __args without the first  element
                    Array.Copy(processingResults.modifiedObjects, 1, stringFormatArg, 0, stringFormatArg.Length);
                    
                    // Rerun ReversePatched String.Format()
                    __result = ReverseStringFormatPatch.OriginalFormat(stringFormatKey, stringFormatArg);
                }
            }
            return;
        }
    }
    
    //
    // [HarmonyPatch(typeof(GenText), nameof(GenText.ToStringMoney))]
    // public class Patch
    // {
    //
    //     //public static void Postfix(ToStringMoney __instance)
    //     //{
    //     //    // Access the cooldownCompleteTick field of the gravEngine instance
    //     //    float floatValue = Traverse.Create(__instance).Field("f").GetValue<float>();
    //     //    string returnString;
    //
    //     //    if (Find.TickManager.TicksGame == cooldownTick)
    //     //    {
    //     //        bool enable = ((Mod)LoadedModManager.GetMod<RN_Mod>()).GetSettings<RN_Setting>().enable;
    //     //        if (enable)
    //     //        {
    //     //            TaggedString label = "DS_Letter_Label".Translate();
    //     //            TaggedString text = "DS_Letter_Text".Translate();
    //     //            LookTargets targets = new LookTargets(__instance);
    //     //            ChoiceLetter letter = LetterMaker.MakeLetter(label, text, LetterDefOf.NeutralEvent, targets, null, null, null);
    //     //            Find.LetterStack.ReceiveLetter(letter);
    //     //        }
    //     //    }
    //     //}
    //     public static void Postfix(ref string __result, float __0 ,string __1)
    //     {
    //         Log.Message("Postfix called. Original result: " + __result);
    //         float f = __0;
    //         string format = __1;
    //
    //         // var nfi = CultureInfo.InvariantCulture.NumberFormat;
    //         // nfi.NumberGroupSeparator = ",";
    //
    //         if (format == null)
    //         {
    //             format = ((!(f >= 10f) && f != 0f) ? "N2" : "N0");
    //         }
    //         else
    //         {
    //             format = format.Replace("F", "N");
    //         }
    //         __result = "MoneyFormat".Translate(f.ToString(format));
    //         
             // "MoneyFormat".Translate(f.ToString(format)).Formatted();
    //
    //     }
    // }
}
