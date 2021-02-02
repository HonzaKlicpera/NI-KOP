using System;
using System.Collections.Generic;
using System.Text;

using OxyPlot;

namespace AnnealingKnapsackWPF
{


    public class MainViewModel
    {
        public MainViewModel()
        {
            MyModel = new PlotModel();
        }

        public PlotModel MyModel { get; private set; }
    }
}
