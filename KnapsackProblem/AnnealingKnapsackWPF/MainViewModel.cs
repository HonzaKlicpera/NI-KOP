using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Generic;

using OxyPlot;

namespace AnnealingKnapsackWPF
{


    public class MainViewModel
    {
        public MainViewModel()
        {
            MyModel = new PlotModel { Title = "Test"};
        }

        public PlotModel MyModel { get; private set; }
    }
}
