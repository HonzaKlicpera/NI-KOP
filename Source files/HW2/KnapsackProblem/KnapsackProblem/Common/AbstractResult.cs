using System;
using System.Collections.Generic;
using System.Text;

namespace KnapsackProblem.Common
{
    public abstract class AbstractResult<I>
    {
        public ulong NumberOfSteps { get; set; }

        public string Strategy { get; set; }

        public string DataSetName { get; set; }

        public I KnapsackInstance { get; set; }
    }
}
