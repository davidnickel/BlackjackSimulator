using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace Domain
{
    public class BasicStrategyDecisionMaker
    {
        private readonly TableRules _rules;
        private readonly IBasicStrategy _strategy;
        private static readonly ILog Log = LogManager.GetLogger(typeof(BasicStrategyDecisionMaker));

        public BasicStrategyDecisionMaker(TableRules rules) : this(rules, BasicStrategyFactory.GetInstance(rules))
        {}        

        internal BasicStrategyDecisionMaker(TableRules rules, IBasicStrategy strategy)
        {
            _rules = rules;
            _strategy = strategy;
        }

        public DecisionType DecidePlay(Player player, Card dealerFaceCard)
        {
            IList<Card> playerCards = player.Hand;

            StringBuilder stringBuilder = new StringBuilder();

            foreach (Card card in playerCards)
            {
                stringBuilder.AppendFormat("{0}, ", card);
            }            

            DecisionType decision = _strategy.GetDecision(player.Hand, dealerFaceCard);

            Log.DebugFormat("{0} has {1} Hand Value is {2}.  Dealer has {3}. {4} decides to {5}", 
                player, 
                stringBuilder, 
                player.Hand.Value, 
                dealerFaceCard, 
                player,
                decision);

            return decision;
        }
    }
}
