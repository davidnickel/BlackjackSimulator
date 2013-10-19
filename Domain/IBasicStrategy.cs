using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain
{
    public interface IBasicStrategy
    {
        DecisionType GetDecision(Hand playerHand, Card dealerUpCard);
    }
}
