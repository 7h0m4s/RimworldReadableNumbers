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
            RN_Setting.DoSettingsWindowContents(inRect);
            base.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return "ReadableNumbers_Option_Mod_Name".Translate();
        }
    }
}
