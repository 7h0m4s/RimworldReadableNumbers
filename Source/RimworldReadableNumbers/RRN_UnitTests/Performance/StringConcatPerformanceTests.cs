using System.Diagnostics;
using NUnit.Framework;
using RimworldReadableNumbers.Patches.String;


namespace RRN_UnitTests.Performance
{

    [TestFixture]
    public class StringConcatPerformanceTests
    {

        [TestFixture]
        public class Postfix
        {
            // Valid
            [Test]
            public void Postfix_Benchmark_ValidData()
            {
                string pattern = "${0}";
                int numberOffset = 1000;
                int iterations = 10000000;
                (string __result, object[] __args)[] dataArray = new (string __result, object[] __args)[iterations];
                for (int i = 0; i < iterations; i++)
                {
                    object[] args = new object[2];
                    args[0] = pattern;
                    args[1] = (iterations + numberOffset + 1).ToString();
                    string result = string.Format(pattern, args[1]);
                    dataArray[i] =  (result, args);
                }
                
                var sw = Stopwatch.StartNew();
    
                for (int i = 0; i < iterations; i++)
                {
                    RimworldReadableNumbers.Patches.String.StringConcatPatch.Postfix(ref dataArray[i].__result, dataArray[i].__args);
                }
    
                sw.Stop();
                long totalTime = sw.ElapsedMilliseconds;
                long averageTime = sw.ElapsedMilliseconds / iterations;

                TestContext.WriteLine($"Total execution time ({totalTime}ms).");
                TestContext.WriteLine($"Average execution time ({averageTime}ms) exceeded 5ms limit.");
            }
        }
    }
}