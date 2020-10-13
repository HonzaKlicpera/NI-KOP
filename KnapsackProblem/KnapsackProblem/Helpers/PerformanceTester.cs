using KnapsackProblem.Common;
using KnapsackProblem.ConstructiveVersion;
using KnapsackProblem.ConstructiveVersion.Strategies;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace KnapsackProblem.Helpers
{
    public static class PerformanceTester
    {
        public static IList<ConstructiveResult> SolveWithPerformanceTest(IList<KnapsackInstance> instances, ConstructiveStrategy strategy, CommandLineOptions options)
        {
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;

            var stopWatch = new Stopwatch();
            var results = new List<ConstructiveResult>();
            
            foreach (var instance in instances)
            {
                Console.WriteLine($"Processing instance no. {instance.Id}");
                ConstructiveResult result = null;

                for(int i = 0; i < options.RepeatCount; i++)
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    GC.TryStartNoGCRegion(100000000);
                    stopWatch.Start();
                    result = strategy.Solve(instance);
                    stopWatch.Stop();
                    GC.EndNoGCRegion();
                }
                var averageRuntime = (double) stopWatch.ElapsedMilliseconds / options.RepeatCount;

                if (result != null)
                {
                    result.DataSetName = options.DataSetName;
                    result.RunTimeMs = averageRuntime;
                    result.Strategy = options.Strategy;
                    results.Add(result);
                }

                stopWatch.Reset();
            }

            return results;
        }
    }
}
