﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AnnealingWPF.Exceptions
{
    public class InvalidInputFormatException : Exception
    {
        public InvalidInputFormatException() { }
        public InvalidInputFormatException(string message) : base(message) { }
    }

}
