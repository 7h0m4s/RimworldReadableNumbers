using System;


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
            bool isPastPeriod = !validationResult.HasDecimalPlace;
            char digitSeparator = RnSetting.DigitSeparator;
            char decimalSeparator = RnSetting.DecimalSeparator;
            short countSinceLastSeparator = 0;
            short resutCharCount = 0;
            for (short i = (short)originalValue.Length;  i-- > 0;) // Reverse Loop
            {
                var currentChar = originalValue[i];
                if (currentChar == '.') {isPastPeriod = true;}
                else
                {
                    if (isPastPeriod)
                    {
                        countSinceLastSeparator++;
                    }
                }
                
                resultValue[resultValue.Length - resutCharCount - 1] = currentChar;
                resutCharCount++;

                if (countSinceLastSeparator == 3 && i != 0) // Add commas only if there are more numbers ahead
                {
                    resultValue[resultValue.Length - resutCharCount - 1] = digitSeparator;
                    resutCharCount++;
                    countSinceLastSeparator = 0;
                }
            }
            isSuccess = true;
            return resultValue.Slice(short.MaxValue - resutCharCount);
        }
    }
}