using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain
{
    public class BasicStrategyPlayer : Player
    {
        private readonly BasicStrategyDecisionMaker _decisionMaker;

        public BasicStrategyPlayer()
            : this("BasicStrategyPlayer")
        { }

        public BasicStrategyPlayer(string name)
            : base(name)
        {
            _decisionMaker = new BasicStrategyDecisionMaker(TableRules.Instance);
        }


        public override DecisionType MakeDecision(Card dealerCard)
        {
            return _decisionMaker.DecidePlay(this, dealerCard);
        }

        public override IBettingStrategy GetBettingStrategy()
        {
            return new FlatBettingStrategy();
        }
    }
}
