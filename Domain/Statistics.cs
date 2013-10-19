using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain
{
    public class Statistics : IStatistics
    {
        private int _wins;
        private int _losses;
        private int _pushes;
        private int _blackJacks;

        #region IStatistics Members

        public int Wins
        {
            get
            {
                return _wins;
            }
            set
            {
                _wins = value;
            }
        }

        public int Losses
        {
            get
            {
                return _losses;
            }
            set
            {
                _losses = value;
            }
        }

        public int Pushes
        {
            get
            {
                return _pushes;
            }
            set
            {
                _pushes = value;
            }
        }

        public int BlackJacks
        {
            get
            {
                return _blackJacks;
            }
            set
            {
                _blackJacks = value;
            }
        }

        public override string ToString()
        {
            return String.Format("Wins: {0} Losses: {1} Pushes: {2} Blackjacks: {3}",
                Wins,
                Losses,
                Pushes,
                BlackJacks);
        }

        #endregion
    }
}
