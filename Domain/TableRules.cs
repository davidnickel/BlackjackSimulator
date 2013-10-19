using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain
{
    public class TableRules
    {
        private int _numberOfDecks;
        private bool _dealerHitsSoft17;
        private decimal _blackJackPayout;

        public static TableRules Instance
        {
            get { return Singleton<TableRules>.GetInstance(delegate { return new TableRules(); }); }
        }

        private TableRules()
        {
            NumberOfDecks = 4;
            DealerHitsSoft17 = true;
            BlackJackPayout = 2.5m;
        }

        public  int NumberOfDecks
        {
            get { return _numberOfDecks; }
            set { _numberOfDecks = value; }
        }

        public bool DealerHitsSoft17
        {
            get { return _dealerHitsSoft17; }
            set { _dealerHitsSoft17 = value; }
        }

        public Decimal BlackJackPayout
        {
            get { return _blackJackPayout; }
            set { _blackJackPayout = value; }
        }
    }
}
