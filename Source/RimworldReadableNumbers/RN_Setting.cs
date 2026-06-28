using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;
using Verse;

namespace RimworldReadableNumbers
{
    internal class RN_Setting : ModSettings
    {
        public static bool Enable = true;
        public static bool Debug = false;
        public static RN_Mod.NumberFormat NumberFormat = RN_Mod.NumberFormat.CommaPeriod;
        public static BlacklistPattern[] Blacklist = Array.Empty<BlacklistPattern>();

        private static string blacklistTextboxBuffer;
        private char blacklistSeparator = '|';

        private static char _digitSeparator = ',';
        private static char _decimalSeparator = '.';
        private static Vector2 scrollPosition = Vector2.zero;

        public static char DigitSeparator
        {
            get { return _digitSeparator; }
            set { _digitSeparator = value; }
        }

        public static char DecimalSeparator
        {
            get { return _decimalSeparator; }
            set { _decimalSeparator = value; }
        }

        [Serializable]
        public struct BlacklistPattern
        {
            public string pattern;
            public bool isSetForRemoval;
        }

        public static class SettingDefaults
        {
            public static bool Enable = true;
            public static bool Debug = false;
            public static RN_Mod.NumberFormat NumberFormat = RN_Mod.NumberFormat.CommaPeriod;
            public static BlacklistPattern[] Blacklist = Array.Empty<BlacklistPattern>();
        }

        private static void ResetToDefault()
        {
            Enable = SettingDefaults.Enable;
            Debug = SettingDefaults.Debug;
            NumberFormat = SettingDefaults.NumberFormat;
            Blacklist = SettingDefaults.Blacklist;
            Enable = SettingDefaults.Enable;
            _digitSeparator = ',';
            _decimalSeparator = '.';
            
            blacklistTextboxBuffer = "";
        }

        public override void ExposeData()
        {
            Scribe_Values.Look(ref Enable, "enable", SettingDefaults.Enable, false);
            Scribe_Values.Look(ref Debug, "debugmode", SettingDefaults.Debug, false);
            Scribe_Values.Look(ref NumberFormat, "numberformat", SettingDefaults.NumberFormat, false);

            string blackListSerialized = string.Join(
                blacklistSeparator.ToString(),
                Blacklist.Select(a =>
                        a.pattern
                            .Replace(blacklistSeparator.ToString(), "\\" + blacklistSeparator))
                    .ToList());
            Scribe_Values.Look(ref blackListSerialized, "blacklist", "", false);

            switch (NumberFormat)
            {
                case RN_Mod.NumberFormat.CommaPeriod:
                    _digitSeparator = ',';
                    break;
                case RN_Mod.NumberFormat.PeriodComma:
                    _digitSeparator = '.';
                    break;
                case RN_Mod.NumberFormat.SpaceComma:
                case RN_Mod.NumberFormat.SpacePeriod:
                    _digitSeparator = ' ';
                    break;
                default:
                    _digitSeparator = ',';
                    break;
            }

            switch (NumberFormat)
            {
                case RN_Mod.NumberFormat.PeriodComma:
                case RN_Mod.NumberFormat.SpaceComma:
                    _decimalSeparator = ',';
                    break;
                case RN_Mod.NumberFormat.CommaPeriod:
                case RN_Mod.NumberFormat.SpacePeriod:
                    _decimalSeparator = '.';
                    break;
                default:
                    _decimalSeparator = '.';
                    break;
            }

            Blacklist = blackListSerialized
                .Split(blacklistSeparator)
                .Where(a => a != "")
                .Select(a => new BlacklistPattern()
                {
                    pattern = a.Replace("\\" + blacklistSeparator, blacklistSeparator.ToString()),
                    isSetForRemoval = false
                }).ToArray();

            base.ExposeData();
        }

        public static void DoSettingsWindowContents(Rect inRect)
        {
            // inRect.yMin += 10f;
            // inRect.yMax -= 10f;
            inRect.yMax = (30 * (Text.LineHeight)) + (Blacklist.Length * (Text.LineHeight) * 2);
            var patternScrollRect = new Rect(0f, 0f, inRect.width - 16f, inRect.height * 1.5f);
            Widgets.BeginScrollView(inRect, ref scrollPosition, patternScrollRect, true);

            Listing_Standard listingStandard = new Listing_Standard(patternScrollRect, () => scrollPosition);
            listingStandard.Begin(inRect);
            listingStandard.maxOneColumn = true;
            listingStandard.ColumnWidth = (inRect.width / 4) * 3;

            listingStandard.CheckboxLabeled("ReadableNumbers_Option_Enable".Translate(), ref RN_Setting.Enable);

            listingStandard.Gap(Text.LineHeight);

            bool resetButtonPressed =
                listingStandard.ButtonText("ReadableNumbers_Option_RestoreDefault".Translate(), "", 0.4f);
            if (resetButtonPressed) ResetToDefault();

            //listingStandard.CheckboxLabeled("ReadableNumbers_Option_Debug".Translate(), ef RN_Setting.debug);
            listingStandard.Gap(Text.LineHeight);
            listingStandard.GapLine();
            listingStandard.Gap(Text.LineHeight);

            var separatorSectionHeight = Text.LineHeight * (Enum.GetNames(typeof(RN_Mod.NumberFormat)).Length + 2);
            var separatorSection = listingStandard.BeginSection(separatorSectionHeight);
            separatorSection.Label("ReadableNumbers_SeparatorCharacter_Label".Translate());
            if (separatorSection.RadioButton("ReadableNumbers_SeparatorCharacter_Comma_Period".Translate(),
                    RN_Setting.NumberFormat == RN_Mod.NumberFormat.CommaPeriod, 10f))
            {
                RN_Setting.NumberFormat = RN_Mod.NumberFormat.CommaPeriod;
                RN_Setting.DigitSeparator = ',';
                RN_Setting.DecimalSeparator = '.';
            }

            if (separatorSection.RadioButton("ReadableNumbers_SeparatorCharacter_Period_Comma".Translate(),
                    RN_Setting.NumberFormat == RN_Mod.NumberFormat.PeriodComma, 10f))
            {
                RN_Setting.NumberFormat = RN_Mod.NumberFormat.PeriodComma;
                RN_Setting.DigitSeparator = '.';
                RN_Setting.DecimalSeparator = ',';
            }

            if (separatorSection.RadioButton("ReadableNumbers_SeparatorCharacter_Space_Period".Translate(),
                    RN_Setting.NumberFormat == RN_Mod.NumberFormat.SpacePeriod, 10f))
            {
                RN_Setting.NumberFormat = RN_Mod.NumberFormat.SpacePeriod;
                RN_Setting.DigitSeparator = ' ';
                RN_Setting.DecimalSeparator = '.';
            }

            if (separatorSection.RadioButton("ReadableNumbers_SeparatorCharacter_Space_Comma".Translate(),
                    RN_Setting.NumberFormat == RN_Mod.NumberFormat.SpaceComma, 10f))
            {
                RN_Setting.NumberFormat = RN_Mod.NumberFormat.SpaceComma;
                RN_Setting.DigitSeparator = ' ';
                RN_Setting.DecimalSeparator = ',';
            }

            listingStandard.EndSection(separatorSection);

            listingStandard.Gap(Text.LineHeight);
            listingStandard.GapLine();
            listingStandard.Gap(Text.LineHeight);
            
            listingStandard.Label("ReadableNumbers_Blacklist_Label".Translate());
            listingStandard.Label("ReadableNumbers_Blacklist_Desc".Translate());
            listingStandard.Gap(Text.LineHeight);
            
            Rect textFieldRect = listingStandard.GetRect(Text.LineHeight);
            
            blacklistTextboxBuffer = Widgets.TextField(textFieldRect, blacklistTextboxBuffer);
            bool addButtonPressed = listingStandard.ButtonText("ReadableNumbers_Blacklist_Add".Translate(), "", 1f);
            listingStandard.Gap(Text.LineHeight);
            if (addButtonPressed)
            {
                if (Blacklist.All(a => a.pattern != blacklistTextboxBuffer))
                {
                    Array.Resize(ref Blacklist, Blacklist.Length + 1);
                    Blacklist[Blacklist.Length - 1] = new BlacklistPattern
                        { pattern = blacklistTextboxBuffer, isSetForRemoval = false };
                }

                blacklistTextboxBuffer = string.Empty;
            }


            if (Blacklist == null)
            {
                Blacklist = Array.Empty<BlacklistPattern>();
            }
            Blacklist = Blacklist?.Where(a => a.isSetForRemoval == false).ToArray();
            for (int i = 0; i < Blacklist.Length; i++)
            {
                listingStandard.CheckboxLabeled($"{Blacklist[i].pattern}", ref Blacklist[i].isSetForRemoval);
            }


            listingStandard.End();
            Widgets.EndScrollView();
        }
    }
}