using KnapsackProblem.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Linq;
using KnapsackAnnealing.Solver;
using KnapsackAnnealing.Common;

using KnapsackProblem.ConstructiveVersion.Strategies;

namespace KnapsackProblem.Helpers
{
    public static class PerformanceTester
    {
        public static int REPEAT_COUNT = 5;

        private static void PreparePerformanceTest(IList<KnapsackInstance> instances)
        {
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;

            //Warmup
            //strategy.SolveAll(instances.Take(15).ToList(), "", "");
        }


        public static float GetEpsilonOfSolution(int solutionPrice, KnapsackInstance instance)
        {
            var branchAndBoundSolver = new BranchBoundSolver();
            var optimalPrice = branchAndBoundSolver.Solve(instance).Configuration.Price;
            if (Math.Max(solutionPrice, optimalPrice) == 0)
                return 0;
            else
                return  (float)Math.Abs(solutionPrice - optimalPrice) / Math.Max(solutionPrice, optimalPrice);
        }

        public static IList<KnapsackResult> SolveWithPerformanceTest(IList<KnapsackInstance> instances, AnnealingOptions options)
        {
            PreparePerformanceTest(instances);

            var stopWatch = new Stopwatch();
            var results = new List<KnapsackResult>();
            
            foreach (var instance in instances)
            {
                Console.WriteLine($"Processing instance no. {instance.Id}");
                KnapsackResult result = null;

                //The algorithm must run repeat at least the set amount of times;
                for(int i = 0; i < REPEAT_COUNT; i++)
                {
                    var solver = new SimulatedAnnealingSolver(instance, options);
                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    //GC.TryStartNoGCRegion(200000000);
                    stopWatch.Start();
                    result = solver.Solve();
                    stopWatch.Stop();
                    //GC.EndNoGCRegion();
                }
                var averageRuntime = stopWatch.Elapsed.TotalMilliseconds / REPEAT_COUNT;

                //Save only the last result
                if (result != null)
                {
                    result.RunTimeMs = averageRuntime;
                    result.Epsilon = GetEpsilonOfSolution(result.Configuration.Price, instance);
                    //result.WatchedParameter = AnnealingSolverConfig.BASE_STARTING_TEMPERATURE.ToString();
                    results.Add(result);
                }

                stopWatch.Reset();
            }

            return results;
        }
    }
}
