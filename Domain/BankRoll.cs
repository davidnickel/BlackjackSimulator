using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Domain
{
    public class BankRoll : IBankRoll
    {
        private Decimal _value;

        public BankRoll(Decimal value)
        {
            Value = value;
        }

        #region IBankRoll Members

        public void PlaceBet(Decimal bet)
        {
            Decrease(bet);
        }

        public Decimal Value
        {
            get { return _value; }
            private set { _value = value; }
        }

        public void Add(Decimal winnings)
        {
            Value += winnings;
        }

        public void Decrease(Decimal value)
        {
            Value -= value;
        }

        public override string ToString()
        {
            return Value.ToString("C", new CultureInfo("en-US", false).NumberFormat);
        }

        #endregion
    }
}
