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
            Value = Convert.ToInt32(rank);
            if (Convert.ToInt32(Rank) >= 10)
            {
                Value = 10;
            }
            if (Rank == CardRank.Ace)
            {
                Value = 1;
            }
        }

        public CardRank Rank { get; set; }

        public CardSuit Suit { get; set; }

        public int Value { get; set; }          

        public override string ToString()
        {
            return String.Format("{0} of {1}", Rank, Suit);
        }
    }
}
