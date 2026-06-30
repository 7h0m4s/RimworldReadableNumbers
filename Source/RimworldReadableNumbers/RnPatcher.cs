using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace RimworldReadableNumbers
{
    [StaticConstructorOnStartup]
    public class RnPatcher
    {
        static RnPatcher()
        {
            Harmony harmony = new Harmony("7h0m4s.RimworldReadableNumbers");
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            
            //all other [HarmonyPatch] Attributes
            harmony.PatchAll(executingAssembly);
        }
        
    }
}
