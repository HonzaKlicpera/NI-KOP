using System;
using System.Collections.Generic;
using System.Text;

namespace KnapsackProblem.Exceptions
{
    public class InvalidArgumentException : Exception
    {
        public InvalidArgumentException() : base() { }
        public InvalidArgumentException(string msg) : base(msg) { }
    }
}
