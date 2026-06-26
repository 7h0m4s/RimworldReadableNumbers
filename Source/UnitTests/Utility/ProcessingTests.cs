using System;
using NUnit.Framework;
using HarmonyLib;
using UnityEngine;
using System.Buffers;
namespace RRN_UnitTests.Utility
{


    [TestFixture]
    public class ProcessingTests
    {
        [SetUp]
        public static void Setup()
        {
            ReadOnlySpan<char> input = "1000.0-00.000".AsSpan();
        }

        [TestFixture]
        public class ProcessStringReference
        {
            // Valid
            [Test]
            public void ShouldReturnSuccessfully()
            {
                var originalString = "abc1000.1000.1000999xyz";
                var updatedString = (string)originalString.Clone();
                RimworldReadableNumbers.Utility.Processing.ProcessStringReference(ref updatedString);
                TestContext.WriteLine($"Original Label:{originalString}");
                TestContext.WriteLine($"Updated Label:{updatedString}");
            }
        }

        [TestFixture]
        public class TokeniseString
        {
            // Valid
            [Test]
            public void ShouldReturnSuccessfully()
            {
                var originalString = "abc999tpg1000.1000.1000999xyz".ToCharArray();
                bool hasAnyNumbers = false;
                var tokens= RimworldReadableNumbers.Utility.Processing.TokeniseString(originalString, ref hasAnyNumbers);
                TestContext.WriteLine($"Has Any Numbers:{hasAnyNumbers}");
                TestContext.WriteLine($"Original String:\n{originalString}");
                TestContext.WriteLine($"Result:\n{String.Join("\n", tokens.ToString())}");
            }
        }
    }
}