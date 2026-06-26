using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimworldReadableNumbers.Utility
{
    public static class Text
    {
        // TODO create alternate FormatNumberWithTryParse that loops through string in reverse and builds a new string with separators. Then compare performance.
        
         /// <summary>
        ///  The utility to convert solid numbers to seperated numbers
        ///  1000000 -> 1,000,000
        /// </summary>
        /// <returns>ReadOnlySpan char</returns>
        public static ReadOnlySpan<char> FormatNumberWithStringManipulation(ref ReadOnlySpan<char> originalValue, ref bool isSuccess)
        {
            ValidationResult validationResult = Validation.IsValidNumberToConvert(ref originalValue);
            if (validationResult.IsValid == false)
            {
                isSuccess = false;
                return null;
            };
            short resultValueLength = (short)(originalValue.Length + (originalValue.Length / 3));
            Span<char> resultValue = new char[resultValueLength];
            bool isPastPeriod = !validationResult.HasPeriod;
            short countSinceLastSeparator = 0;
            short resutCharCount = 0;
            for (short i = (short)originalValue.Length;  i-- > 0;) // Reverse Loop
            {
                var currentChar = originalValue[i];
                if (currentChar == '.') isPastPeriod = true;

                if (isPastPeriod)
                {
                    countSinceLastSeparator++;
                }
                
                resultValue[resultValue.Length - resutCharCount - 1] = currentChar;
                resutCharCount++;

                if (countSinceLastSeparator == 3 && i != 0) // Add commas only if there are more numbers ahead
                {
                    resultValue[resultValue.Length - resutCharCount - 1] = ',';
                    resutCharCount++;
                    countSinceLastSeparator = 0;
                }
            }
            isSuccess = true;
            return resultValue.Slice(resultValueLength - resutCharCount);
        }
        
        /// <summary>
        ///  The utility to convert solid numbers to seperated numbers
        ///  1000000 -> 1,000,000
        /// </summary>
        /// <returns>Boolean</returns>
        public static (bool isSuccess, string formattedNumber) FormatNumberWithTryParse(ref string strValue)
        {
            ValidationResult validationResult = Validation.IsValidNumberToConvert(ref strValue);
            if (validationResult.IsValid == false) return (false, null);
            var stringArgLength = strValue.Length;
            short periodIndex = validationResult.PeriodIndex;
            if (validationResult.HasPeriod)
            {
                // Choose best TryParse based on rough number of digits in string
                // we are not checking for - character for simplicity
                if (stringArgLength - 1 > 15)
                {
                    if (Decimal.TryParse(strValue, out Decimal outVal))
                    {
                        strValue = outVal.ToString($"N{stringArgLength - periodIndex - 1}");
                        return (true, strValue);
                    }
                } 
                else if (stringArgLength - 1 > 7)
                {
                    if (Double.TryParse(strValue, out Double outVal))
                    {
                        strValue = outVal.ToString($"N{stringArgLength - periodIndex - 1}");
                        return (true, strValue);
                    }
                }
                else
                {
                    if (float.TryParse(strValue, out float floatOut))
                    {
                        strValue = floatOut.ToString($"N{stringArgLength - periodIndex - 1}");
                        return (true, strValue);
                    }
                }
            }
            else
            {
                if (stringArgLength > 9)
                {
                    if (long.TryParse(strValue, out long outVal))
                    {
                        strValue = outVal.ToString("N0");
                        return (true, strValue);
                    }
                }
                else if (stringArgLength > 4)
                {
                    if (int.TryParse(strValue, out int outVal))
                    {
                        strValue =  outVal.ToString("N0");
                        return (true, strValue);
                    }
                }
                else 
                {
                    if (short.TryParse(strValue, out short outVal))
                    {
                        strValue = outVal.ToString("N0");
                        return (true, strValue);
                    }
                }
            }
            return (false, null);
        }
    }
}