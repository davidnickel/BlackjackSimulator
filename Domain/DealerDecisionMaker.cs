using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace Domain
{
    public class DealerDecisionMaker
    {
        private readonly TableRules _rules;

        public DealerDecisionMaker(TableRules rules)
        {
            _rules = rules;
        }        

        public DecisionType DecidePlay(Player player, Card dealerFaceCard)
        {
            DecisionType decision;
           
            if (_rules.DealerHitsSoft17)
            {
                if (player.ActiveHand.Value <= 16 || (player.ActiveHand.Value == 17 && player.ActiveHand.ContainsSoftAce)) 
                {
                    decision = DecisionType.Hit;
                }
                else
                {
                    decision = DecisionType.Stand;
                }
            }
            else
            {
                if (player.ActiveHand.Value <= 16)
                {
                    decision = DecisionType.Hit;
                }
                else
                {
                    decision = DecisionType.Stand;
                }
            }
            return decision;
        }
    }
}
