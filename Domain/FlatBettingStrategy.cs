using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain
{
    public class FlatBettingStrategy : IBettingStrategy
    {
        #region IBettingStrategy Members

        public Decimal MakeBet()
        {
            return 5.0m;
        }

        #endregion
    }
}
