using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Performance
{
    /// <summary>
    /// Represents a delegate that knows about execution time
    /// </summary>
    public delegate void ExecutionTimeDelegate(object sender, ExecutionTimeEventArgs args);
}
