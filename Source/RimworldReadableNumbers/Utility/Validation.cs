namespace RimworldReadableNumbers.Utility
{
        public static class Validation
        {
            public static bool IsValidNumberToConvert(string value)
            {
                var index = 0;
                var digitsWithoutPeriod = 0;
                var hasNegativeSign = false;
                var hasPeriod = false;
                foreach (char c in value)
                {
                    if (!char.IsDigit(c) && c != '.' && c != '-' && c != '+') return false;

                    if (c == '.')
                    {
                        if (hasPeriod) return false; // more than one period in string
                        hasPeriod = true;
                    }


                    if (index == 0 && (c == '-' || c == '+'))
                    {
                        hasNegativeSign = true;
                    }
                    else
                    {
                        if (hasPeriod == false && c != '.') digitsWithoutPeriod++;
                    }

                    if (hasPeriod && digitsWithoutPeriod <= 3) return false;
                    index++;
                }

                if (index <= 2 || (hasNegativeSign && index <= 3))
                    return false; // number is too short to need separator
                return true;
            }
        }
    
}