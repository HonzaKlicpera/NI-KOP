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

namespace AnnealingKnapsackWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private int numberOfFinishedInstances;
        private int numberOfInstancesTotal;

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

        private AnnealingOptions GetOptions()
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
                ScoreStrategy = new LinearScoreStrategy(),
                StartingPositionStrategy = GetStartingPositionStrategy(seed),
                BaseStartingTemperature = float.Parse(StartingTemperature.Text),
                MinimalTemperature = float.Parse(MinimalTemperature.Text),
                TryStrategy = new RandomTryStrategy(seed),
                BaseEquilibriumSteps = int.Parse(BaseEquilibriumSteps.Text),
                MaxRejectedRatio = float.Parse(MaxUnaccepted.Text),
                PenaltyMultiplier = float.Parse(PenaltyMultiplier.Text),
                SavePlotInfo = true
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
        
        private IStartingPositionStrategy GetStartingPositionStrategy(int seed)
        {
            var selectedIndex = StartingPosition.SelectedIndex;
            switch (selectedIndex)
            {
                case 0:
                    return new RandomStartingPos(seed);
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

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
            var instances = InputReader.ReadSatInstances(InputFile.Text);
            IList<SatInstance> instancesToSolve;
            if ((bool)RandomInstance.IsChecked)
            {
                Random random = new Random();
                instancesToSolve = new List<SatInstance>();
                instancesToSolve.Add(instances.ElementAt(random.Next(instances.Count)));
            }
            else
            {
                instancesToSolve = instances;
            }

            var results = await SolveInstances(instancesToSolve, true);

            MyPlot.Model.Series.Clear();
            var solutionInfoBuilder = new StringBuilder();
            foreach (var result in results)
            {
                solutionInfoBuilder.AppendLine($"----INSTANCE NO.{result.SatInstance.Id}------");
                solutionInfoBuilder.AppendLine($"Epsilon: {result.Epsilon}");
                solutionInfoBuilder.AppendLine($"Time [ms]: {result.RunTimeMs}");
                solutionInfoBuilder.AppendLine($"Actual optimalization value: {result.Configuration.GetOptimalizationValue()}");
                solutionInfoBuilder.AppendLine($"Optimal price: {result.OptimalConfiguration.GetOptimalizationValue()}");
                solutionInfoBuilder.AppendLine($"Actual vector: {string.Join(",", result.Configuration.Valuations.Select(i => i ? 1 : 0))}");
                solutionInfoBuilder.AppendLine($"Solution vector: {string.Join(",", result.OptimalConfiguration.Valuations.Select(i => i ? 1 : 0))}");
                var series = new LineSeries
                {
                    ItemsSource = result.MovesHistory
                };
                MyPlot.Model.Series.Add(series);
            }

            MyPlot.Model.DefaultXAxis.Title = "Number of steps";
            MyPlot.Model.DefaultYAxis.Title = "Value of optimalization criterion";
            SolutionInfo.Text = solutionInfoBuilder.ToString();
            MyPlot.InvalidatePlot();

        }

        private void BrowseOutputFile_Click(object sender, RoutedEventArgs e)
        {
        }

        private async Task<IList<SatResult>> SolveInstances(IList<SatInstance> instances, bool savePlotInfo)
        {
            var options = GetOptions();
            options.SavePlotInfo = savePlotInfo;

            numberOfInstancesTotal = int.Parse(NumberOfInstances.Text);
            var instanceOffset = int.Parse(InstanceOffset.Text);
            numberOfFinishedInstances = 0;

            var results = await Task.Factory.StartNew(() =>
            {
                var performanceTester = new PerformanceTester();
                performanceTester.RaiseInstanceCalculationFinished += OnInstanceCalculationFinished;
                return performanceTester.SolveWithPerformanceTest(instances.Skip(instanceOffset).Take(numberOfInstancesTotal).ToList(), options);
            }
            );
            ProgressLabel.Content = "Calculations done!";

            return results;
        }

        private async void SolveAndOutput_Click(object sender, RoutedEventArgs e)
        {
            var instances = InputReader.ReadSatInstances(InputFile.Text).Take(int.Parse(NumberOfInstances.Text)).ToList();
            var results = await SolveInstances(instances, false);

            var solutionInfoBuilder = new StringBuilder();
            solutionInfoBuilder.AppendLine($"Min epsilon: {results.Select(r => r.Epsilon).Min()}");
            solutionInfoBuilder.AppendLine($"Avg epsilon: {results.Select(r => r.Epsilon).Average()}");
            solutionInfoBuilder.AppendLine($"Max epsilon: {results.Select(r => r.Epsilon).Max()}");
            solutionInfoBuilder.AppendLine($"Min runtime: {results.Select(r => r.RunTimeMs).Min()}");
            solutionInfoBuilder.AppendLine($"Avg runtime: {results.Select(r => r.RunTimeMs).Average()}");
            solutionInfoBuilder.AppendLine($"Max runtime: {results.Select(r => r.RunTimeMs).Max()}");
            SolutionInfo.Text = solutionInfoBuilder.ToString();

            var outputLocation = System.IO.Path.Join(OutputFolder.Text, OutputFileName.Text);
            //OutputWriter.WriteAllResults(results, outputLocation);
        }

        private void MyPlot_Initialized(object sender, EventArgs e)
        {

        }

    }
}
