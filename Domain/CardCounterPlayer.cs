using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain
{
    public class CardCounterPlayer : Player
    {
        private CardCounterDecisionMaker _decisionMaker; 
        private static int _cardCount;

        public CardCounterPlayer()
            : this("CardCounterPlayer")
        { }

        public CardCounterPlayer(string name)
            : base(name)
        {
            _decisionMaker = new CardCounterDecisionMaker(TableRules.Instance);
            _cardCount = 0;
        }


        public override DecisionType MakeDecision(Card dealerCard)
        {
            int cardCount = DetermineCardCount();
            return _decisionMaker.DecidePlay(this, dealerCard, cardCount);
        }

        private int DetermineCardCount()
        {
            //REMOVE
            //This is also wrong
            //dealt cards may be empty - we need to also look at cards out on table
            //because when the deck becomes empty and gets shuffled, dealt cards
            //gets cleared but the cards out on the table should still be there
            //and the count should get recalculated based on that

            //use discarded cards and cards in play
            foreach (Card card in Table.Instance.DiscardedCards)
            {
                if (card.Rank == CardRank.Ace || card.Value == 10)
                {
                    _cardCount--;
                }
                else
                {
                    _cardCount++;
                }
            }

            return 0;
        }


        public override IBettingStrategy GetBettingStrategy()
        {
            throw new NotImplementedException();
        }
    }
}
