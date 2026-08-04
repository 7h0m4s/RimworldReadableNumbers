using System;
using Verse;


namespace RimworldReadableNumbers.Utility
{
    public static class Text
    {
        
        private static Memory<char> _resultMemory =  new Memory<char>(new char[short.MaxValue]);
        
        private static RnMod.SeparatorGrouping _separatorGrouping;
        private static bool _isPastPeriod;
        private static bool _isPastFirstGroup;
        private static char _digitSeparator;
        private static char _decimalSeparator;
        private static bool _replaceDecimalCharacter;
        private static short _countSinceLastSeparator;
        private static short _resultCharCount;
        private static int _resultWritingIndex;
        private static bool _canInsertSeparator;

        
        /// <summary>
        ///  The utility to convert solid numbers to separated numbers
        ///  1000000 -> 1,000,000
        /// </summary>
        /// <returns>ReadOnlySpan char</returns>
        public static ReadOnlySpan<char> FormatNumberWithStringManipulation(ref ReadOnlySpan<char> originalValue, ref bool isSuccess)
        {
            ValidationResult validationResult = Validation.IsValidNumberToFormat(ref originalValue);
            if (validationResult.IsValid == false)
            {
                isSuccess = false;
                return null;
            };

            Span<char> resultValue = _resultMemory.Span;
            _separatorGrouping = RnSetting.SeparatorGrouping;
            _isPastPeriod = !validationResult.HasDecimalPlace;
            _isPastFirstGroup = false;
            _digitSeparator = RnSetting.DigitSeparator;
            _decimalSeparator = RnSetting.DecimalSeparator;
            _replaceDecimalCharacter = _decimalSeparator != '.';
            _countSinceLastSeparator = 0;
            _resultCharCount = 0;
            _resultWritingIndex = resultValue.Length - 1;
            _canInsertSeparator = false;
            for (short i = (short)originalValue.Length;  i-- > 0;) // Reverse Loop
            {
                var currentChar = originalValue[i];
                if (currentChar == '.')
                {
                    _isPastPeriod = true;
                    
                    // Update the Decimal Separator only if we need to.
                    if (_replaceDecimalCharacter)
                    {
                        currentChar = _decimalSeparator;
                    }
                }
                else
                {
                    if (_isPastPeriod)
                    {
                        _countSinceLastSeparator++;
                    }
                }
                
                resultValue[_resultWritingIndex] = currentChar;
                _resultCharCount++;
                _resultWritingIndex--;

                // Add commas only if there are more numbers ahead
                if (i != 0 && _isPastPeriod) 
                {
                    // Add a digit separator if enough digits have passed for the current SeparatorGrouping setting
                    _canInsertSeparator = false;
                    switch (_separatorGrouping)
                    {
                        case RnMod.SeparatorGrouping.ThreeDigits:
                            _canInsertSeparator = _countSinceLastSeparator == 3;
                            break;
                        case RnMod.SeparatorGrouping.ThreeThenTwoDigits:
                            _canInsertSeparator = (_isPastFirstGroup == false && _countSinceLastSeparator == 3) ||
                                                 (_isPastFirstGroup == true && _countSinceLastSeparator == 2);
                            break;
                        case RnMod.SeparatorGrouping.FourDigits:
                            _canInsertSeparator = _countSinceLastSeparator == 4;
                            break;
                        case RnMod.SeparatorGrouping.None:
                            _canInsertSeparator = false;
                            break;
                    }

                    if (_canInsertSeparator)
                    {
                        // Insert Separator and reset for next digit group
                        resultValue[_resultWritingIndex] = _digitSeparator;
                        _resultCharCount++;
                        _resultWritingIndex--;
                        _countSinceLastSeparator = 0;
                        _isPastFirstGroup = true;
                    }
                }
            }
            isSuccess = true;
            return resultValue.Slice(short.MaxValue - _resultCharCount);
        }



        
    }
}