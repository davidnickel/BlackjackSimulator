using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace Domain
{
    public class CardCounterDecisionMaker
    {
        private readonly TableRules _rules;
        private readonly IBasicStrategy _strategy;
        private static readonly ILog Log = LogManager.GetLogger(typeof(CardCounterDecisionMaker));

        public CardCounterDecisionMaker(TableRules rules) : this(rules, BasicStrategyFactory.GetInstance(rules))
        {}

        internal CardCounterDecisionMaker(TableRules rules, IBasicStrategy strategy)
        {
            _rules = rules;
            _strategy = strategy;
        }

        public DecisionType DecidePlay(Player player, Card dealerFaceCard, int cardCount)
        {
            IList<Card> playerCards = player.Hand;

            StringBuilder stringBuilder = new StringBuilder();
            foreach (Card card in playerCards)
            {
                stringBuilder.AppendFormat("{0}, ", card);
            }            

            DecisionType basicStrategydecision = _strategy.GetDecision(player.Hand, dealerFaceCard);

            //For now these are equal - card counting is not implemented
            DecisionType cardCountdecision = basicStrategydecision;

            Log.DebugFormat("{0} has {1} Hand Value is {2}, Dealer has {3} Basic Strategy says {4}, Card Count is {5}, {6} decides to {7}", 
                player, 
                stringBuilder, 
                player.Hand.Value,
                dealerFaceCard,
                basicStrategydecision,
                cardCount,                 
                player,
                cardCountdecision);

            return cardCountdecision;
        }
    }
}
