using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using RimworldReadableNumbers.Utility;
using UnityEngine;
using Verse;

namespace RimworldReadableNumbers.Patches.RimHud
{
    // Source:
    //  public static string ToDecimalString(this int self, int remainder) => !Theme.ShowDecimals.Value ? self.ToString().Bold() : $"{self.ToString().Bold()}.{(remainder >= 100 ? "99" : remainder.ToString("D2")).Size(Theme.SmallTextStyle.ActualSize)}";
    
    
    [HarmonyPatch]
    [HarmonyPatch("RimHUD.Extensions.TextExtensions", "ToDecimalString")]
    public class RimHudTextExtensionsToDecimalStringPatch
    {

        [HarmonyPrepare]
        static bool Prepare(MethodBase original)
        {
            return ModsConfig.ActiveModsInLoadOrder.Any(m => m.PackageId == "jaxe.rimhud");
        }

        [HarmonyPostfix]
        public static void Postfix(ref string __result)
        {
            __result = __result.Replace('.', RnSetting.DecimalSeparator);
        }
    }
}