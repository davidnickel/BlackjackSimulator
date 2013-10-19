using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain
{
    public class MimicDealerPlayer : Player
    {
        internal readonly DealerDecisionMaker _decisionMaker;

        public MimicDealerPlayer()
            : this("MimicDealerPlayer")
        { }

        public MimicDealerPlayer(string name)
            : base(name)
        {
            _decisionMaker = new DealerDecisionMaker(TableRules.Instance);
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
