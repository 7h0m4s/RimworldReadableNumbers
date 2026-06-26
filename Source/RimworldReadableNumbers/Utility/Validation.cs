using System;
using System.Collections.Generic;
using Verse;

namespace RimworldReadableNumbers.Utility
{
        public readonly struct ValidationResult
        {
            public bool IsValid { get; }
            public bool IsSigned { get; }
            public bool HasPeriod { get; }
            public short PeriodIndex{  get; }
            public ValidationResult(bool isValid, bool isSigned,  bool hasPeriod, short periodIndex)
            {
                this.IsValid = isValid;
                this.IsSigned = isSigned;
                this.HasPeriod = hasPeriod;
                this.PeriodIndex = periodIndex;
            }
            public ValidationResult(bool isValid)
            {
                this.IsValid = isValid;
                this.IsSigned = false;
                this.HasPeriod = false;
                this.PeriodIndex = 0;
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
            
            public static ValidationResult IsValidNumberToConvert(ref ReadOnlySpan<char> value)
            {
                if (value == null) return new ValidationResult(false);
                if (value.Length > short.MaxValue) return new ValidationResult(false);
                short index = 0;
                var digitsWithoutPeriod = 0;
                short periodIndex = 0;
                var isSigned = false;
                var hasPeriod = false;
                for (short i = 0; i < value.Length; i++)
                {
                    char c = value[i];
                    if (!char.IsDigit(c) && c != '.' && c != '-' && c != '+') return new ValidationResult(false);

                    if (c == '.')
                    {
                        if (hasPeriod) return new ValidationResult(false); // more than one period in string
                        hasPeriod = true;
                        periodIndex = index;
                    }

                    if (index == 0 && (c == '-' || c == '+'))
                    {
                        isSigned = true;
                    }
                    else
                    {
                        if (hasPeriod == false && c != '.') digitsWithoutPeriod++;
                    }

                    if (hasPeriod && digitsWithoutPeriod <= 3) return new ValidationResult(false);
                    index++;
                }

                if (value.Length - (isSigned? 1 : 0) - (hasPeriod? value.Length - periodIndex - 1 : 0) <= 3 )
                    return new ValidationResult(false); // Too few digits (<3) before period to require processing
                return new ValidationResult(true, isSigned, hasPeriod, periodIndex);
            }

            public static ValidationResult IsValidNumberToConvert(ref string value)
            {
                ReadOnlySpan<char> valueAsSpan = value.AsSpan();
                return IsValidNumberToConvert(ref valueAsSpan);
            }

          
        }
    
}