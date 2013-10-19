using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace Domain
{
    public class DealerDecisionMaker
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(DealerDecisionMaker));
        private readonly TableRules _rules;

        public DealerDecisionMaker(TableRules rules)
        {
            _rules = rules;
        }        

        public DecisionType DecidePlay(Player player, Card dealerFaceCard)
        {
            DecisionType decision;
            IList<Card> playerCards = player.Hand;

            StringBuilder stringBuilder = new StringBuilder();
            foreach (Card card in playerCards)
            {
                stringBuilder.AppendFormat("{0}, ", card);
            }

            if (_rules.DealerHitsSoft17)
            {
                
                if (player.Hand.Value <= 16 || (player.Hand.Value == 17 && player.Hand.HasSoftAce))
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
                if (player.Hand.Value <= 16)
                {
                    decision = DecisionType.Hit;
                }
                else
                {
                    decision = DecisionType.Stand;
                }
            }

            Log.DebugFormat("{0} has {1} Hand Value is {2}.  {3} decides to {4}", 
                player, 
                stringBuilder, 
                player.Hand.Value, 
                player, 
                decision);

            return decision;
        }
    }


    
}
