using System;
using NUnit.Framework;


namespace RRN_UnitTests.Utility
{
    [TestFixture]
    public class ValidationTests
    {
    
        [SetUp]
        public static void Setup()
        {
            ReadOnlySpan<char> input = "1000.0-00.000".AsSpan();
        }
        
        [TestFixture]
        public class IsValidNumberToConvert
        {
            // Valid
            [Test]
            public void IsValidNumberToConvert_OneThousand_ReturnsTrue()
            {
                string str = "1000";
                var result = RimworldReadableNumbers.Utility.Validation.IsValidNumberToConvert(ref  str);
                Assert.True(result.IsValid);
                Assert.True(result.HasPeriod == false);
                Assert.True(result.IsSigned == false);
            }
            
            // Not Valid
            [Test]
            public void IsValidNumberToConvert_NullValue_ReturnFalse()
            {
                string testValue = null;
                var result = RimworldReadableNumbers.Utility.Validation.IsValidNumberToConvert(ref testValue);
                Assert.False(result.IsValid);
            }
            [Test]
            public void IsValidNumberToConvert_MultiplePeriods_ReturnFalse()
            {
                string testValue = "1000.000.00";
                var result = RimworldReadableNumbers.Utility.Validation.IsValidNumberToConvert(ref testValue);
                Assert.False(result.IsValid);
            }
            [Test]
            public void IsValidNumberToConvert_GreaterThan128String_ReturnFalse()
            {
                string testValue = new string('9', 128);;
                var result = RimworldReadableNumbers.Utility.Validation.IsValidNumberToConvert(ref testValue);
                Assert.False(result.IsValid);
            }
        }

    }
}