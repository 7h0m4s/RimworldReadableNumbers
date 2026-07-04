using System;
using System.Collections.Generic;
using Verse;

namespace RimworldReadableNumbers.Utility
{
        public readonly struct ValidationResult
        {
            public bool IsValid { get; }
            public bool HasDecimalPlace { get; }
            public short DecimalPlaceIndex{  get; }
            public ValidationResult(bool isValid,  bool hasDecimalPlace, short decimalPlaceIndex)
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
            public static bool HasEnoughDigits_And_Not_BlackListed(ref ReadOnlySpan<char> label)
            {
                var blacklist = RnSetting.Blacklist;
                bool isEnoughDigits = false;
                
                short numDigitsInSequence = 0;
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
                                ||blacklistItem.Pattern.Length - blacklistItem.SearchIndex > label.Length - i
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
                    
                    if (char.IsDigit(currentChar))
                    {
                        numDigitsInSequence++;
                        if(numDigitsInSequence >= 4) isEnoughDigits = true;
                    }
                    else
                    {
                        numDigitsInSequence = 0;
                    }
                }

                return isEnoughDigits;
            }
            
            public static ValidationResult IsValidNumberToConvert(ref ReadOnlySpan<char> value)
            {
                if (value == null) return new ValidationResult(false);
                if (value.Length > short.MaxValue) return new ValidationResult(false);
                short index = 0;
                char decimalSeparator = '.'; //RnSetting.DecimalSeparator;
                var digitsWithoutPeriod = 0;
                short decimalPlaceIndex = 0;
                var hasDecimalPlace = false;
                for (short i = 0; i < value.Length; i++)
                {
                    char c = value[i];
                    if (!char.IsDigit(c) && c != decimalSeparator) return new ValidationResult(false);

                    if (c == decimalSeparator)
                    {
                        if (hasDecimalPlace) return new ValidationResult(false); // more than one period in string
                        hasDecimalPlace = true;
                        decimalPlaceIndex = index;
                    }

                    else
                    {
                        if (hasDecimalPlace == false && c != decimalSeparator) digitsWithoutPeriod++;
                    }

                    if (hasDecimalPlace && digitsWithoutPeriod <= 3) return new ValidationResult(false);
                    index++;
                }

                if (value.Length - (hasDecimalPlace? value.Length - decimalPlaceIndex - 1 : 0) <= 3 )
                    return new ValidationResult(false); // Too few digits (<3) before period to require processing
                return new ValidationResult(true, hasDecimalPlace, decimalPlaceIndex);
            }


          
        }
    
}