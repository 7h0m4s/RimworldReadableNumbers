using System;
using Verse;


namespace RimworldReadableNumbers.Utility
{
    public static class Text
    {
        private static Memory<char> _resultMemory =  new Memory<char>(new char[short.MaxValue]);

        
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
            RnMod.SeparatorGrouping separatorGrouping = RnSetting.SeparatorGrouping;
            bool isPastPeriod = !validationResult.HasDecimalPlace;
            bool isPastFirstGroup = false;
            char digitSeparator = RnSetting.DigitSeparator;
            char decimalSeparator = RnSetting.DecimalSeparator;
            bool replaceDecimalCharacter = decimalSeparator != '.';
            short countSinceLastSeparator = 0;
            short resutCharCount = 0;
            bool canInsertSeparator = false;
            for (short i = (short)originalValue.Length;  i-- > 0;) // Reverse Loop
            {
                var currentChar = originalValue[i];
                if (currentChar == '.')
                {
                    isPastPeriod = true;
                    
                    // Update the Decimal Separator only if we need to.
                    if (replaceDecimalCharacter)
                    {
                        currentChar = decimalSeparator;
                    }
                }
                else
                {
                    if (isPastPeriod)
                    {
                        countSinceLastSeparator++;
                    }
                }
                
                resultValue[resultValue.Length - resutCharCount - 1] = currentChar;
                resutCharCount++;

                // Add commas only if there are more numbers ahead
                if (i != 0 && isPastPeriod) 
                {
                    // Add a digit separator if enough digits have passed for the current SeparatorGrouping setting
                    canInsertSeparator = false;
                    switch (separatorGrouping)
                    {
                        case RnMod.SeparatorGrouping.ThreeDigits:
                            canInsertSeparator = countSinceLastSeparator == 3;
                            break;
                        case RnMod.SeparatorGrouping.ThreeThenTwoDigits:
                            canInsertSeparator = (isPastFirstGroup == false && countSinceLastSeparator == 3) ||
                                                 (isPastFirstGroup == true && countSinceLastSeparator == 2);
                            break;
                        case RnMod.SeparatorGrouping.FourDigits:
                            canInsertSeparator = countSinceLastSeparator == 4;
                            break;
                        case RnMod.SeparatorGrouping.None:
                            canInsertSeparator = false;
                            break;
                    }

                    if (canInsertSeparator)
                    {
                        // Insert Separator and reset for next digit group
                        resultValue[resultValue.Length - resutCharCount - 1] = digitSeparator;
                        resutCharCount++;
                        countSinceLastSeparator = 0;
                        isPastFirstGroup = true;
                    }
                }
            }
            isSuccess = true;
            return resultValue.Slice(short.MaxValue - resutCharCount);
        }



        
    }
}