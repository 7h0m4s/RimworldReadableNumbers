using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace RimworldReadableNumbers
{
    [StaticConstructorOnStartup]
    public class RN_Mod : Mod
    {
        private readonly RN_Setting _rnSettings;
        public RN_Mod(ModContentPack content)
            : base(content)
        {
            _rnSettings = base.GetSettings<RN_Setting>();
        }
        
        public enum DigitSeparator
        {
            Comma,
            Period,
            Space,
            None
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);
            
            listingStandard.ColumnWidth = inRect.width / 2;
            
            listingStandard.CheckboxLabeled("ReadableNumbers_Option_Debug".Translate(), ref _rnSettings.debug);
            listingStandard.Gap(12);
            listingStandard.GapLine();

            var separatorSectionHeight = Text.LineHeight * (Enum.GetNames(typeof(DigitSeparator)).Length + 2) ;
            var separatorSection = listingStandard.BeginSection(separatorSectionHeight);
            separatorSection.Label("ReadableNumbers_SeparatorCharacter_Label".Translate());
            if (separatorSection.RadioButton("ReadableNumbers_SeparatorCharacter_Comma".Translate(), _rnSettings.digitSeparator == DigitSeparator.Comma, 10f ))
                _rnSettings.digitSeparator = DigitSeparator.Comma;
            if (separatorSection.RadioButton("ReadableNumbers_SeparatorCharacter_Space".Translate(), _rnSettings.digitSeparator == DigitSeparator.Space, 10f))
                _rnSettings.digitSeparator = DigitSeparator.Space;
            if (separatorSection.RadioButton("ReadableNumbers_SeparatorCharacter_None".Translate(), _rnSettings.digitSeparator == DigitSeparator.None, 10f))
                _rnSettings.digitSeparator = DigitSeparator.None;
            listingStandard.EndSection(separatorSection);
            
            
            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return "ReadableNumbers_Option_Mod_Name".Translate();
        }
    }
}
