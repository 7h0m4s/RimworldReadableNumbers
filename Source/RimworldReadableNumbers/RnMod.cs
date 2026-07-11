using UnityEngine;
using Verse;

namespace RimworldReadableNumbers
{
    [StaticConstructorOnStartup]
    public class RnMod : Mod
    {
        private readonly RnSetting _rnSettings;

        public RnMod(ModContentPack content)
            : base(content)
        {
            _rnSettings = base.GetSettings<RnSetting>();
        }

        /// <summary>
        /// Naming Convention: [DigitSeperator][DecimalSeperator]
        /// </summary>
        public enum NumberFormat
        {
            CommaPeriod,
            PeriodComma,
            SpacePeriod,
            SpaceComma,
            None
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