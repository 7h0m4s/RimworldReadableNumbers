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
            public sbyte PeriodIndex{  get; }
            public ValidationResult(bool isValid, bool isSigned,  bool hasPeriod, sbyte periodIndex)
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
                this.PeriodIndex = -1;
            }
        }
        public static class Validation
        {
            
            public static ValidationResult IsValidNumberToConvert(ref string value)
            {
                if (value == null) return new ValidationResult(false);
                if (value.Length > sbyte.MaxValue) return new ValidationResult(false);
                sbyte index = 0;
                var digitsWithoutPeriod = 0;
                sbyte periodIndex = -1;
                var isSigned = false;
                var hasPeriod = false;
                foreach (char c in value)
                {
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

                if (value.Length - (isSigned? 1 : 0) - (periodIndex == -1? 0 : value.Length - periodIndex) <= 3 )
                    return new ValidationResult(false); // Too few digits (<3) before period to require processing
                return new ValidationResult(true, isSigned, hasPeriod, periodIndex);
            }

            public static bool IsAllowedType(ref object arg)
            {
                if (arg == null) return false;
                switch (arg)
                {
                    case string a:
                        return true;
                    case NamedArgument namedArgument:
                        return namedArgument.arg is string;
                    case string[] a:
                        return true;
                    case object[] a:
                        return true;
                    case IEnumerable<object> enumerable:
                        return true;
                    case object a:
                        try
                        {
                            // Try to convert to string
                            return a.ToString() is string;
                        }
                        catch (NullReferenceException)
                        {
                            // Can't convert to String
                        }
                        return false;
                    default:
                        return false;
                }
            }

            public static bool IsAllowedResult(string result)
            {
                byte numberOfSlashes = 0;
                foreach(var c in result.AsSpan())
                {
                    if (c == '\\' || c=='/')
                    {
                        numberOfSlashes++;
                        if (numberOfSlashes == 2)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }

            public static bool IsAllowedFormatPattern(object[] __args)
            {
                string formatPattern;
                if (__args[0] is IFormatProvider)
                {
                    formatPattern =  (string)__args[1];
                }
                else
                {
                    formatPattern =  (string)__args[0];
                }
                byte numberOfOpenBrackets = 0;
                foreach(var c in formatPattern.AsSpan())
                {
                    // Skip advanced String.Format() patterns. 
                    // e.g. {0:X2}{1:X2}{2:X2}{3:X2}
                    if (c == '{' )
                    {
                        numberOfOpenBrackets++;
                    }
                    if (c == '}')
                    {
                        if (numberOfOpenBrackets > 0)
                        {
                            numberOfOpenBrackets--;
                        }
                    }
                    if (c == ':' && numberOfOpenBrackets > 0)
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    
}