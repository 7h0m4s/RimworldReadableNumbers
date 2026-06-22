using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using RimworldReadableNumbers.Patches.String;
using RimworldReadableNumbers.Patches.Translator;
using Verse;

namespace RimworldReadableNumbers
{
    [StaticConstructorOnStartup]
    public class RN_Patcher
    {
        static RN_Patcher()
        {
            Harmony harmony = new Harmony("7h0m4s.RimworldReadableNumbers");
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            
            //StringFormatPatch.Postfix() -> string.Format()
            PatchDeclaredMethods(harmony, 
                typeof(StringFormatPatch), 
                nameof(StringFormatPatch.Postfix), 
                typeof(string), 
                "Format");
            
            //StringConcatPatch.Postfix() -> string.Concat()
            PatchDeclaredMethods(harmony, 
                typeof(StringConcatPatch), 
                nameof(StringConcatPatch.Postfix), 
                typeof(string), 
                "Concat");
            
            //TranslatorFormattedStringExtensionsPatch.Postfix() -> TranslatorFormattedStringExtensions.Translate()
            PatchDeclaredMethods(harmony, 
                typeof(TranslatorFormattedStringExtensionsPatch), 
                nameof(TranslatorFormattedStringExtensionsPatch.Postfix), 
                typeof(TranslatorFormattedStringExtensions), 
                "Translate");
            
            //all other [HarmonyPatch] Attributes
            harmony.PatchAll(executingAssembly);
        }

        // Apply a postfix patch method  to all declarations of a target method
        private static void PatchDeclaredMethods(Harmony harmony, Type patchMethodType, string patchMethodName, Type targetMethodDType, string targetMethodName)
        {
            MethodInfo postfix = AccessTools.Method(patchMethodType, patchMethodName);
            var methods = AccessTools.GetDeclaredMethods(targetMethodDType)
                .Where(m => m.Name == targetMethodName);
            foreach (var method in methods)
            {
                harmony.Patch(method,
                    postfix: new HarmonyMethod(postfix));
            }
        }
    }
}
