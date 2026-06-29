using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace RimworldReadableNumbers.Utility
{
    public static class Processing
    {
        private static StringBuilder _processStringBuilder = new StringBuilder(short.MaxValue);
        private static StringBuilder _tokenStringBuilder = new StringBuilder(short.MaxValue);
        private static char[][] _tokens  = new char[short.MaxValue][];
        private static bool[] _tokenHasNumberArray = new bool[short.MaxValue];
        private static short _tokenCount = 0;
        private static bool _hasAnyNumbers = false;
        private static Memory<char> resultSpan =  new Memory<char>(new char[short.MaxValue]);
        
        public static void ProcessStringReference(ref string label)
        {
            ReadOnlySpan<char> labelSpan = label.AsSpan();
            // TODO Test all functions with   [MethodImpl(MethodImplOptions.AggressiveInlining)]
            // only use the attribute if we can prove it performs better
            if (labelSpan == null
                || labelSpan.Length <= 3 // skip if result string is too short to need a separator
                || labelSpan.Length > short.MaxValue // skip if string is too big
                || !RN_Setting.Enable
                //|| Current.ProgramState != ProgramState.Playing
                //|| Current.Game.CurrentMap == null
               ) return;
            
            // Convert to  ReadOnlySpan<char> for performance
            
             //if (!Validation.HasEnoughDigitsForFormatting_Short(ref labelSpan)) return;
             if (!Validation.HasEnoughDigitsForFormatting_Short_Unrolled(ref labelSpan)) return;
            
            
             _hasAnyNumbers = false;
            Utility.Processing.TokeniseString(labelSpan);
            if (!_hasAnyNumbers) return;
            
            // Set StringBuilder buffer to be large enough to hold resulting string if max
            // digit separators are used
            StringBuilder stringBuilder = _processStringBuilder;
            stringBuilder.Clear();
            bool hasAnySuccessfulFormats = false;
            
            // Process each token and reconstruct original string
            for (short i = 0; i < _tokenCount; i++)
            {
                ReadOnlySpan<char> currentToken = _tokens[i];
                bool tokenHasNumberItem = _tokenHasNumberArray[i];
                if (currentToken == null) break;
                if (tokenHasNumberItem == false)
                {
                    stringBuilder.Append(currentToken);
                    continue;
                }
                bool isSuccess = false;
                ReadOnlySpan<char> formattedNumber = Utility.Text.FormatNumberWithStringManipulation(ref currentToken, ref isSuccess);
                if (isSuccess)
                {
                    stringBuilder.Append(formattedNumber);
                    hasAnySuccessfulFormats =  true;
                }
                else
                {
                    stringBuilder.Append(currentToken);
                }
            }
            if(hasAnySuccessfulFormats) label = stringBuilder.ToString();
        }
        
        public static void TokeniseString(ReadOnlySpan<char> originalString)
        {
            // TODO implement ObjectPool<StringBuilder> 
            // ArrayPool<T> for return value?
            //string[] tokens = ArrayPool<string>.Shared.Rent(originalString.Length);
            
            StringBuilder sb = _tokenStringBuilder;
            ReadOnlySpan<char> charArray = originalString;
            _tokenCount = 0;
            char decimalSeparator = RN_Setting.DecimalSeparator;
            _hasAnyNumbers = false;
            for(short i = 0; i < originalString.Length; i++)
            {
                char previousChar = i == 0 ? 'A' :charArray[i - 1];
                char currentChar = charArray[i];
                char nextChar = i == originalString.Length - 1 ? 'A' :charArray[i + 1];
                bool isPreviousCharDigit = Char.IsNumber(previousChar);
                bool isCurrentCharDigit = Char.IsNumber(currentChar);
                bool isNextCharDigit = Char.IsNumber(nextChar);
                bool isCurrentTokenContainingNumber = false;
                // if (isPreviousCharDigit && isNextCharDigit && currentChar == ',') // Doesn't need DigitSeparator setting
                // {
                //     // Skip if comma already exists between 2 numbers "0,0"
                //     // to avoid formatting World Debug ID and Coordinates
                //     // e.g. "(123,456)"
                //     hasAnyNumbers = false;
                //     return null;
                // }
                sb.Append(currentChar);
                if(isCurrentCharDigit) isCurrentTokenContainingNumber = true;
                if (i == originalString.Length - 1  // End of the original string
                    || (!isCurrentCharDigit && currentChar != decimalSeparator && isNextCharDigit && nextChar != decimalSeparator) // Is start of number
                    || (isCurrentCharDigit && currentChar != decimalSeparator && !isNextCharDigit && nextChar != decimalSeparator) // Is end of number
                   )
                {
                    if (isCurrentCharDigit)
                    {
                        if (_hasAnyNumbers == false) _hasAnyNumbers = true;
                        
                    }
                    _tokens[_tokenCount] = sb.ToString().ToCharArray();
                    _tokenHasNumberArray[_tokenCount] = isCurrentTokenContainingNumber;
                    
                    _tokenCount += 1;
                    sb.Clear();
                    isCurrentTokenContainingNumber = false;
                } 
            }
            return;
        }

      

    }
}