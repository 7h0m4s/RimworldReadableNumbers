using System;

namespace RimworldReadableNumbers.Utility
{
    public static class Patching
    {
        [ThreadStatic]
        private static bool _disableReadableNumberFormatting = false;
        public static bool DisableReadableNumberFormatting
        {
            get => _disableReadableNumberFormatting;
            set => _disableReadableNumberFormatting = value;
        }
        
        
        [ThreadStatic]
        private static bool _isAlreadyReadableNumberFormatted = false;
        
        public static bool IsAlreadyReadableNumberFormatted
        {
            get => _isAlreadyReadableNumberFormatted;
            set => _isAlreadyReadableNumberFormatted = value;
        }
    }
}