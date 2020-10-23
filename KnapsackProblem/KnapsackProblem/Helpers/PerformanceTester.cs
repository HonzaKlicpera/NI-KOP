using KnapsackProblem.Common;
using KnapsackProblem.ConstructiveVersion;
using KnapsackProblem.ConstructiveVersion.Strategies;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Linq;

namespace KnapsackProblem.Helpers
{
    public static class PerformanceTester
    {
        public static int REPEAT_COUNT = 5;

        private static void PreparePerformanceTest(IList<KnapsackInstance> instances, ConstructiveStrategy strategy)
        {
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;

            //Warmup
            strategy.SolveAll(instances.Take(15).ToList(), "", "");
        }

        public static IList<ConstructiveResult> SolveWithPerformanceTest(IList<KnapsackInstance> instances, ConstructiveStrategy strategy, CommandLineOptions options)
        {
            PreparePerformanceTest(instances, strategy);

            var stopWatch = new Stopwatch();
            var results = new List<ConstructiveResult>();
            
            foreach (var instance in instances)
            {
                Console.WriteLine($"Processing instance no. {instance.Id}");
                ConstructiveResult result = null;

                //The algorithm must run repeat at least the set amount of times;
                for(int i = 0; i < REPEAT_COUNT; i++)
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    //GC.TryStartNoGCRegion(200000000);
                    stopWatch.Start();
                    result = strategy.Solve(instance);
                    stopWatch.Stop();
                    strategy.FreeAll();
                    //GC.EndNoGCRegion();
                }
                var averageRuntime = stopWatch.Elapsed.TotalMilliseconds / REPEAT_COUNT;

                //Save only the last result
                if (result != null)
                {
                    result.DataSetName = options.DataSetName;
                    result.RunTimeMs = averageRuntime;
                    if(result.Strategy == null)
                        result.Strategy = options.Strategy;
                    results.Add(result);
                }

                stopWatch.Reset();
            }

            return results;
        }
    }
}
