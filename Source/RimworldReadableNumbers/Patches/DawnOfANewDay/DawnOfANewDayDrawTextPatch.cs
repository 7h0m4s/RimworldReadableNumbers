using System;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using Verse;

namespace RimworldReadableNumbers.Patches.DawnOfANewDay
{
        [HarmonyPatch]
        public class DawnOfANewDayDrawTextPatch
        {
            [HarmonyPrepare]
            static bool Prepare(MethodBase original)
            {
                if (GetDawnOfANewDayDrawTextMethod() == null) return false;
                return true;
            }
            
            [HarmonyTargetMethod]
            public static MethodBase TargetMethod()
            {
                return GetDawnOfANewDayDrawTextMethod();
            }
            
            [HarmonyPrefix]
            public static bool Prefix()
            {
                Utility.Patching.DisableReadableNumberFormatting = true;
                return true;
            }

            [HarmonyPostfix]
            public static void Postfix()
            {
                Utility.Patching.DisableReadableNumberFormatting = false;
            }

            private static MethodBase GetDawnOfANewDayDrawTextMethod()
            {
                if (ModsConfig.ActiveModsInLoadOrder.All(m => m.PackageId != "alendio.dawnofanewday"))
                    return null;
                var assembly = AppDomain.CurrentDomain.GetAssemblies()
                    .FirstOrDefault(a => a.FullName.StartsWith("DawnOfANewDay"));

                var method = assembly?.GetType("DawnNewDay.DawnComponent").GetDeclaredMethods().FirstOrDefault(a=> a.Name == "DrawText");
                return method;
                
            }
        }
}