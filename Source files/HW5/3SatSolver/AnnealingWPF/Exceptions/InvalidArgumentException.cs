using System;
using System.Collections.Generic;
using System.Text;

namespace AnnealingWPF.Exceptions
{
    public class InvalidArgumentException : Exception
    {
        public InvalidArgumentException() : base() { }
        public InvalidArgumentException(string msg) : base(msg) { }
    }
}
