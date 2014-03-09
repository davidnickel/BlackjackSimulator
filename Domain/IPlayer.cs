using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain
{
    public interface IPlayer
    {
        DecisionType MakeDecision(Card dealerCard);
        void ReceiveCard(Card card);
        List<Hand> Hands { get; }
        HandOutcomeType Outcome { get; set; }
        IStatistics Statistics { get; set; }
        string Name { get; set; }
        IBankRoll BankRoll { get; set; }
        void PlaceBet(Decimal bet);
        void WonRound();
        void LostRound();
        void PushedRound();
        void SurrenderedRound();
        Decimal CurrentBet { get; set; }
        IBettingStrategy GetBettingStrategy();
        void PlaceBet();

        Hand ActiveHand
        {
            get;
        }
    }
}
