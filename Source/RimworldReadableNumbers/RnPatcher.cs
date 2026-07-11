using HarmonyLib;
using System.Reflection;
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