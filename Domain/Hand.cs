using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain
{
    public class Hand : List<Card>
    {
        public Hand() { }

        public int Value
        {
            get
            {
                int value = 0;
                foreach (Card card in this)
                {
                    value += card.TrueValue;
                }

                return value;
            }
        }
       
        public bool HasSoftAce
        {            
            get
            {
                foreach (Card card in this)
                {
                    if (card.Rank == CardRank.Ace && card.TrueValue == 11)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Convert the first instance of a soft ace to value of 1
        /// </summary>
        public void ConvertSoftAceToHard()
        {
            foreach (Card card in this)
            {
                if (card.Rank == CardRank.Ace && card.TrueValue == 11)
                {
                    card.TrueValue = 1;
                    return;
                }
            }
        }
    }
}
