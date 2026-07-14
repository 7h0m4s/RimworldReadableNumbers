using System;
using System.Linq;
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
        public static bool FormatAllNumbers = SettingDefaults.FormatAllNumbers;
        public static RnMod.SeparatorAndDecimalFormat SeparatorAndDecimalFormat = SettingDefaults.SeparatorAndDecimalFormat;
        public static RnMod.SeparatorGrouping SeparatorGrouping = SettingDefaults.SeparatorGrouping;
        public static BlacklistPattern[] Blacklist = SettingDefaults.Blacklist;
        
        public static char CustomSeparatorChar = SettingDefaults.CustomSeparatorChar;
        public static char CustomDecimalChar = SettingDefaults.CustomDecimalChar;
        private static string _customSeparatorString = SettingDefaults.CustomSeparatorChar.ToString();
        private static string _customDecimalString = SettingDefaults.CustomDecimalChar.ToString();

        public static bool CacheEnable = SettingDefaults.CacheEnable;
        public static int CacheMaxCapacity = SettingDefaults.CacheMaxCapacity;

        
        private static string _cacheMaxCapacityTextboxBuffer = SettingDefaults.CacheMaxCapacity.ToString();

        private static string _blacklistTextboxBuffer;
        private readonly char _blacklistSeparator = '|';

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
            public static readonly bool FormatAllNumbers = true;
            public static readonly RnMod.SeparatorAndDecimalFormat SeparatorAndDecimalFormat = RnMod.SeparatorAndDecimalFormat.CommaPeriod;
            public static readonly RnMod.SeparatorGrouping SeparatorGrouping = RnMod.SeparatorGrouping.ThreeDigits;
            public static readonly char CustomSeparatorChar = ',';
            public static readonly char CustomDecimalChar = '.';
            public static readonly bool CacheEnable = true;
            public static readonly int CacheMaxCapacity = 25000;
            public static readonly BlacklistPattern[] Blacklist = Array.Empty<BlacklistPattern>();
        }

        private static void ResetToDefault()
        {
            Enable = SettingDefaults.Enable;
            Debug = SettingDefaults.Debug;
            FormatAllNumbers = SettingDefaults.FormatAllNumbers;
            
            SeparatorAndDecimalFormat = SettingDefaults.SeparatorAndDecimalFormat;
            CustomSeparatorChar = SettingDefaults.CustomSeparatorChar;
            CustomDecimalChar = SettingDefaults.CustomDecimalChar;
            _customSeparatorString = SettingDefaults.CustomSeparatorChar.ToString();
            _customDecimalString = SettingDefaults.CustomDecimalChar.ToString();
            
            CacheEnable = SettingDefaults.CacheEnable;
            CacheMaxCapacity = SettingDefaults.CacheMaxCapacity;
            Blacklist = SettingDefaults.Blacklist;
            Enable = SettingDefaults.Enable;
            _digitSeparator = ',';
            _decimalSeparator = '.';

            _cacheMaxCapacityTextboxBuffer = SettingDefaults.CacheMaxCapacity.ToString();
            _blacklistTextboxBuffer = "";
        }

        public override void ExposeData()
        {
            Scribe_Values.Look(ref Enable, "enable", SettingDefaults.Enable, false);
            Scribe_Values.Look(ref Debug, "debugmode", SettingDefaults.Debug, false);
            Scribe_Values.Look(ref FormatAllNumbers, "formatallnumbers", SettingDefaults.FormatAllNumbers, false);
            Scribe_Values.Look(ref SeparatorAndDecimalFormat, "numberformat", SettingDefaults.SeparatorAndDecimalFormat, false);
            
            
            Scribe_Values.Look(ref _customSeparatorString, "customseparatorchar", SettingDefaults.CustomSeparatorChar.ToString(), false);
            Scribe_Values.Look(ref _customDecimalString, "customdecimalchar", SettingDefaults.CustomDecimalChar.ToString(), false);
            if (_customSeparatorString.Length == 1)
            {
                CustomSeparatorChar = _customSeparatorString.ToCharArray().First();
                RnSetting.DigitSeparator = CustomSeparatorChar;
            }
            else
            {
                CustomSeparatorChar = SettingDefaults.CustomSeparatorChar;
                RnSetting.DigitSeparator = SettingDefaults.CustomSeparatorChar;
            }
                
            if (_customDecimalString.Length == 1)
            {
                CustomDecimalChar = _customDecimalString.ToCharArray().First();
                RnSetting.DecimalSeparator = CustomDecimalChar;
            }
            else
            {
                CustomDecimalChar = SettingDefaults.CustomDecimalChar;
                RnSetting.DecimalSeparator = SettingDefaults.CustomDecimalChar;
            }

            switch (SeparatorAndDecimalFormat)
            {
                case RnMod.SeparatorAndDecimalFormat.CommaPeriod:
                    _digitSeparator = ',';
                    break;
                case RnMod.SeparatorAndDecimalFormat.PeriodComma:
                    _digitSeparator = '.';
                    break;
                case RnMod.SeparatorAndDecimalFormat.SpaceComma:
                case RnMod.SeparatorAndDecimalFormat.SpacePeriod:
                    _digitSeparator = ' ';
                    break;
                case RnMod.SeparatorAndDecimalFormat.ApostropheComma:
                case RnMod.SeparatorAndDecimalFormat.ApostrophePeriod:
                    _digitSeparator = '\'';
                    break;
                case RnMod.SeparatorAndDecimalFormat.Custom:
                    _digitSeparator =  CustomSeparatorChar;
                    break;
                default:
                    _digitSeparator = ',';
                    break;
            }

            switch (SeparatorAndDecimalFormat)
            {
                case RnMod.SeparatorAndDecimalFormat.PeriodComma:
                case RnMod.SeparatorAndDecimalFormat.SpaceComma:
                case RnMod.SeparatorAndDecimalFormat.ApostropheComma:
                    _decimalSeparator = ',';
                    break;
                case RnMod.SeparatorAndDecimalFormat.CommaPeriod:
                case RnMod.SeparatorAndDecimalFormat.SpacePeriod:
                case RnMod.SeparatorAndDecimalFormat.ApostrophePeriod:
                    _decimalSeparator = '.';
                    break;
                case RnMod.SeparatorAndDecimalFormat.Custom:
                    _decimalSeparator =  CustomDecimalChar;
                    break;
                default:
                    _decimalSeparator = '.';
                    break;
            }
            
            Scribe_Values.Look(ref SeparatorGrouping, "separatorgrouping", SettingDefaults.SeparatorGrouping, false);
            
            Scribe_Values.Look(ref CacheEnable, "cacheenable", SettingDefaults.CacheEnable, false);
            Scribe_Values.Look(ref CacheMaxCapacity, "cachemaxcapacity", SettingDefaults.CacheMaxCapacity, false);

            string blackListSerialized = string.Join(
                _blacklistSeparator.ToString(),
                Blacklist.Select(a =>
                        a.Pattern
                            .Replace(_blacklistSeparator.ToString(), "\\" + _blacklistSeparator))
                    .ToList());
            Scribe_Values.Look(ref blackListSerialized, "blacklist", "", false);

            Blacklist = blackListSerialized
                .Split(_blacklistSeparator)
                .Where(a => a != "")
                .Select(a => new BlacklistPattern()
                {
                    Pattern = a.Replace("\\" + _blacklistSeparator, _blacklistSeparator.ToString()),
                    IsSetForRemoval = false
                }).ToArray();

            Processing.ClearResultCache();

            base.ExposeData();
        }

        public static void DoSettingsWindowContents(Rect inRect)
        {
            // Data Setup
            //inRect.yMax = (45 * (Text.LineHeight)) + (Blacklist.Length * (Text.LineHeight) * 2);
            
            float contentsHeight = (35 * (Text.LineHeight)) + (Blacklist.Length * (Text.LineHeight) * 1.5f);
            var patternScrollRect = new Rect(0f, 0f, inRect.width - 12f, inRect.height * 1.0f + contentsHeight * 1.0f);
            Widgets.BeginScrollView(inRect, ref scrollPosition, patternScrollRect, true);

            Listing_Standard listingStandard = new Listing_Standard(patternScrollRect, () => scrollPosition);
            listingStandard.Begin(patternScrollRect);
            listingStandard.maxOneColumn = true;
            listingStandard.ColumnWidth = (inRect.width / 4) * 3;

            #region Misc Settings
            
            // Reset to Default
            bool resetButtonPressed =
                listingStandard.ButtonText("ReadableNumbers_Option_RestoreDefault".Translate(), "", 0.4f);
            if (resetButtonPressed) ResetToDefault();
            
            // Basic Options Section: Enable, Debug, Restore Defaults
            var generalSectionHeight = Text.LineHeight * 5;
            var generalSection = listingStandard.BeginSection(generalSectionHeight);
            Text.Font = GameFont.Medium;
            generalSection.Label("ReadableNumbers_Option_Heading".Translate());
            Text.Font = GameFont.Small;
            generalSection.CheckboxLabeled("ReadableNumbers_Option_Enable".Translate(), ref RnSetting.Enable);
            generalSection.CheckboxLabeled("ReadableNumbers_Option_Debug".Translate(), ref RnSetting.Debug);
            generalSection.CheckboxLabeled("ReadableNumbers_Option_FormatAllNumbers".Translate(), ref RnSetting.FormatAllNumbers);
            listingStandard.EndSection(generalSection);

            #endregion Misc Settings

            listingStandard.Gap(Text.LineHeight);
            listingStandard.GapLine();
            listingStandard.Gap(Text.LineHeight);

            #region Separator Selection

            // Separator Selector Options:
            var separatorSectionHeight = Text.LineHeight * (Enum.GetNames(typeof(RnMod.SeparatorAndDecimalFormat)).Length + 8);
            var separatorSection = listingStandard.BeginSection(separatorSectionHeight);
            Text.Font = GameFont.Medium;
            separatorSection.Label("ReadableNumbers_SeparatorCharacter_Heading".Translate());
            Text.Font = GameFont.Small;
            //separatorSection.Label("ReadableNumbers_SeparatorCharacter_Label".Translate());
            if (separatorSection.RadioButton("ReadableNumbers_SeparatorCharacter_Comma_Period".Translate().Resolve() 
                                             + GenerateSeparatorDecimalExample(',','.'),
                    RnSetting.SeparatorAndDecimalFormat == RnMod.SeparatorAndDecimalFormat.CommaPeriod, 10f))
            {
                RnSetting.SeparatorAndDecimalFormat = RnMod.SeparatorAndDecimalFormat.CommaPeriod;
                RnSetting.DigitSeparator = ',';
                RnSetting.DecimalSeparator = '.';
            }

            if (separatorSection.RadioButton("ReadableNumbers_SeparatorCharacter_Period_Comma".Translate().Resolve() 
                                             + GenerateSeparatorDecimalExample('.',','),
                    RnSetting.SeparatorAndDecimalFormat == RnMod.SeparatorAndDecimalFormat.PeriodComma, 10f))
            {
                RnSetting.SeparatorAndDecimalFormat = RnMod.SeparatorAndDecimalFormat.PeriodComma;
                RnSetting.DigitSeparator = '.';
                RnSetting.DecimalSeparator = ',';
            }

            if (separatorSection.RadioButton("ReadableNumbers_SeparatorCharacter_Space_Period".Translate().Resolve() 
                                             + GenerateSeparatorDecimalExample(' ','.'),
                    RnSetting.SeparatorAndDecimalFormat == RnMod.SeparatorAndDecimalFormat.SpacePeriod, 10f))
            {
                RnSetting.SeparatorAndDecimalFormat = RnMod.SeparatorAndDecimalFormat.SpacePeriod;
                RnSetting.DigitSeparator = ' ';
                RnSetting.DecimalSeparator = '.';
            }

            if (separatorSection.RadioButton("ReadableNumbers_SeparatorCharacter_Space_Comma".Translate().Resolve() 
                                             + GenerateSeparatorDecimalExample(' ',','),
                    RnSetting.SeparatorAndDecimalFormat == RnMod.SeparatorAndDecimalFormat.SpaceComma, 10f))
            {
                RnSetting.SeparatorAndDecimalFormat = RnMod.SeparatorAndDecimalFormat.SpaceComma;
                RnSetting.DigitSeparator = ' ';
                RnSetting.DecimalSeparator = ',';
            }
            
            if (separatorSection.RadioButton("ReadableNumbers_SeparatorCharacter_Apostrophe_Period".Translate().Resolve() 
                                             + GenerateSeparatorDecimalExample('\'','.'),
                    RnSetting.SeparatorAndDecimalFormat == RnMod.SeparatorAndDecimalFormat.ApostrophePeriod, 10f))
            {
                RnSetting.SeparatorAndDecimalFormat = RnMod.SeparatorAndDecimalFormat.ApostrophePeriod;
                RnSetting.DigitSeparator = '\'';
                RnSetting.DecimalSeparator = '.';
            }

            if (separatorSection.RadioButton("ReadableNumbers_SeparatorCharacter_Apostrophe_Comma".Translate().Resolve() 
                                             + GenerateSeparatorDecimalExample('\'',','),
                    RnSetting.SeparatorAndDecimalFormat == RnMod.SeparatorAndDecimalFormat.ApostropheComma, 10f))
            {
                RnSetting.SeparatorAndDecimalFormat = RnMod.SeparatorAndDecimalFormat.ApostropheComma;
                RnSetting.DigitSeparator = '\'';
                RnSetting.DecimalSeparator = ',';
            }
            
            if (separatorSection.RadioButton("ReadableNumbers_SeparatorCharacter_Custom".Translate().Resolve(),
                    RnSetting.SeparatorAndDecimalFormat == RnMod.SeparatorAndDecimalFormat.Custom, 10f))
            {
                RnSetting.SeparatorAndDecimalFormat = RnMod.SeparatorAndDecimalFormat.Custom;
            }
            
            separatorSection.Label("ReadableNumbers_SeparatorCharacter_Custom_Separator".Translate());
            _customSeparatorString = separatorSection.TextEntry( _customSeparatorString, 1);
            if (_customSeparatorString.Length > 1) _customSeparatorString = _customSeparatorString.First().ToString();
            separatorSection.Label("ReadableNumbers_SeparatorCharacter_Custom_Decimal".Translate());
            _customDecimalString = separatorSection.TextEntry( _customDecimalString, 1);
            if (_customDecimalString.Length > 1) _customDecimalString = _customDecimalString.First().ToString();
            if (RnSetting.SeparatorAndDecimalFormat == RnMod.SeparatorAndDecimalFormat.Custom)
            {
                if (_customSeparatorString.Length == 1)
                {
                    CustomSeparatorChar = _customSeparatorString.ToCharArray().First();
                    RnSetting.DigitSeparator = CustomSeparatorChar;
                }
                else
                {
                    CustomSeparatorChar = SettingDefaults.CustomSeparatorChar;
                    RnSetting.DigitSeparator = SettingDefaults.CustomSeparatorChar;
                }
                
                if (_customDecimalString.Length == 1)
                {
                    CustomDecimalChar = _customDecimalString.ToCharArray().First();
                    RnSetting.DecimalSeparator = CustomDecimalChar;
                }
                else
                {
                    CustomDecimalChar = SettingDefaults.CustomDecimalChar;
                    RnSetting.DecimalSeparator = SettingDefaults.CustomDecimalChar;
                }
            }
            
            GUI.color = Color.red;
            separatorSection.Label("ReadableNumbers_SeparatorCharacter_Custom_Warning_Label".Translate());
            GUI.color = Color.white;
            listingStandard.EndSection(separatorSection);
            
            #endregion Separator Selection

            listingStandard.Gap(Text.LineHeight);
            listingStandard.GapLine();
            listingStandard.Gap(Text.LineHeight);
            
            #region Grouping Selection

            // Grouping Selector Options:
            var groupingSectionHeight = Text.LineHeight * (Enum.GetNames(typeof(RnMod.SeparatorGrouping)).Length + 2);
            var groupingSection = listingStandard.BeginSection(groupingSectionHeight);
            Text.Font = GameFont.Medium;
            groupingSection.Label("ReadableNumbers_SeparatorGrouping_Heading".Translate());
            Text.Font = GameFont.Small;
            if (groupingSection.RadioButton("ReadableNumbers_SeparatorGrouping_ThreeDigit".Translate().Resolve() 
                                            + GenerateGroupingExample(RnMod.SeparatorGrouping.ThreeDigits),
                    RnSetting.SeparatorGrouping == RnMod.SeparatorGrouping.ThreeDigits, 10f))
            {
                RnSetting.SeparatorGrouping = RnMod.SeparatorGrouping.ThreeDigits;
            }

            if (groupingSection.RadioButton("ReadableNumbers_SeparatorGrouping_TwoAndThreeDigit".Translate().Resolve() 
                                            + GenerateGroupingExample(RnMod.SeparatorGrouping.ThreeThenTwoDigits),
                    RnSetting.SeparatorGrouping == RnMod.SeparatorGrouping.ThreeThenTwoDigits, 10f))
            {
                RnSetting.SeparatorGrouping = RnMod.SeparatorGrouping.ThreeThenTwoDigits;
            }

            if (groupingSection.RadioButton("ReadableNumbers_SeparatorGrouping_FourDigit".Translate().Resolve() 
                                            + GenerateGroupingExample(RnMod.SeparatorGrouping.FourDigits),
                    RnSetting.SeparatorGrouping == RnMod.SeparatorGrouping.FourDigits, 10f))
            {
                RnSetting.SeparatorGrouping = RnMod.SeparatorGrouping.FourDigits;
            }

            if (groupingSection.RadioButton("ReadableNumbers_SeparatorGrouping_None".Translate().Resolve() 
                                            + GenerateGroupingExample(RnMod.SeparatorGrouping.None),
                    RnSetting.SeparatorGrouping == RnMod.SeparatorGrouping.None, 10f))
            {
                RnSetting.SeparatorGrouping = RnMod.SeparatorGrouping.None;
            }

            listingStandard.EndSection(groupingSection);
            
            #endregion Grouping Selection

            listingStandard.Gap(Text.LineHeight);
            listingStandard.GapLine();
            listingStandard.Gap(Text.LineHeight);

            #region Cache

            // Cache Options:
            var cacheSectionHeight = Text.LineHeight * 10;
            var cacheSection = listingStandard.BeginSection(cacheSectionHeight);
            Text.Font = GameFont.Medium;
            cacheSection.Label("ReadableNumbers_Cache_Heading".Translate());
            Text.Font = GameFont.Small;
            cacheSection.CheckboxLabeled("ReadableNumbers_Cache_Enable".Translate(), ref RnSetting.CacheEnable);
            cacheSection.Label("ReadableNumbers_Cache_Desc".Translate());
            cacheSection.Gap(Text.LineHeight);
            cacheSection.Label("ReadableNumbers_Cache_MaxCapacity_Label".Translate());
            Rect cacheMaxCapacityRect = cacheSection.GetRect(Text.LineHeight);
            Widgets.TextFieldNumeric(cacheMaxCapacityRect, ref RnSetting.CacheMaxCapacity,
                ref _cacheMaxCapacityTextboxBuffer, 100, 1000000);
            listingStandard.EndSection(cacheSection);

            #endregion Cache

            listingStandard.Gap(Text.LineHeight);
            listingStandard.GapLine();
            listingStandard.Gap(Text.LineHeight);

            #region Blacklist

            // Blacklist Options:
            var blacklistSectionHeight = Text.LineHeight *  8 + (Blacklist.Length * (Text.LineHeight) * 1.5f);
            var blacklistSection = listingStandard.BeginSection(blacklistSectionHeight);
            Text.Font = GameFont.Medium;
            blacklistSection.Label("ReadableNumbers_BlackList_Heading".Translate());
            Text.Font = GameFont.Small;
            //blacklistSection.Label("ReadableNumbers_Blacklist_Label".Translate());
            blacklistSection.Label("ReadableNumbers_Blacklist_Desc".Translate());
            blacklistSection.Gap(Text.LineHeight);

            Rect textFieldRect = blacklistSection.GetRect(Text.LineHeight);
            _blacklistTextboxBuffer = Widgets.TextField(textFieldRect, _blacklistTextboxBuffer);
            bool addButtonPressed = blacklistSection.ButtonText("ReadableNumbers_Blacklist_Add".Translate(), "", 1f);
            blacklistSection.Gap(Text.LineHeight);
            if (addButtonPressed)
            {
                if (Blacklist.All(a => a.Pattern != _blacklistTextboxBuffer))
                {
                    Array.Resize(ref Blacklist, Blacklist.Length + 1);
                    Blacklist[Blacklist.Length - 1] = new BlacklistPattern
                        { Pattern = _blacklistTextboxBuffer, IsSetForRemoval = false };
                    Processing.ClearResultCache();
                }

                _blacklistTextboxBuffer = string.Empty;
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
                blacklistSection.CheckboxLabeled($"{Blacklist[i].Pattern}", ref Blacklist[i].IsSetForRemoval);
            }

            listingStandard.EndSection(blacklistSection);
            #endregion Blacklist

            listingStandard.End();
            Widgets.EndScrollView();
        }

        #region Helper Methods
        private static string GenerateSeparatorDecimalExample(char digitSeparatorChar, char decimalChar)
        {
            return $" \t \t(1{digitSeparatorChar}000{digitSeparatorChar}000{decimalChar}00)";
        }
        
        private static string GenerateGroupingExample(RnMod.SeparatorGrouping grouping)
        {
            switch (grouping)
            {
                case RnMod.SeparatorGrouping.ThreeDigits:
                    return $"\t \t \t(10,000,000.00)";
                case RnMod.SeparatorGrouping.ThreeThenTwoDigits:
                    return $" \t \t(1,00,00,000.00)";
                case RnMod.SeparatorGrouping.FourDigits:
                    return $"\t \t \t(1000,0000.00)";
                case RnMod.SeparatorGrouping.None:
                    return $"\t \t \t \t(10000000.00)";
            }
            return "";
        }
        
        #endregion Helper Methods
    }
}