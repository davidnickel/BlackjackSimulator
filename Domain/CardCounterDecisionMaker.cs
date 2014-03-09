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


            DecisionType basicStrategydecision = _strategy.GetDecision(player.ActiveHand, dealerFaceCard);

            //For now these are equal - card counting is not implemented
            DecisionType cardCountdecision = basicStrategydecision;


            return cardCountdecision;
        }
    }
}
