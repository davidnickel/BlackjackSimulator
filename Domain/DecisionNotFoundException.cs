using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain
{
    public class DecisionNotFoundException : Exception
    {
        public DecisionNotFoundException() : this("Decision Not Found.") { }

        public DecisionNotFoundException(string message) : base(message) { }


    }
}
