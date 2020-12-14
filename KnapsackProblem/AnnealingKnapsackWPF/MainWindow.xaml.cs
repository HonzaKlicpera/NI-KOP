using KnapsackAnnealing.Common;
using KnapsackProblem.Helpers;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using KnapsackAnnealing.Solver.CoolingStrategies;
using KnapsackAnnealing.Solver.EquilibriumStrategies;
using KnapsackAnnealing.Solver.FrozenStrategies;
using KnapsackAnnealing.Solver.ScoreStrategies;
using KnapsackAnnealing.Solver.StartingPositionStrategies;
using KnapsackAnnealing.Solver.TryStrategies;
using KnapsackAnnealing.Solver;
using AnnealingKnapsackWPF.Solver.StartingPositionStrategies;

namespace AnnealingKnapsackWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "C:\\Users\\Johny\\source\\repos\\NI-KOP\\ReferenceData\\HW4\\NK\\NK20_inst.dat"; // Default file name
            //dlg.DefaultExt = ".txt"; // Default file extension
            //dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;
                InputFile.Text = filename;
            }
        }

        private AnnealingOptions GetOptions()
        {
            var seed = 5;
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
                MaxUnaccepted = int.Parse(MaxUnaccepted.Text),
                PenaltyMultiplier = float.Parse(PenaltyMultiplier.Text)
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
                    return new GreedyStartingPos();
                case 1:
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var instances = InputReader.ReadKnapsackInstances(InputFile.Text);

            var results = PerformanceTester.SolveWithPerformanceTest(instances.Take(int.Parse(NumberOfInstances.Text)).ToList(), GetOptions());

            MyPlot.Model.Series.Clear();
            var solutionInfoBuilder = new StringBuilder();
            foreach (var result in results)
            {
                solutionInfoBuilder.AppendLine($"----INSTANCE NO.{result.KnapsackInstance.Id}------");
                solutionInfoBuilder.AppendLine($"Epsilon: {result.Epsilon}");
                solutionInfoBuilder.AppendLine($"Time [ms]: {result.RunTimeMs}");
                var series = new LineSeries
                {
                    ItemsSource = result.MovesHistory
                };
                MyPlot.Model.Series.Add(series);
            }

            SolutionInfo.Text = solutionInfoBuilder.ToString();
            MyPlot.InvalidatePlot();

        }

        private void StartingTemperature_Copy1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void BaseEquilibriumSteps_Copy_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
