using System;
using System.Collections.Generic;
using System.Text;

namespace AnnealingWPF.Common
{
    public class ReferenceConfiguration
    {
        public int InstanceId { get; set; }
        public IList<bool> Valuations { get; set; } = new List<bool>();
        public int OptimalizationValue { get; set; }
    }
}
