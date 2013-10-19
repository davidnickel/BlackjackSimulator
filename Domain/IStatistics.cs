using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain
{
    public interface IStatistics
    {
        int Wins { get; set; }
        int Losses { get; set; }
        int Pushes { get; set; }
        int BlackJacks { get; set; }
    }
}
