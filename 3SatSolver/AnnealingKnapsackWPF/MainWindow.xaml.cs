using AnnealingWPF.Common;
using AnnealingWPF.Helpers;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AnnealingWPF.Solver.CoolingStrategies;
using AnnealingWPF.Solver.EquilibriumStrategies;
using AnnealingWPF.Solver.FrozenStrategies;
using AnnealingWPF.Solver.ScoreStrategies;
using AnnealingWPF.Solver.StartingPositionStrategies;
using AnnealingWPF.Solver.TryStrategies;
using AnnealingKnapsackWPF.Solver.StartingPositionStrategies;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Diagnostics;

namespace AnnealingKnapsackWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private int numberOfFinishedInstances;
        private int numberOfInstancesTotal;

        private IList<SatInstance> loadedInstances;
        private string loadedInstanceLocation;

        private IDictionary<int, ReferenceConfiguration> loadedReferenceConfigurations;
        private string loadedReferenceConfigurationsLocation;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void OnInstanceCalculationFinished()
        {
            numberOfFinishedInstances++;
            Dispatcher.Invoke(() =>
           {
               ProgressLabel.Content = $"Currently finished: {numberOfFinishedInstances}/{numberOfInstancesTotal}";
           });
        }

        private void BrowseInputFile_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = InputFile.Text;
            dialog.IsFolderPicker = true;

            if(dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                InputFile.Text = dialog.FileName;
            }
        }

        private void BrowseReferenceFile_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = ReferenceFile.Text;

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                ReferenceFile.Text = dialog.FileName;
            }
        }

        private AnnealingOptions GetOptions(bool savePlotInfo)
        {
            
            int seed;
            if ((bool)RandomSeed.IsChecked)
            {
                var random = new Random();
                seed = random.Next();
            }
            else
                seed = 5;
                
            var annealingOptions = new AnnealingOptions
            {
                CoolingCoefficient = float.Parse(CoolingCoefficient.Text),
                CoolStrategy = new CoefficientCoolingStrategy(),
                EquilibriumStrategy = GetEquilibriumStrategy(),
                FrozenStrategy = GetFrozenStrategy(),
                ScoreStrategy = GetScoreStrategy(),
                StartingPositionStrategy = new RandomStartingPos(seed),
                BaseStartingTemperature = float.Parse(StartingTemperature.Text),
                MinimalTemperature = float.Parse(MinimalTemperature.Text),
                TryStrategy = GetTryStrategy(seed),
                BaseEquilibriumSteps = int.Parse(BaseEquilibriumSteps.Text),
                MaxRejectedRatio = float.Parse(MaxUnaccepted.Text),
                PenaltyMultiplier = float.Parse(PenaltyMultiplier.Text),
                SavePlotInfo = savePlotInfo
            };

            return annealingOptions;
        }

        private EquilibriumStrategy GetEquilibriumStrategy()
        {
            var selectedIndex = equilibriumStrat.SelectedIndex;
            switch (selectedIndex)
            {
                case 0:
                    return new ConstantEquilibriumStrategy();
                case 1:
                    return new MoveBasedEquilibriumStrategy();
                default:
                    return null;
            }
        }
        
        private TryStrategy GetTryStrategy(int seed)
        {
            var selectedIndex = TryStrategy.SelectedIndex;
            switch (selectedIndex)
            {
                case 0:
                    return new RandomTryStrategy(seed);
                case 1:
                    return new ImproveTryStrategy(seed, 
                        double.Parse(RandomNeighborProb.Text), 
                        double.Parse(RandomNewProb.Text), 
                        double.Parse(ImproveScoreProb.Text), 
                        double.Parse(ImproveSatisProb.Text));
                default:
                    return null;
            }
        }

        private IFrozenStrategy GetFrozenStrategy()
        {
            var selectedIndex = FrozenStrategy.SelectedIndex;
            switch (selectedIndex)
            {
                case 0:
                    return new ConstantFrozenStrategy();
                case 1:
                    return new MoveBasedFrozenStrategy();
                default:
                    return null;
            }
        }

        private IScoreStrategy GetScoreStrategy()
        {
            var selectedIndex = ScoreStrategy.SelectedIndex;
            switch (selectedIndex)
            {
                case 0:
                    return new SoftPenaltyScoreStrategy();
                case 1:
                    return new HardPenaltyScoreStrategy();
                default:
                    return null;
            }
        }

        private async void SolveAndPlotClick(object sender, RoutedEventArgs e)
        {
            var instances = GetInstances();

            if ((bool)RandomInstance.IsChecked)
            {
                Random random = new Random();
                instances = new List<SatInstance> { instances.ElementAt(random.Next(instances.Count)) };
            }
            else
            {
                instances = instances.Take(int.Parse(NumberOfInstances.Text)).ToList();
            }

            var results = await SolveInstances(instances, true);
            WriteDetailedResultInformation(results);
        }

        private void WriteDetailedResultInformation(IList<SatResult> results)
        {
            if((bool) AutoClear.IsChecked)
                AnnealingPlot.Model.Series.Clear();
            var solutionInfoBuilder = new StringBuilder();
            foreach (var result in results)
            {
                solutionInfoBuilder.AppendLine($"----INSTANCE NO.{result.SatInstance.Id}------");
                solutionInfoBuilder.AppendLine($"Epsilon: {result.Epsilon}");
                solutionInfoBuilder.AppendLine($"Time [ms]: {result.RunTimeMs}");
                solutionInfoBuilder.AppendLine($"Actual optimalization value: {result.Configuration.GetOptimalizationValue()}");
                solutionInfoBuilder.AppendLine($"Number of unsatisfied clauses: {result.Configuration.NumberOfUnsatisfiedClauses()}");
                solutionInfoBuilder.AppendLine($"Actual vector: {string.Join(",", result.Configuration.Valuations.Select(i => i ? 1 : 0))}");
                if (result.OptimalConfiguration != null)
                {
                    solutionInfoBuilder.AppendLine($"Optimal price: {result.OptimalConfiguration.OptimalizationValue}");
                    solutionInfoBuilder.AppendLine($"Solution vector: {string.Join(",", result.OptimalConfiguration.Valuations.Select(i => i ? 1 : 0))}");
                }
                var series = new LineSeries
                {
                    ItemsSource = result.MovesHistory
                };
                AnnealingPlot.Model.Series.Add(series);
            }

            AnnealingPlot.Model.DefaultXAxis.Title = "Number of steps";
            AnnealingPlot.Model.DefaultYAxis.Title = "Value of optimalization criterion";
            SolutionInfo.Text = solutionInfoBuilder.ToString();
            AnnealingPlot.InvalidatePlot();
        }

        private void BrowseOutputFile_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = InputFile.Text;
            dialog.IsFolderPicker = true;

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                OutputFolder.Text = dialog.FileName;
            }
        }

        private float GetEpsilonOfSolution(int resultPrice, int optimalPrice)
        {
            if (Math.Max(resultPrice, optimalPrice) == 0)
                return 0;
            else
                return (float)Math.Abs(resultPrice - optimalPrice) / Math.Max(resultPrice, optimalPrice);
        }

        private void AddEpsilonValues(IList<SatResult> results)
        {
            var referenceConfigurations = GetReferenceConfigurations();
            foreach (var result in results)
            {

                if (referenceConfigurations.TryGetValue(result.SatInstance.Id, out var optimalConfiguration))
                {
                    result.Epsilon = GetEpsilonOfSolution(result.Configuration.GetOptimalizationValue(), optimalConfiguration.OptimalizationValue);
                    result.OptimalConfiguration = optimalConfiguration;
                }
                else
                {
                    result.Epsilon = 0;
                    Debug.WriteLine($"Warning: no reference for id: {result.SatInstance.Id}");
                }
            }
        }

        private async Task<IList<SatResult>> SolveInstances(IList<SatInstance> instances, bool savePlotInfo)
        {
            var options = GetOptions(savePlotInfo);
            

            numberOfInstancesTotal = int.Parse(NumberOfInstances.Text);
            var instanceOffset = int.Parse(InstanceOffset.Text);
            numberOfFinishedInstances = 0;

            var results = await Task.Factory.StartNew(() =>
            {
                var performanceTester = new PerformanceTester();
                performanceTester.RaiseInstanceCalculationFinished += OnInstanceCalculationFinished;
                var res = performanceTester.SolveWithPerformanceTest(instances.Skip(instanceOffset).Take(numberOfInstancesTotal).ToList(), options);
                return res;
            }
            );

            foreach(var result in results)
            {
                result.ResultLabel = CustomRowLabel.Text;
            }

            if ((bool)ReferenceFileCheckbox.IsChecked)
                AddEpsilonValues(results);
            ProgressLabel.Content = "Calculations done!";
            return results;
        }

        private async void SolveAndOutput_Click(object sender, RoutedEventArgs e)
        {
            var instances = GetInstances().Take(int.Parse(NumberOfInstances.Text)).ToList();
            var results = await SolveInstances(instances, false);

            var solutionInfoBuilder = new StringBuilder();
            solutionInfoBuilder.AppendLine($"Min epsilon: {results.Select(r => r.Epsilon).Min()}");
            solutionInfoBuilder.AppendLine($"Avg epsilon: {results.Select(r => r.Epsilon).Average().ToString("F10")}");
            solutionInfoBuilder.AppendLine($"Max epsilon: {results.Select(r => r.Epsilon).Max()}");
            solutionInfoBuilder.AppendLine($"Min runtime: {results.Select(r => r.RunTimeMs).Min()}");
            solutionInfoBuilder.AppendLine($"Avg runtime: {results.Select(r => r.RunTimeMs).Average()}");
            solutionInfoBuilder.AppendLine($"Max runtime: {results.Select(r => r.RunTimeMs).Max()}");
            solutionInfoBuilder.AppendLine($"Number of unsatisfied: {results.Aggregate(0,(acc, res) => res.Configuration.IsSatisfiable() ? acc: acc+1)}");
            SolutionInfo.Text = solutionInfoBuilder.ToString();

            var outputLocation = System.IO.Path.Join(OutputFolder.Text, OutputFileName.Text);
            OutputWriter.WriteAllResults(results, outputLocation);
        }

        //Method used to lazy-load the instances
        private IList<SatInstance> GetInstances()
        {
            //Load a new list if not loaded or not up to date
            if(loadedInstanceLocation == null || loadedInstanceLocation != InputFile.Text)
            {
                loadedInstances = InputReader.ReadSatInstances(InputFile.Text).ToList();
                loadedInstanceLocation = InputFile.Text;
            }
            return loadedInstances;
        }

        private IDictionary<int, ReferenceConfiguration> GetReferenceConfigurations()
        {
            //Load a new list if not loaded or not up to date
            if (loadedReferenceConfigurationsLocation == null || loadedReferenceConfigurationsLocation != ReferenceFile.Text)
            {
                loadedReferenceConfigurations = InputReader.ReadOptimalConfigurations(ReferenceFile.Text);
                loadedReferenceConfigurationsLocation = ReferenceFile.Text;
            }
            return loadedReferenceConfigurations;
        }

        private void MyPlot_Initialized(object sender, EventArgs e)
        {

        }

        private void ReferenceFileCheckbox_Click(object sender, RoutedEventArgs e)
        {
            ReferenceFile.IsEnabled = (bool) ReferenceFileCheckbox.IsChecked;
            ReferenceFileBrowseButton.IsEnabled = (bool)ReferenceFileCheckbox.IsChecked;
        }
    }
}
