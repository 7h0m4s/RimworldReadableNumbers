using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace RimworldReadableNumbers.Patches.Rimworld
{
     [HarmonyPatch]
    public class ExcludeStaticTextLabelsPatch
    {
        [HarmonyTargetMethods]
        public static IEnumerable<MethodBase> TargetMethods()
        {
            var rimworldAssembly = typeof(global::Verse.Widgets).Assembly;
           List<MethodBase> methodsToPatch = new List<MethodBase>();
           
           // methodsToPatch.AddRange(rimworldAssembly.GetTypes().SelectMany(t => t.GetMethods(
           //     BindingFlags.Public
           //     | BindingFlags.NonPublic
           //     | BindingFlags.Instance
           //     | BindingFlags.Static
           //     | BindingFlags.DeclaredOnly
           //     ).Where(a=> a.IsAbstract == false 
           //                 && (a.Attributes & MethodAttributes.PinvokeImpl) == 0 
           //                 && a.GetMethodBody() != null
           //                 && a.DeclaringType != null 
           //                 && a.DeclaringType.IsGenericTypeDefinition == false 
           //                 && a.DeclaringType.IsInterface == false
           //                 && a.IsConstructor == false 
           //                 && a.IsFamilyOrAssembly == false 
           //                 && a.IsSpecialName == false 
           //                 && a.IsVirtual == false 
           //                 && a.IsConstructedGenericMethod == false 
           //                 && a.IsGenericMethod == false )));
          
           // methodsToPatch.AddRange(rimworldAssembly.GetType("".GetDeclaredMethods().Where(a=> a.Name == ""));
           
            methodsToPatch.AddRange(typeof(global::Verse.ModSummaryWindow).GetDeclaredMethods().Where(a=> a.Name == "DrawContents"));
            methodsToPatch.AddRange(typeof(global::Verse.StartingPawnUtility).GetDeclaredMethods().Where(a=> a.Name == "DrawSkillSummaries"));
            methodsToPatch.AddRange(typeof(global::Verse.MouseoverReadout).GetDeclaredMethods().Where(a=> a.Name == "MouseoverReadoutOnGUI"));
            methodsToPatch.AddRange(typeof(RimWorld.Bill).GetDeclaredMethods().Where(a=> a.Name == "DoInterface"));
            methodsToPatch.AddRange(typeof(RimWorld.IdeoUIUtility).GetDeclaredMethods().Where(a=> a.Name == "DoInitialIdeoSelection"));
            methodsToPatch.AddRange(typeof(RimWorld.ScenPart_PawnModifier).GetDeclaredMethods().Where(a=> a.Name == "DoPawnModifierEditInterface"));
            methodsToPatch.AddRange(typeof(RimWorld.ScenPart_PlanetLayer).GetDeclaredMethods().Where(a=> a.Name == "DoConnections"));
            methodsToPatch.AddRange(typeof(RimWorld.Dialog_AssignBuildingOwner).GetDeclaredMethods().Where(a=> a.Name == "DrawUnassignedRow"));
            methodsToPatch.AddRange(typeof(RimWorld.Dialog_CreateXenogerm).GetDeclaredMethods().Where(a=> a.Name == "DrawSection"));
            methodsToPatch.AddRange(typeof(RimWorld.Dialog_CreateXenotype).GetDeclaredMethods().Where(a=> a.Name == "DrawSection"));
            methodsToPatch.AddRange(typeof(RimWorld.Dialog_EntityCodex).GetDeclaredMethods().Where(a=> a.Name == "LeftRect"));
            methodsToPatch.AddRange(typeof(RimWorld.Dialog_ManageDrugPolicies).GetDeclaredMethods().Where(a=> a.Name == "DoPolicyConfigArea"));
            methodsToPatch.AddRange(typeof(RimWorld.Dialog_ManageDrugPolicies).GetDeclaredMethods().Where(a=> a.Name == "DoColumnLabels"));
            methodsToPatch.AddRange(typeof(RimWorld.FactionUIUtility).GetDeclaredMethods().Where(a=> a.Name == "DoWindowContents"));
            methodsToPatch.AddRange(typeof(RimWorld.HealthCardUtility).GetDeclaredMethods().Where(a=> a.Name == "DrawHediffRow"));
            methodsToPatch.AddRange(typeof(RimWorld.TransferableOneWayWidget).GetDeclaredMethods().Where(a=> a.Name == "FillMainRect"));
            methodsToPatch.AddRange(typeof(RimWorld.TransferableUIUtility).GetDeclaredMethods().Where(a=> a.Name == "DoTransferableSorters"));
            methodsToPatch.AddRange(typeof(RimWorld.MainMenuDrawer).GetDeclaredMethods().Where(a=> a.Name == "DoDevBuildWarningRect"));
            methodsToPatch.AddRange(typeof(RimWorld.Page_ConfigureStartingPawns).GetDeclaredMethods().Where(a=> a.Name == "DrawPawnList"));
            methodsToPatch.AddRange(typeof(RimWorld.Page_ModsConfig).GetDeclaredMethods().Where(a=> a.Name == "DoRequirementSection"));
            methodsToPatch.AddRange(typeof(RimWorld.StorytellerUI).GetDeclaredMethods().Where(a=> a.Name == "DrawStorytellerSelectionInterface"));
            methodsToPatch.AddRange(typeof(RimWorld.LearningReadout).GetDeclaredMethods().Where(a=> a.Name == "WindowOnGUI"));
            methodsToPatch.AddRange(typeof(RimWorld.ITab_Pawn_Feeding).GetDeclaredMethods().Where(a=> a.Name == "FillTab"));
            methodsToPatch.AddRange(typeof(RimWorld.ITab_Pawn_Visitor).GetDeclaredMethods().Where(a=> a.Name == "DoPrisonerTab"));
            methodsToPatch.AddRange(typeof(RimWorld.ITab_PenAutoCut).GetDeclaredMethods().Where(a=> a.Name == "DrawAutoCutOptions"));
            methodsToPatch.AddRange(typeof(RimWorld.MainTabWindow_Research).GetDeclaredMethods().Where(a=> a.Name == "DrawProjectInfo"));
            methodsToPatch.AddRange(typeof(RimWorld.MainTabWindow_Research).GetDeclaredMethods().Where(a=> a.Name == "DrawProjectProgress"));
            methodsToPatch.AddRange(typeof(RimWorld.MainTabWindow_Research).GetDeclaredMethods().Where(a=> a.Name == "DrawRightRect"));
            methodsToPatch.AddRange(typeof(RimWorld.MainTabWindow_Work).GetDeclaredMethods().Where(a=> a.Name == "DoManualPrioritiesCheckbox"));
            methodsToPatch.AddRange(typeof(RimWorld.SkillUI).GetDeclaredMethods().Where(a=> a.Name == "DrawSkillsOf"));
            methodsToPatch.AddRange(typeof(RimWorld.Planet.WITab_Caravan_Gear).GetDeclaredMethods().Where(a=> a.Name == "DoPawnRows"));
            methodsToPatch.AddRange(typeof(RimWorld.Planet.WITab_Caravan_Health).GetDeclaredMethods().Where(a=> a.Name == "DoColumnHeaders"));
            methodsToPatch.AddRange(typeof(RimWorld.Planet.WorldFactionsUIUtility).GetDeclaredMethods().Where(a=> a.Name == "DoWindowContents"));
           
            //methodsToPatch.Add(typeof(Bill.GetMethod(nameof(Bill.DoInterface)));
            
            return methodsToPatch;
        }
        
        [HarmonyTranspiler]
        [HarmonyPriority(Priority.Last)]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, MethodBase currentMethodBase)
        {
            MethodInfo verseTaggedStringMethod = AccessTools.GetDeclaredMethods(typeof(global::Verse.Translator)).First(a=> a.Name == nameof(global::Verse.Translator.Translate) && a.ReturnType == typeof(TaggedString));
            MethodInfo[] methodsToFind = AccessTools.GetDeclaredMethods(typeof(global::Verse.Widgets)).Where(a=> a.Name == nameof(global::Verse.Widgets.Label)).ToArray();
            MethodInfo[] methodsToCall = AccessTools.GetDeclaredMethods(typeof(RimworldReadableNumbers.Patches.Verse.Widgets.WidgetsLabelOriginals)).Where(a=> a.Name == nameof(RimworldReadableNumbers.Patches.Verse.Widgets.WidgetsLabelOriginals.LabelWidgetOriginal)).ToArray();
        
            ReadOnlyCollection<CodeInstruction> instructionList = instructions.ToList().AsReadOnly();
            int instructionCount = instructionList.Count;
            for (int i = 0; i < instructionCount; i++)
            {
                var currentInstruction = instructionList[i];
                if (
                    i >= 2
                    && instructionList[i - 2].opcode == OpCodes.Ldstr // Check for string literal e.g. "SuspendedCaps"
                    && instructionList[i - 1].opcode == OpCodes.Call
                    && instructionList[i - 1].operand as MethodInfo == verseTaggedStringMethod // Check for Translate(this string key) call
                    && currentInstruction.opcode == OpCodes.Call
                    && methodsToFind.Any(a => a == currentInstruction.operand as MethodBase) // Check for Widgets.Label call
                )
                {
                    if (RnSetting.Debug)
                    {
                        Log.Message($"[Readable Numbers] Exclude Rimworld method from number formatting: {currentMethodBase.DeclaringType?.ToString()} | {currentMethodBase.Name.ToString()}");
                    }

                    if (currentInstruction.operand as MethodBase == null) yield return currentInstruction;

                    // Find the matching WidgetsLabelOriginals.LabelWidgetOriginal() method
                    var methodToCall = methodsToCall.FirstOrDefault(a => ((MethodBase)currentInstruction.operand)
                        .GetParameters()
                        .Types()
                        .SequenceEqual(a.GetParameters()
                            .Types()));
                    if (methodToCall == null) yield return currentInstruction;

                    currentInstruction.operand = methodToCall;
                }

                yield return currentInstruction;
            }
        }
    }
}