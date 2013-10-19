using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain
{
    public abstract class DecisionMaker
    {
        public abstract DecisionType DecidePlay(Player player, Card dealerFaceCard);
    }
}
