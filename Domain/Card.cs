using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain
{
    public class Card : ICard
    {
        public Card(CardRank rank, CardSuit suit)
        {   
            Rank = rank;
            Suit = suit;
            TrueValue = Convert.ToInt32(rank);
            if (Convert.ToInt32(Rank) >= 10)
            {
                TrueValue = 10;
            }
            if (Rank == CardRank.Ace)
            {
                TrueValue = 11;
            }
        }

        public CardRank Rank { get; set; }

        public CardSuit Suit { get; set; }

        public int TrueValue { get; set; }          

        public override string ToString()
        {
            return String.Format("{0} of {1}", Rank, Suit);
        }
    }
}
