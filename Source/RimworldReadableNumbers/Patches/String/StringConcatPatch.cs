using System;
using Verse;

namespace RimworldReadableNumbers.Patches.String
{
    public class StringConcatPatch
    {

        // [HarmonyPrefix]
        // [HarmonyPatch(nameof(SomeClass.MethodB))]
        public static void Postfix(ref string __result, object[] __args)
        {
            //Log.Message($"Value:{__instance} Format:{__0}");
            if (__result == null
                || __args.Length < 2
                || __args[0] == null
                || __args[1] == null
                || Current.ProgramState != ProgramState.Playing
                || Current.Game.CurrentMap == null
                || __result.Length <= 3 // skip if result string is too short to need a separator
                ) return;

            object[] modifiedArgs = Utility.Processing.ProcessArguments(ref __args);
            
            // Rerun String.Format()
            if (modifiedArgs != null)
            {
                if (modifiedArgs[0] is IFormatProvider)
                {
                    IFormatProvider stringFormatFormat = (IFormatProvider)modifiedArgs[0];
                    string stringFormatKey = (string)modifiedArgs[1];
                    object[] stringFormatArg = new object[modifiedArgs.Length - 2];
                    Array.Copy(modifiedArgs, 2, stringFormatArg, 0, stringFormatArg.Length);
                    __result = ReverseStringFormatPatch.OriginalFormat(stringFormatFormat, stringFormatKey, stringFormatArg);
                }
                else
                {
                    string stringFormatKey = (string)modifiedArgs[0];
                    object[] stringFormatArg = new object[modifiedArgs.Length - 1];
                    Array.Copy(modifiedArgs, 1, stringFormatArg, 0, stringFormatArg.Length);
                    __result = ReverseStringFormatPatch.OriginalFormat(stringFormatKey, stringFormatArg);
                }
            }


            var temp2 = string.Concat("a", "b");
            //var temp = "MoneyFormat".Translate(1000000.ToString("F0"));
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
