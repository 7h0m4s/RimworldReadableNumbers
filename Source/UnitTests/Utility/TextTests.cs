using NUnit.Framework;

namespace RRN_UnitTests.Utility
{
    [TestFixture]
    public class TextTests
    {

        [TestFixture]
        public class FormatNumber
        {
            [Test]
            public void IsValidNumberToConvert_OneThousand_ReturnsTrue()
            {
                string originalString = "1000.0-00.000";
                var result = RimworldReadableNumbers.Utility.Text.FormatNumberWithTryParse(ref  originalString);
                TestContext.WriteLine($"Original String:{originalString}");
                TestContext.WriteLine($"Formatted String:{result}");
            }
            
           
        }

    }
}