using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimworldReadableNumbers.Utility
{
    public static class Processing
    {
        public static (bool isSuccess, object[] modifiedObjects) ProcessPatchArguments(ref object[] arguments)
        {
            if (arguments == null || arguments.Length > byte.MaxValue) return (false, null);
            object[] modifiedObjects = new object[arguments.Length];
            bool hasModified = false;
            for(byte i = 0; i < arguments.Length; i++)
            {
                // Skip if argument is not a string or NamedArgument
                if (Validation.IsAllowedType(ref arguments[i]) == false) continue;

                var formatNumberResult = Text.FormatNumber(ref arguments[i]);
                if (formatNumberResult.isSuccess)
                {
                    hasModified = true;
                    modifiedObjects[i] = formatNumberResult.formattedObject;
                }
                else
                {
                    modifiedObjects[i] = arguments[i];
                }
                
            }
            return (hasModified, modifiedObjects);
        }

        public static void ProcessStringReference(ref string label)
        {
            if (label == null
                || label.Length <= 3 // skip if result string is too short to need a separator
                //|| Current.ProgramState != ProgramState.Playing
                //|| Current.Game.CurrentMap == null
               ) return;

            (string[] tokens, bool hasAnyNumbers) = Utility.Processing.TokeniseString(label);
            if (!hasAnyNumbers) return;
            
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < tokens.Length; i++)
            {
                if (tokens[i] == null) break;
                if (Utility.Validation.IsValidNumberToConvert(ref tokens[i]).IsValid)
                {
                    Utility.Text.FormatNumber(ref tokens[i]);
                };
                stringBuilder.Append(tokens[i]);
            }
            label = stringBuilder.ToString();
        }

        public static (string[] Tokens, bool hasAnyNumbers) TokeniseString(string originalString)
        {
            // TODO implement ObjectPool<StringBuilder> 
            // ArrayPool<T> for return value?
            //string[] tokens = ArrayPool<string>.Shared.Rent(originalString.Length);
            string[] tokens = new string[originalString.Length];
            var sb = new StringBuilder(originalString.Length);
            var charArray = originalString.ToCharArray();
            int tokenCount = 0;
            bool hasAnyNumbers = false;
            for(int i = 0; i < originalString.Length; i++)
            {
                char currentChar = charArray[i];
                sb.Append(currentChar);
                if (i == originalString.Length - 1  // End of the original string
                    || (!Char.IsNumber(currentChar) && currentChar != '.' && Char.IsNumber(charArray[i + 1]) && charArray[i + 1] != '.') // Is start of number
                    || (Char.IsNumber(currentChar) && currentChar != '.' && !Char.IsNumber(charArray[i + 1]) && charArray[i + 1] != '.') // Is end of number
                    )
                {
                    if(Char.IsNumber(currentChar) && hasAnyNumbers == false) hasAnyNumbers = true;
                    string token = sb.ToString();
                    tokens[tokenCount] = token;
                    tokenCount += 1;
                    sb.Clear();
                }
            }
            
            
            return (tokens, hasAnyNumbers);
        }

    }
}