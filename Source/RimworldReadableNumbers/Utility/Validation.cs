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
            public static bool HasEnoughDigitsForFormatting_Short(ref ReadOnlySpan<char> label)
            {
                short numDigitsInSequence = 0;
                for (short i = 0; i < label.Length; i++)
                {
                    if (char.IsDigit(label[i]))
                    {
                        numDigitsInSequence++;
                        if(numDigitsInSequence >= 4) return true;
                    }
                    else
                    {
                        numDigitsInSequence = 0;
                    }
                }

                return false;
            }
            public static bool HasEnoughDigitsForFormatting_Short_Unrolled(ref ReadOnlySpan<char> label)
            {
                short numDigitsInSequence = 0;
                short i = 0;
                for ( ;i < label.Length - 4; i+=4)
                {
                    if (char.IsDigit(label[i + 0]))
                    {
                        numDigitsInSequence++;
                        if(numDigitsInSequence >= 4) return true;
                    }
                    else
                    {
                        numDigitsInSequence = 0;
                    }
                    
                    if (char.IsDigit(label[i + 1]))
                    {
                        numDigitsInSequence++;
                        if(numDigitsInSequence >= 4) return true;
                    }
                    else
                    {
                        numDigitsInSequence = 0;
                    }
                    
                    if (char.IsDigit(label[i + 2]))
                    {
                        numDigitsInSequence++;
                        if(numDigitsInSequence >= 4) return true;
                    }
                    else
                    {
                        numDigitsInSequence = 0;
                    }
                    
                    if (char.IsDigit(label[i + 3]))
                    {
                        numDigitsInSequence++;
                        if(numDigitsInSequence >= 4) return true;
                    }
                    else
                    {
                        numDigitsInSequence = 0;
                    }
                }
                
                // Continues with remaining 
                for (;i < label.Length; i++)
                {
                    if (char.IsDigit(label[i]))
                    {
                        numDigitsInSequence++;
                        if(numDigitsInSequence >= 4) return true;
                    }
                    else
                    {
                        numDigitsInSequence = 0;
                    }
                }

                return false;
            }
            
            public static ValidationResult IsValidNumberToConvert(ref Span<char> value)
            {
                if (value == null) return new ValidationResult(false);
                if (value.Length > short.MaxValue) return new ValidationResult(false);
                short index = 0;
                char decimalSeparator = RN_Setting.DecimalSeparator;
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