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
    public class PerformanceTester
    {
        public delegate void InstanceCalculationFinished();
        public  event InstanceCalculationFinished RaiseInstanceCalculationFinished;

        public static int REPEAT_COUNT = 5;

        private void PreparePerformanceTest(IList<KnapsackInstance> instances)
        {
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;

            //Warmup
            //strategy.SolveAll(instances.Take(15).ToList(), "", "");
        }

        private KnapsackConfiguration GetOptimalConfiguration(KnapsackInstance instance)
        {
            var branchAndBoundSolver = new BranchBoundSolver();
            var optimalSolution = branchAndBoundSolver.Solve(instance);
            return optimalSolution.Configuration;
        }

        private float GetEpsilonOfSolution(int resultPrice, int optimalPrice)
        {
            if (Math.Max(resultPrice, optimalPrice) == 0)
                return 0;
            else
                return  (float)Math.Abs(resultPrice - optimalPrice) / Math.Max(resultPrice, optimalPrice);
        }

        public IList<KnapsackResult> SolveWithPerformanceTest(IList<KnapsackInstance> instances, AnnealingOptions options)
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
                    var optimalConfiguration = GetOptimalConfiguration(instance);
                    result.RunTimeMs = averageRuntime;
                    result.OptimalConfiguration = optimalConfiguration;
                    result.Epsilon = GetEpsilonOfSolution(result.Configuration.Price, optimalConfiguration.Price);
                    results.Add(result);
                }
                RaiseInstanceCalculationFinished();
                stopWatch.Reset();
            }

            return results;
        }
    }
}
