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

            DecisionType decision = _strategy.GetDecision(player.ActiveHand, dealerFaceCard);

          
            return decision;
        }
    }
}
