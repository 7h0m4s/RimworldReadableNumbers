using UnityEngine;
using UnityStubs.Gui;
using Verse;

namespace RimworldReadableNumbers
{
    [StaticConstructorOnStartup]
    public class RnMod : Mod
    {
        private readonly RnSetting _rnSettings;
        //public RnGuiBackend RnGuiBackendInstance;
        public static RnMod Instance;

        public RnMod(ModContentPack content)
            : base(content)
        {
            Instance = this;
            _rnSettings = base.GetSettings<RnSetting>();

            // Commented out below code, as GuiBackend.Current seems to now be getting reset every frame.
            //RnGuiBackendInstance = new RnGuiBackend(GuiBackend.Current);
            //GuiBackend.Current = RnGuiBackendInstance;
        }

        /// <summary>
        /// Naming Convention: [DigitSeperator][DecimalSeperator]
        /// </summary>
        public enum SeparatorAndDecimalFormat
        {
            CommaPeriod,
            PeriodComma,
            SpacePeriod,
            SpaceComma,
            ApostrophePeriod,
            ApostropheComma,
            Custom,
        }
        
        public enum SeparatorGrouping
        {
            ThreeDigits,
            ThreeThenTwoDigits,
            FourDigits,
            None,
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            RnSetting.DoSettingsWindowContents(inRect);
            base.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return "ReadableNumbers_Option_Mod_Name".Translate();
        }
    }
}