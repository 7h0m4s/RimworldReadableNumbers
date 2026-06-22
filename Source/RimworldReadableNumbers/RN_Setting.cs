using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace RimworldReadableNumbers
{
    internal class RN_Setting : ModSettings
    {
        public bool enable = true;
        public RN_Mod.DigitSeparator digitSeparator = RN_Mod.DigitSeparator.Comma;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref enable, "enable", true, false);
            Scribe_Values.Look(ref digitSeparator, "digitSeparator", RN_Mod.DigitSeparator.Comma, false);
            base.ExposeData();
        }
    }
}
