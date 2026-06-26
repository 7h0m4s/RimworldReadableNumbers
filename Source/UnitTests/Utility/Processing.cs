using System;
using NUnit.Framework;
using HarmonyLib;
using UnityEngine;
using System.Buffers;

namespace RRN_UnitTests.Utility
{


    [TestFixture]
    public class Processing
    {

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
                var ( tokens, hasAnyNumbers )= RimworldReadableNumbers.Utility.Processing.TokeniseString("abc1000.1000.1000999xyz");
                TestContext.WriteLine($"Has Any Numbers:{hasAnyNumbers}");
                TestContext.WriteLine($"Result:\n{String.Join("\n", tokens)}");
            }
        }
    }
}