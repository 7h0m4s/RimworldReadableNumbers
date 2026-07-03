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
using RimworldReadableNumbers.Utility;
using UnityEngine;
using Verse;
using Text = Verse.Text;

namespace RimworldReadableNumbers
{
    internal class RnSetting : ModSettings
    {
        public static bool Enable = SettingDefaults.Enable;
        public static bool Debug = SettingDefaults.Debug;
        public static RnMod.NumberFormat NumberFormat = SettingDefaults.NumberFormat;
        public static BlacklistPattern[] Blacklist = SettingDefaults.Blacklist;
        
        public static bool CacheEnable = SettingDefaults.CacheEnable;
        public static int CacheMaxCapacity = SettingDefaults.CacheMaxCapacity;
        
        private static string cacheMaxCapacityTextboxBuffer;
        
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

        public class BlacklistPattern
        {
            public string Pattern;
            public int SearchIndex;
            public bool IsSetForRemoval;
        }

        private static class SettingDefaults
        {
            public static readonly bool Enable = true;
            public static readonly bool Debug = false;
            public static readonly RnMod.NumberFormat NumberFormat = RnMod.NumberFormat.CommaPeriod;
            public static readonly bool CacheEnable = true;
            public static readonly int CacheMaxCapacity = 25000;
            public static readonly BlacklistPattern[] Blacklist = Array.Empty<BlacklistPattern>();
        }

        private static void ResetToDefault()
        {
            Enable = SettingDefaults.Enable;
            Debug = SettingDefaults.Debug;
            NumberFormat = SettingDefaults.NumberFormat;
            CacheEnable = SettingDefaults.CacheEnable;
            CacheMaxCapacity = SettingDefaults.CacheMaxCapacity;
            Blacklist = SettingDefaults.Blacklist;
            Enable = SettingDefaults.Enable;
            _digitSeparator = ',';
            _decimalSeparator = '.';
            
            cacheMaxCapacityTextboxBuffer = "";
            blacklistTextboxBuffer = SettingDefaults.CacheMaxCapacity.ToString();
        }

        public override void ExposeData()
        {
            Scribe_Values.Look(ref Enable, "enable", SettingDefaults.Enable, false);
            Scribe_Values.Look(ref Debug, "debugmode", SettingDefaults.Debug, false);
            Scribe_Values.Look(ref NumberFormat, "numberformat", SettingDefaults.NumberFormat, false);
            
            Scribe_Values.Look(ref CacheEnable, "cacheenable", SettingDefaults.CacheEnable, false);
            Scribe_Values.Look(ref CacheMaxCapacity, "cachemaxcapacity", SettingDefaults.CacheMaxCapacity, false);

            string blackListSerialized = string.Join(
                blacklistSeparator.ToString(),
                Blacklist.Select(a =>
                        a.Pattern
                            .Replace(blacklistSeparator.ToString(), "\\" + blacklistSeparator))
                    .ToList());
            Scribe_Values.Look(ref blackListSerialized, "blacklist", "", false);

            switch (NumberFormat)
            {
                case RnMod.NumberFormat.CommaPeriod:
                    _digitSeparator = ',';
                    break;
                case RnMod.NumberFormat.PeriodComma:
                    _digitSeparator = '.';
                    break;
                case RnMod.NumberFormat.SpaceComma:
                case RnMod.NumberFormat.SpacePeriod:
                    _digitSeparator = ' ';
                    break;
                default:
                    _digitSeparator = ',';
                    break;
            }

            switch (NumberFormat)
            {
                case RnMod.NumberFormat.PeriodComma:
                case RnMod.NumberFormat.SpaceComma:
                    _decimalSeparator = ',';
                    break;
                case RnMod.NumberFormat.CommaPeriod:
                case RnMod.NumberFormat.SpacePeriod:
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
                    Pattern = a.Replace("\\" + blacklistSeparator, blacklistSeparator.ToString()),
                    IsSetForRemoval = false
                }).ToArray();

            Processing.ClearResultCache();

            base.ExposeData();
        }

        public static void DoSettingsWindowContents(Rect inRect)
        {
            // Data Setup
            inRect.yMax = (45 * (Text.LineHeight)) + (Blacklist.Length * (Text.LineHeight) * 2);
            var patternScrollRect = new Rect(0f, 0f, inRect.width - 16f, inRect.height * 1.5f);
            Widgets.BeginScrollView(inRect, ref scrollPosition, patternScrollRect, true);

            Listing_Standard listingStandard = new Listing_Standard(patternScrollRect, () => scrollPosition);
            listingStandard.Begin(inRect);
            listingStandard.maxOneColumn = true;
            listingStandard.ColumnWidth = (inRect.width / 4) * 3;

            #region Misc Settings
            
            // Basic Options Section: Enable, Debug, Restore Defaults
            listingStandard.CheckboxLabeled("ReadableNumbers_Option_Enable".Translate(), ref RnSetting.Enable);
            listingStandard.CheckboxLabeled("ReadableNumbers_Option_Debug".Translate(), ref RnSetting.Debug);

            listingStandard.Gap(Text.LineHeight);

            bool resetButtonPressed =
                listingStandard.ButtonText("ReadableNumbers_Option_RestoreDefault".Translate(), "", 0.4f);
            if (resetButtonPressed) ResetToDefault();
            
            #endregion Misc Settings
            
            listingStandard.Gap(Text.LineHeight);
            listingStandard.GapLine();
            listingStandard.Gap(Text.LineHeight);
            
            #region Separator Selection
            
            // Separator Selector Options:
            var separatorSectionHeight = Text.LineHeight * (Enum.GetNames(typeof(RnMod.NumberFormat)).Length + 2);
            var separatorSection = listingStandard.BeginSection(separatorSectionHeight);
            separatorSection.Label("ReadableNumbers_SeparatorCharacter_Label".Translate());
            if (separatorSection.RadioButton("ReadableNumbers_SeparatorCharacter_Comma_Period".Translate(),
                    RnSetting.NumberFormat == RnMod.NumberFormat.CommaPeriod, 10f))
            {
                RnSetting.NumberFormat = RnMod.NumberFormat.CommaPeriod;
                RnSetting.DigitSeparator = ',';
                RnSetting.DecimalSeparator = '.';
            }

            if (separatorSection.RadioButton("ReadableNumbers_SeparatorCharacter_Period_Comma".Translate(),
                    RnSetting.NumberFormat == RnMod.NumberFormat.PeriodComma, 10f))
            {
                RnSetting.NumberFormat = RnMod.NumberFormat.PeriodComma;
                RnSetting.DigitSeparator = '.';
                RnSetting.DecimalSeparator = ',';
            }

            if (separatorSection.RadioButton("ReadableNumbers_SeparatorCharacter_Space_Period".Translate(),
                    RnSetting.NumberFormat == RnMod.NumberFormat.SpacePeriod, 10f))
            {
                RnSetting.NumberFormat = RnMod.NumberFormat.SpacePeriod;
                RnSetting.DigitSeparator = ' ';
                RnSetting.DecimalSeparator = '.';
            }

            if (separatorSection.RadioButton("ReadableNumbers_SeparatorCharacter_Space_Comma".Translate(),
                    RnSetting.NumberFormat == RnMod.NumberFormat.SpaceComma, 10f))
            {
                RnSetting.NumberFormat = RnMod.NumberFormat.SpaceComma;
                RnSetting.DigitSeparator = ' ';
                RnSetting.DecimalSeparator = ',';
            }

            listingStandard.EndSection(separatorSection);
            
            #endregion Separator Selection

            listingStandard.Gap(Text.LineHeight);
            listingStandard.GapLine();
            listingStandard.Gap(Text.LineHeight);
            
            #region Cache
            
            // Cache Options:
            listingStandard.CheckboxLabeled("ReadableNumbers_Cache_Enable".Translate(), ref RnSetting.CacheEnable);
            listingStandard.Label("ReadableNumbers_Cache_Desc".Translate());
            listingStandard.Gap(Text.LineHeight);
            listingStandard.Label("ReadableNumbers_Cache_MaxCapacity_Label".Translate());
            Rect cacheMaxCapacityRect = listingStandard.GetRect(Text.LineHeight);
            Widgets.TextFieldNumeric(cacheMaxCapacityRect, ref RnSetting.CacheMaxCapacity, ref cacheMaxCapacityTextboxBuffer,100, 1000000);
            
            #endregion Cache
            
            listingStandard.Gap(Text.LineHeight);
            listingStandard.GapLine();
            listingStandard.Gap(Text.LineHeight);
            
            #region Blacklist
            
            // Blacklist Options:
            listingStandard.Label("ReadableNumbers_Blacklist_Label".Translate());
            listingStandard.Label("ReadableNumbers_Blacklist_Desc".Translate());
            listingStandard.Gap(Text.LineHeight);
            
            Rect textFieldRect = listingStandard.GetRect(Text.LineHeight);
            blacklistTextboxBuffer = Widgets.TextField(textFieldRect, blacklistTextboxBuffer);
            bool addButtonPressed = listingStandard.ButtonText("ReadableNumbers_Blacklist_Add".Translate(), "", 1f);
            listingStandard.Gap(Text.LineHeight);
            if (addButtonPressed)
            {
                if (Blacklist.All(a => a.Pattern != blacklistTextboxBuffer))
                {
                    Array.Resize(ref Blacklist, Blacklist.Length + 1);
                    Blacklist[Blacklist.Length - 1] = new BlacklistPattern
                        { Pattern = blacklistTextboxBuffer, IsSetForRemoval = false };
                    Processing.ClearResultCache();
                }

                blacklistTextboxBuffer = string.Empty;
            }


            if (Blacklist == null)
            {
                Blacklist = Array.Empty<BlacklistPattern>();
            }

            if (Blacklist.Any(a => a.IsSetForRemoval))
            {
                Blacklist = Blacklist.Where(a => a.IsSetForRemoval == false).ToArray();
                Processing.ClearResultCache();
            }

            for (int i = 0; i < Blacklist.Length; i++)
            {
                listingStandard.CheckboxLabeled($"{Blacklist[i].Pattern}", ref Blacklist[i].IsSetForRemoval);
            }
            
            #endregion Blacklist

            listingStandard.End();
            Widgets.EndScrollView();
        }
    }
}