using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain
{
    public interface ICard
    {
        CardRank Rank { get; set; }
        CardSuit Suit { get; set; }
        int TrueValue { get; set; }
    }
}
