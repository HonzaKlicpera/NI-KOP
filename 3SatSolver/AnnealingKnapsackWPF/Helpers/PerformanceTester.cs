using AnnealingWPF.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Linq;
using AnnealingWPF.Solver;
using AnnealingWPF.Common;


namespace AnnealingWPF.Helpers
{
    public class PerformanceTester
    {
        public delegate void InstanceCalculationFinished();
        public  event InstanceCalculationFinished RaiseInstanceCalculationFinished;

        public static int REPEAT_COUNT = 5;

        private void PreparePerformanceTest(IList<SatInstance> instances)
        {
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;

            //Warmup
            //strategy.SolveAll(instances.Take(15).ToList(), "", "");
        }

        private float GetEpsilonOfSolution(int resultPrice, int optimalPrice)
        {
            if (Math.Max(resultPrice, optimalPrice) == 0)
                return 0;
            else
                return  (float)Math.Abs(resultPrice - optimalPrice) / Math.Max(resultPrice, optimalPrice);
        }

        public IList<SatResult> SolveWithPerformanceTest(IList<SatInstance> instances, AnnealingOptions options)
        {
            PreparePerformanceTest(instances);

            var stopWatch = new Stopwatch();
            var results = new List<SatResult>();
            
            foreach (var instance in instances)
            {
                Console.WriteLine($"Processing instance no. {instance.Id}");
                SatResult result = null;

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
                    //TODO - load optimal configuration
                    result.RunTimeMs = averageRuntime;
                    result.Epsilon = GetEpsilonOfSolution(result.Configuration.GetOptimalizationValue(), result.OptimalConfiguration.GetOptimalizationValue());
                    results.Add(result);
                }
                RaiseInstanceCalculationFinished();
                stopWatch.Reset();
            }

            return results;
        }
    }
}
