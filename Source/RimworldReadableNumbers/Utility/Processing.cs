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
        private static Token[] _tokens  = new Token[short.MaxValue];
        private static short _tokenCount = 0;
        private static short _tokenLength = 0;
        private static bool _hasAnyNumbers = false;
        private static Memory<char> _resultMemory =  new Memory<char>(new char[short.MaxValue]);
        private static int _resultLength = 0;
        private static Dictionary<string,string> _resultCache = InitialiseResultCache();
        
        
        private static readonly char[] _colourTagPrefix = "<color=".ToCharArray();
        private static short _colourTagIndex = 0;
        private static bool _isColourTag = false;
        private static Rect dummyRect = new Rect(float.MinValue, float.MinValue, float.MinValue, float.MinValue);
        private static bool _hasLabelChanged = false;
        

        private struct Token
        {
            public short StartIndex;
            public short Length;
            public bool HasNumber;
        }


        /// <summary>
        /// Returns True if label was modified
        /// Returns False if no formatting was performed
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public static bool ProcessLabel(ref string label)
        {
            ReadOnlySpan<char> labelSpan = label.AsSpan();
            if (labelSpan == null
                || labelSpan.Length <= 2 // skip if result string is too short to need a separator
                || labelSpan.Length > short.MaxValue - 1 // skip if string is too big
                || !RnSetting.Enable
                || Patching.DisableReadableNumberFormatting // skip if formatting has already been done earlier
                // || Current.ProgramState != ProgramState.Playing
                // || Current.Game.CurrentMap == null
               ) return false;
            
            // Reset _hasLabelChanged
            _hasLabelChanged = false;
            
            // 1. Try to get result from cache
            if (TryResultCache(ref label)) return _hasLabelChanged;
            
            // 2. Try to quickly prove that the string is not valid for formatting
            // e.g. not enough sequential numbers
            if (!Validation.HasEnoughDigitsAndNotBlackListed(ref labelSpan))
            {
                TryAddToResultCache(ref label, null);
                return _hasLabelChanged;
            }
            
            // 3. Split the string up into tokens of sequential digits and sequential non-digits
            _hasAnyNumbers = false;
            Utility.Processing.TokeniseString(labelSpan);
            
            // 4. Abort formatting and save result to cache if no valid Number tokens were found
            if (!_hasAnyNumbers)
            {
                TryAddToResultCache(ref label, null);
                return _hasLabelChanged;
            }

            // 5. Perform formatting process and update original label
            CompileLabelResult(ref label, ref labelSpan);
            
            return _hasLabelChanged;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryResultCache(ref string label)
        {
            if(RnSetting.CacheEnable == false) return false;
            if (_resultCache.TryGetValue(label, out string resultValue))
            {
                if (resultValue == null)
                {
                    return true; // Cached as no formatting needed
                }
                label = resultValue;
                _hasLabelChanged = true;
                return true;
            };
            return false;
        }        
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryAddToResultCache(ref string label, string resultValue)
        {
            if(RnSetting.CacheEnable == false) return false;
            if(RnSetting.Debug && _resultCache.Count % 1000 == 0) Log.Message($"Readable Numbers: Cache capacity at {_resultCache.Count} of {RnSetting.CacheMaxCapacity}");
            if (_resultCache.Count > RnSetting.CacheMaxCapacity) _resultCache.Clear();
            return _resultCache.TryAdd(label, resultValue);
        }      
        
        public static void ClearResultCache(bool forceNewCache = false)
        {
            _resultCache = InitialiseResultCache();
        }

        private static Dictionary<string,string> InitialiseResultCache()
        {
            return new Dictionary<string, string>(new Dictionary<string, string>(RnSetting.CacheMaxCapacity), StringComparer.Ordinal);
        }
        
        private static void TokeniseString(ReadOnlySpan<char> stringReadOnlySpan)
        {
            _tokenCount = 0;
            _tokenLength = 0;
            char decimalSeparator = '.'; 
            _hasAnyNumbers = false;
            
            ReadOnlySpan<char> colourTagPrefix = _colourTagPrefix.AsSpan();
            _colourTagIndex = 0;
            _isColourTag = false;
            
            
            bool isCurrentTokenContainingNumber = false;
            for(short i = 0; i < stringReadOnlySpan.Length; i++)
            {
                //char previousChar = i == 0 ? 'A' :charArray[i - 1];
                char currentChar = stringReadOnlySpan[i];
                char nextChar = i == stringReadOnlySpan.Length - 1 ? 'A' :stringReadOnlySpan[i + 1];
                //bool isPreviousCharDigit = char.IsNumber(previousChar);
                bool isCurrentCharDigit = char.IsNumber(currentChar);
                bool isNextCharDigit = char.IsNumber(nextChar);
                
                _tokenLength++;
                if(isCurrentCharDigit) isCurrentTokenContainingNumber = true;
                
                //ColourTag Detection <color=#808080FF>
                if (!_isColourTag && colourTagPrefix[_colourTagIndex] == currentChar)
                {
                    if (_colourTagIndex == colourTagPrefix.Length - 1)
                    {
                        _isColourTag = true;
                    }
                    _colourTagIndex++;
                }
                else
                {
                    _colourTagIndex = 0;
                }
                
                
                if (i == stringReadOnlySpan.Length - 1  // End of the original string
                    || (!isCurrentCharDigit && currentChar != decimalSeparator && isNextCharDigit && nextChar != decimalSeparator && !_isColourTag) // Is start of number and not ColourTag
                    || (isCurrentCharDigit && currentChar != decimalSeparator && !isNextCharDigit && nextChar != decimalSeparator && !_isColourTag) // Is end of number not ColourTag
                    || (_isColourTag && currentChar == '>') // Is end of ColourTag
                   )
                {
                    // Flag that this set of Tokens is worth formatting
                    if (isCurrentCharDigit)
                    {
                        if (_hasAnyNumbers == false) _hasAnyNumbers = true;
                    }
                    
                    // Store current Token
                    _tokens[_tokenCount].StartIndex = (short)(i - _tokenLength + 1); // Calculate start index
                    _tokens[_tokenCount].Length = _tokenLength;
                    _tokens[_tokenCount].HasNumber = isCurrentTokenContainingNumber && !_isColourTag; // ColourTags don't count as numbers
                    
                    // Reset values
                    _tokenCount += 1;
                    _tokenLength = 0;
                    _isColourTag = false;
                    isCurrentTokenContainingNumber = false;
                } 
            }
        }


        private static void CompileLabelResult(ref string label, ref ReadOnlySpan<char> labelSpan)
        {
            _resultLength = 0;
            var resultSpan = _resultMemory.Span;
            
            // Process each token and reconstruct original string
            for (int i = 0; i < _tokenCount; i++)
            {
                // Slice the current token from original Label's span
                Token currentToken = _tokens[i];
                ReadOnlySpan<char> currentTokenSlice = labelSpan.Slice(currentToken.StartIndex, currentToken.Length);
                bool tokenHasNumberItem = currentToken.HasNumber;
                
                // Token isn't a number so doesn't need to be formatted
                if (tokenHasNumberItem == false)
                {
                    currentTokenSlice.CopyTo(resultSpan.Slice(_resultLength, currentToken.Length));
                    _resultLength += currentToken.Length;
                    continue;
                }
                
                // Format number token
                bool isSuccess = false;
                ReadOnlySpan<char> formattedNumber = Utility.Text.FormatNumberWithStringManipulation(ref currentTokenSlice, ref isSuccess);
                if (isSuccess)
                {
                    formattedNumber.CopyTo(resultSpan.Slice(_resultLength, formattedNumber.Length));
                    _resultLength += formattedNumber.Length;
                    _hasLabelChanged =  true;
                }
                else
                {
                    // Format failed - fallback to copying original label's span
                    currentTokenSlice.CopyTo(resultSpan.Slice(_resultLength, currentToken.Length));
                    _resultLength += currentToken.Length;
                }
            }

            if (_hasLabelChanged)
            {
                // Save formatted result and override original Label's value
                string resultValue = _resultMemory.Slice(0,_resultLength).ToString();
                TryAddToResultCache(ref label, resultValue);
                label = resultValue;
            }
            else
            {
                // No formatting occured, but store the result in cache anyway
                // to skip this process in the future
                TryAddToResultCache(ref label, label);
            }
        }
      

    }
}