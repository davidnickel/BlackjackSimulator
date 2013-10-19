using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain
{
    public interface IBankRoll
    {
        void PlaceBet(Decimal bet);
        Decimal Value { get; }
        void Add(Decimal value);
        void Decrease(Decimal value);
    }
}
