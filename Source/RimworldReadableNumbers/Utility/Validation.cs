using System;

namespace RimworldReadableNumbers.Utility
{
    public readonly struct ValidationResult
    {
        public bool IsValid { get; }
        public bool HasDecimalPlace { get; }
        public short DecimalPlaceIndex { get; }

        public ValidationResult(bool isValid, bool hasDecimalPlace, short decimalPlaceIndex)
        {
            this.IsValid = isValid;
            this.HasDecimalPlace = hasDecimalPlace;
            this.DecimalPlaceIndex = decimalPlaceIndex;
        }

        public ValidationResult(bool isValid)
        {
            this.IsValid = isValid;
            this.HasDecimalPlace = false;
            this.DecimalPlaceIndex = 0;
        }
    }

    public static class Validation
    {
        public static bool HasEnoughDigitsAndNotBlackListed(ref ReadOnlySpan<char> label)
        {
            var blacklist = RnSetting.Blacklist;
            bool isEnoughDigits = false;

            short numDigitsInSequence = 0;
            bool hasDecimalPlace = false;
            char lastChar='A';
            char secondLastChar ='A';
            for (short i = 0; i < label.Length; i++)
            {
                char currentChar = label[i];

                // Check if the label is on the blacklist
                if (blacklist.Length > 0)
                {
                    for (short j = 0; j < blacklist.Length; j++)
                    {
                        var blacklistItem = blacklist[j];
                        if (i == 0) blacklistItem.SearchIndex = 0;

                        // skip if pattern is too big to match anymore
                        if (blacklistItem.Pattern.Length > label.Length
                            || blacklistItem.Pattern.Length - blacklistItem.SearchIndex > label.Length - i
                            || blacklistItem.SearchIndex > blacklistItem.Pattern.Length - 1) continue;


                        char currentPatternChar = blacklistItem.Pattern[blacklistItem.SearchIndex];
                        if (currentPatternChar == currentChar)
                        {
                            if (blacklistItem.Pattern.Length - 1 == blacklistItem.SearchIndex)
                            {
                                // if a whole pattern has been matched then return failure
                                return false;
                            }

                            blacklistItem.SearchIndex += 1;
                        }
                        else
                        {
                            blacklistItem.SearchIndex = 0;
                        }
                    }
                }

                if (char.IsNumber(currentChar))
                {
                    numDigitsInSequence++;
                    if (numDigitsInSequence >= 4) isEnoughDigits = true;
                    if (lastChar == '.' && char.IsNumber(secondLastChar))
                    {
                        hasDecimalPlace = true;
                    }
                }
                else
                {
                    numDigitsInSequence = 0;
                }
                secondLastChar = lastChar;
                lastChar = currentChar;
            }

            // Return true only if there is a number >1000
            // or
            // RnSetting.FormatAllNumbers is true
            //      and RnSetting.DecimalSeparator is not a period
            //      and there is a number with a decimal value e.g. "1.2" 
            return isEnoughDigits || (RnSetting.FormatAllNumbers && RnSetting.DecimalSeparator != '.' && hasDecimalPlace);
        }

        public static ValidationResult IsValidNumberToFormat(ref ReadOnlySpan<char> value)
        {
            if (value == null) return new ValidationResult(false);
            if (value.Length > short.MaxValue) return new ValidationResult(false);
            short index = 0;
            char decimalSeparator = '.'; // Hardcoded since Rimworld is Culture Insensitive and will always use '.'
            var digitsWithoutPeriod = 0;
            short decimalPlaceIndex = 0;
            var hasDecimalPlace = false;
            for (short i = 0; i < value.Length; i++)
            {
                char c = value[i];
                if (!char.IsNumber(c) && c != decimalSeparator) return new ValidationResult(false);

                if (c == decimalSeparator)
                {
                    if (hasDecimalPlace)
                        return
                            new ValidationResult(
                                false); // more than one period in string, therefore not a number to format
                    hasDecimalPlace = true;
                    decimalPlaceIndex = index;
                }

                else
                {
                    if (hasDecimalPlace == false && c != decimalSeparator) digitsWithoutPeriod++;
                }

                if (hasDecimalPlace && digitsWithoutPeriod <= ((RnSetting.FormatAllNumbers && RnSetting.DecimalSeparator != '.' && decimalPlaceIndex < value.Length - 1) ? 0 : 3))
                {
                    return new ValidationResult(false);
                }
                index++;
            }

            // Skip if value is > 1000
            // Except if FormatAllNumbers == true and there is a valid decimal e.g. "0.0"
            if (value.Length - (hasDecimalPlace ? value.Length - decimalPlaceIndex - 1 : 0) <=
                ((RnSetting.FormatAllNumbers && RnSetting.DecimalSeparator != '.' && hasDecimalPlace && decimalPlaceIndex < value.Length - 1) ? 0 : 3))
            {
                return new ValidationResult(false); // Too few digits before period to require processing
            }
            else
            {
                return new ValidationResult(true, hasDecimalPlace, decimalPlaceIndex);
            }
        }
    }
}