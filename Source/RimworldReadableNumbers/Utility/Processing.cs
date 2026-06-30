using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Verse;

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
        private static Memory<char> _resultMemory =  new Memory<char>(new char[short.MaxValue]);
        private static int _resultLength = 0;
        private static Dictionary<string,string> _resultCache = new Dictionary<string,string>();
        
        public static void ProcessLabel(ref string label)
        {
            float currentTime = Time.time;
            ReadOnlySpan<char> labelSpan = label.AsSpan();
            if (labelSpan == null
                || labelSpan.Length <= 3 // skip if result string is too short to need a separator
                || labelSpan.Length > short.MaxValue // skip if string is too big
                || !RnSetting.Enable
                || Current.ProgramState != ProgramState.Playing
                || Current.Game.CurrentMap == null
               ) return;
            
            if (TryResultCache(ref label)) return;
            if (!Validation.HasEnoughDigits_And_Not_BlackListed(ref labelSpan))
            {
                TryAddToResultCache(ref label, null);
                return;
            }
            
            _hasAnyNumbers = false;
            Utility.Processing.TokeniseString(labelSpan);
            if (!_hasAnyNumbers)
            {
                TryAddToResultCache(ref label, null);
                return;
            }

            ConstructLabelResult(ref label);

        }

        private static bool TryResultCache(ref string label)
        {
            var successfullyGotResultValue = _resultCache.TryGetValue(label, out string resultValue);
            if (successfullyGotResultValue == true)
            {
                if (resultValue == null) return true; // Cached as no formatting needed
                label = resultValue;
                return true;
            };
            return false;
        }        
        private static bool TryAddToResultCache(ref string label, string resultValue)
        {
            // TODO set configurable cache clear threshold and/or timed cache clear 
            if(_resultCache.Count % 100 == 0) Log.Message($"Readable Numbers: Cache count of {_resultCache.Count}");
            if (_resultCache.Count > 10000) _resultCache.Clear();
            return _resultCache.TryAdd(label, resultValue);
        }      
        public static void ClearResultCache()
        {
            _resultCache.Clear();
            return;
        }
        
        public static void TokeniseString(ReadOnlySpan<char> originalString)
        {
            // TODO implement ObjectPool<StringBuilder> 
            // ArrayPool<T> for return value?
            //string[] tokens = ArrayPool<string>.Shared.Rent(originalString.Length);
            
            StringBuilder sb = _tokenStringBuilder;
            ReadOnlySpan<char> charArray = originalString;
            _tokenCount = 0;
            char decimalSeparator = '.'; //RnSetting.DecimalSeparator;
            _hasAnyNumbers = false;
            bool isCurrentTokenContainingNumber = false;
            for(short i = 0; i < originalString.Length; i++)
            {
                char previousChar = i == 0 ? 'A' :charArray[i - 1];
                char currentChar = charArray[i];
                char nextChar = i == originalString.Length - 1 ? 'A' :charArray[i + 1];
                bool isPreviousCharDigit = Char.IsNumber(previousChar);
                bool isCurrentCharDigit = Char.IsNumber(currentChar);
                bool isNextCharDigit = Char.IsNumber(nextChar);
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


        public static void ConstructLabelResult(ref string label)
        {
            bool hasAnySuccessfulFormats = false;
            _resultLength = 0;
            var resultSpan = _resultMemory.Span;
            // Process each token and reconstruct original string
            for (int i = 0; i < _tokenCount; i++)
            {
                Span<char> currentToken = _tokens[i];
                bool tokenHasNumberItem = _tokenHasNumberArray[i];
                if (currentToken == null) break;
                if (tokenHasNumberItem == false)
                {
                    currentToken.CopyTo(resultSpan.Slice(_resultLength, currentToken.Length));
                    _resultLength += currentToken.Length;
                    continue;
                }
                bool isSuccess = false;
                ReadOnlySpan<char> formattedNumber = Utility.Text.FormatNumberWithStringManipulation(ref currentToken, ref isSuccess);
                if (isSuccess)
                {
                    formattedNumber.CopyTo(resultSpan.Slice(_resultLength, formattedNumber.Length));
                    _resultLength += formattedNumber.Length;
                    hasAnySuccessfulFormats =  true;
                }
                else
                {
                    currentToken.CopyTo(resultSpan.Slice(_resultLength, currentToken.Length));
                    _resultLength += currentToken.Length;
                }
            }

            if (hasAnySuccessfulFormats)
            {
                string resultValue = _resultMemory.Slice(0,_resultLength).ToString();
                TryAddToResultCache(ref label, resultValue);
                label = resultValue;
            }
            else
            {
                TryAddToResultCache(ref label, label);
            }
        }
      

    }
}