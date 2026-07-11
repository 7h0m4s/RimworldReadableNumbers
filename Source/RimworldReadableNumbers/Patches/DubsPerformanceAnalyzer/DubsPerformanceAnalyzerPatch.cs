using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using Verse;

namespace RimworldReadableNumbers.Patches.DubsPerformanceAnalyzer
{
    [HarmonyPatch]
    public static class DubsPerformanceAnalyzerPatch
    {
        private static Assembly DubsAnalyserAssembly;

        [HarmonyPrepare]
        static bool Prepare(MethodBase original)
        {
            return ModsConfig.ActiveModsInLoadOrder.Any(m => m.PackageId == "dubwise.dubsperformanceanalyzer.steam");
        }

        [HarmonyTargetMethods]
        public static IEnumerable<MethodBase> TargetMethods()
        {
            DubsAnalyserAssembly = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(a => a.FullName.StartsWith("PerformanceAnalyzer"));
            if (DubsAnalyserAssembly == null) return new List<MethodBase>();

            var methodsToPatch = new List<MethodBase>();

            FindDubsAnalyserMethod(methodsToPatch, "Analyzer.Profiling.Panel_BottomRow", "DrawTab");
            FindDubsAnalyserMethod(methodsToPatch, "Analyzer.Profiling.Panel_TopRow", "Draw");
            FindDubsAnalyserMethod(methodsToPatch, "Analyzer.Profiling.Panel_DevOptions", "Draw");
            FindDubsAnalyserMethod(methodsToPatch, "Analyzer.Profiling.Panel_DevOptions", "Drawtab");
            FindDubsAnalyserMethod(methodsToPatch, "Analyzer.Profiling.Panel_Stats", "DrawStats");
            FindDubsAnalyserMethod(methodsToPatch, "Analyzer.Profiling.Panel_Tabs", "DrawTabs");
            FindDubsAnalyserMethod(methodsToPatch, "Analyzer.Profiling.Panel_Tabs", "DrawEntry");
            FindDubsAnalyserMethod(methodsToPatch, "Analyzer.Profiling.Panel_Logs", "DrawColumnHeader");
            FindDubsAnalyserMethod(methodsToPatch, "Analyzer.Profiling.Panel_Logs", "DrawColumnContents");
            FindDubsAnalyserMethod(methodsToPatch, "Analyzer.Profiling.Window_SearchBar", "Draw");
            FindDubsAnalyserMethod(methodsToPatch, "Analyzer.Profiling.Panel_Patches", "Draw");
            FindDubsAnalyserMethod(methodsToPatch, "Analyzer.Profiling.Row", "DrawDeltaString");
            FindDubsAnalyserMethod(methodsToPatch, "Analyzer.Profiling.Panel_Save", "DrawComparison");
            FindDubsAnalyserMethod(methodsToPatch, "Analyzer.Profiling.Panel_Save", "DrawOptionsColumn");
            FindDubsAnalyserMethod(methodsToPatch, "Analyzer.Profiling.Panel_Save", "DrawFileInfo");
            FindDubsAnalyserMethod(methodsToPatch, "Analyzer.Profiling.Panel_Graph", "DrawButton");
            FindDubsAnalyserMethod(methodsToPatch, "Analyzer.Profiling.Panel_StackTraces", "DrawCurrentStatus");
            FindDubsAnalyserMethod(methodsToPatch, "Analyzer.Profiling.Panel_StackTraces", "DrawEnableButton");
            FindDubsAnalyserMethod(methodsToPatch, "Analyzer.Profiling.Panel_StackTraces", "DrawDisableButton");
            FindDubsAnalyserMethod(methodsToPatch, "Analyzer.Profiling.Panel_StackTraces", "DrawChangeTraceButton");
            FindDubsAnalyserMethod(methodsToPatch, "Analyzer.Profiling.Panel_StackTraces", "DrawStackTrace");
            FindDubsAnalyserMethod(methodsToPatch, "Analyzer.Profiling.Panel_StackTraces", "DrawStringWithin");
            FindDubsAnalyserMethod(methodsToPatch, "Analyzer.Profiling.GraphDrawer", "DrawGraph");
            FindDubsAnalyserMethod(methodsToPatch, "Analyzer.Profiling.GraphDrawer", "DrawAxis");
            FindDubsAnalyserMethod(methodsToPatch, "Analyzer.Profiling.DubGUI", "InlineTripleMessage");
            FindDubsAnalyserMethod(methodsToPatch, "Analyzer.Profiling.DubGUI", "InlineDoubleMessage");
            FindDubsAnalyserMethod(methodsToPatch, "Analyzer.Profiling.DubGUI", "InlineDoubleMessageNC");
            FindDubsAnalyserMethod(methodsToPatch, "Analyzer.Profiling.DubGUI", "SliderLabel");
            FindDubsAnalyserMethod(methodsToPatch, "Analyzer.Profiling.DubGUI", "LabeledSliderFloat");
            FindDubsAnalyserMethod(methodsToPatch, "Analyzer.Profiling.DubGUI", "Checkbox", true);

            FindDubsAnalyserMethod(methodsToPatch, "Analyzer.Profiling.DubGUI", "InputField");
            FindDubsAnalyserMethod(methodsToPatch, "Analyzer.Profiling.DubGUI", "Heading", true);
            FindDubsAnalyserMethod(methodsToPatch, "ColourPicker.colourPicker", "DoWindowContents");

            // Displays the FPS/TPS
            // Allow formatting for now.
            //FindDubsAnalyserMethod(methodsToPatch, "Analyzer.GUIElement_TPS", "Prefix");

            return methodsToPatch;
        }

        private static void FindDubsAnalyserMethod(List<MethodBase> methodList, string type, string method,
            bool hasOverloads = false)
        {
            try
            {
                if (hasOverloads)
                {
                    methodList.AddRange(DubsAnalyserAssembly.GetType(type).GetMethods(
                            BindingFlags.Public
                            | BindingFlags.NonPublic
                            | BindingFlags.Instance
                            | BindingFlags.Static
                            | BindingFlags.DeclaredOnly
                        ).Where(a => a.Name == method)
                    );
                }
                else
                {
                    methodList.Add(DubsAnalyserAssembly.GetType(type).GetMethod(method,
                            BindingFlags.Public
                            | BindingFlags.NonPublic
                            | BindingFlags.Instance
                            | BindingFlags.Static
                            | BindingFlags.DeclaredOnly
                        )
                    );
                }
            }
            catch (Exception e)
            {
                Log.Error($"Error Encountered Patching DubsPerformanceAnalyzer.{type}:{method} \n-----> {e.Message}");
                return;
            }
        }

        [HarmonyPrefix]
        public static bool Prefix()
        {
            RimworldReadableNumbers.Utility.Patching.DisableReadableNumberFormatting = true;
            return true;
        }

        [HarmonyPostfix]
        public static void Postfix()
        {
            RimworldReadableNumbers.Utility.Patching.DisableReadableNumberFormatting = false;
        }
    }
}