namespace RimworldReadableNumbers.Utility
{
    public static class Text
    {
        /// <summary>
        ///  The utility to convert solid numbers to seperated numbers
        ///  1000000 -> 1,000,000
        /// </summary>
        /// <param name="strValue">String to be formatted</param>
        /// <returns>Boolean</returns>
        public static bool FormatNumber(ref string strValue)
        {
            var stringArgLength = strValue.Length;
            var periodIndex = strValue.IndexOf('.');
            if (periodIndex != -1)
            {
                if (float.TryParse(strValue, out float floatOut))
                {
                    strValue = strValue.Replace(strValue,
                        floatOut.ToString($"N{stringArgLength - periodIndex - 1}"));
                    return true;
                }
            }
            else
            {
                if (int.TryParse(strValue, out int intOut))
                {
                    strValue = strValue.Replace(strValue, intOut.ToString("N0"));
                    return true;
                }
            }

            return false;
        }
    }
}