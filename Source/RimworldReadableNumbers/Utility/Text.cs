using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimworldReadableNumbers.Utility
{
    public static class Text
    {
        /// <summary>
        ///  The utility to convert solid numbers to seperated numbers
        ///  1000000 -> 1,000,000
        /// </summary>
        /// <returns>Boolean</returns>
        public static (bool isSuccess, string formattedNumber) FormatNumber(ref string strValue)
        {
            ValidationResult validationResult = Validation.IsValidNumberToConvert(ref strValue);
            if (validationResult.IsValid == false) return (false, null);
            var stringArgLength = strValue.Length;
            sbyte periodIndex = validationResult.PeriodIndex;
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
        
        /// <inheritdoc cref="FormatNumber(ref string)"/>>
        public static (bool isSuccess, NamedArgument formattedNumber) FormatNumber(ref NamedArgument namedArgument)
        {
            if(namedArgument.arg is string namedArgumentString == false) return (false, new NamedArgument());
            var formatNumberResult = FormatNumber(ref namedArgumentString);
            namedArgument.arg = formatNumberResult.formattedNumber;
            return (formatNumberResult.isSuccess, namedArgument);
        }

        /// <inheritdoc cref="FormatNumber(ref string)"/>>
        public static (bool isSuccess, object formattedObject) FormatNumber(ref object objectRef)
        {
            if (objectRef == null) return (false, null);
            switch (objectRef)
            {
                case string stringRef:
                    return FormatNumber(ref stringRef);
                case NamedArgument namedArgumentRef:
                    return FormatNumber(ref namedArgumentRef);
                case string[] stringArrayRef:
                    string[] modifiedStringArray = new string[stringArrayRef.Length];
                    bool isStringArrayRefModified = false;
                    for(int i = 0; i < stringArrayRef.Length; i++)
                    {
                        var formatNumberResult = FormatNumber(ref stringArrayRef[i]);
                        if (formatNumberResult.isSuccess)
                        {
                            isStringArrayRefModified = true;
                            modifiedStringArray[i] = formatNumberResult.formattedNumber;
                        }
                    }
                    return (isStringArrayRefModified, modifiedStringArray);
                case object[] objectArrayRef:
                    bool isObjectArrayRefModified = false;
                    object[] modifiedObjectArray = new object[objectArrayRef.Length];
                    for(int i = 0; i < objectArrayRef.Length; i++)
                    {
                        var formatNumberResult = FormatNumber(ref objectArrayRef[i]);
                        if (formatNumberResult.isSuccess)
                        {
                            isObjectArrayRefModified = true;
                            modifiedObjectArray[i] = formatNumberResult.formattedObject;;
                        }
                    }
                    return (isObjectArrayRefModified, modifiedObjectArray);
                case IEnumerable<object> tEnumerableRef:
                    bool isEnumerableRefModified = false;
                    List<object> modifiedEnumerable = tEnumerableRef.ToList();
                    for(int i = 0; i < tEnumerableRef.Count(); i++)
                    {
                        var item = tEnumerableRef.ElementAt(i);
                        var formatNumberResult = FormatNumber(ref item);
                        if (formatNumberResult.isSuccess)
                        {
                            isEnumerableRefModified = true;
                            modifiedEnumerable[i] = formatNumberResult.formattedObject;;
                        }
                    }
                    return (isEnumerableRefModified,  modifiedEnumerable);
                case TaggedString a:
                    string rawText = a.RawText;
                    return FormatNumber(ref rawText);
                case object a:
                    return (false, null);
                default:
                    return (false, null);
            }
            
        }
    }
}