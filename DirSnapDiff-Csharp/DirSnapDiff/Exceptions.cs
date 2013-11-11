using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ICrew.DirSnapDiff
{
    sealed class InvalidParameterExcetion : Exception
    {
        public InvalidParameterExcetion(string message)
            : base(message)
        {
        }
    }
}
