using System;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using Verse;

namespace RimworldReadableNumbers.Patches.ToggleableReadouts
{
    public class ToggleableReadoutsReadoutCachePatch
    {
        [HarmonyPatch]
        public class ReadoutCachePatch
        {
            [HarmonyPrepare]
            static bool Prepare(MethodBase original)
            {
                if (GetToggleReadoutMethod("", true) == null) return false;
                return true;
            }

            [HarmonyTargetMethod]
            public static MethodBase TargetMethod()
            {
                return GetToggleReadoutMethod("", true);
            }

            [HarmonyPostfix]
            public static void Postfix(object __instance)
            {
                var field = Traverse.Create(__instance).Field("valueLabel");
                string valueLabel = field.GetValue<string>();
                Utility.Processing.ProcessLabel(ref valueLabel);
                field.SetValue(valueLabel);
            }
        }

        [HarmonyPatch("ToggleableReadouts.ReadoutCache", "Update")]
        public class UpdatePatch
        {
            [HarmonyPrepare]
            static bool Prepare(MethodBase original)
            {
                if (GetToggleReadoutMethod("Update", false) == null) return false;
                return true;
            }

            [HarmonyPostfix]
            public static void Postfix(object __instance)
            {
                var field = Traverse.Create(__instance).Field("valueLabel");
                string valueLabel = field.GetValue<string>();
                Utility.Processing.ProcessLabel(ref valueLabel);
                field.SetValue(valueLabel);
            }
        }

        private static MethodBase GetToggleReadoutMethod(string methodName, bool isConstructor)
        {
            if (ModsConfig.ActiveModsInLoadOrder.All(m => m.PackageId != "owlchemist.toggleablereadouts"))
                return null;
            var assembly = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(a => a.FullName.StartsWith("ToggleableReadouts"));
            if (isConstructor)
            {
                var constructors = assembly?.GetType("ToggleableReadouts.ReadoutCache").GetConstructors();
                return constructors?.FirstOrDefault();
            }
            else
            {
                return assembly?.GetType("ToggleableReadouts.ReadoutCache").GetMethod(methodName);
            }
        }
    }
}