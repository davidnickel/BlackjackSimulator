using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain
{
    public interface IPlayer
    {
        DecisionType MakeDecision(Card dealerCard);
        IList<IPlayer> SplitPlayers { get; }
        void ReceiveCard(Card card);
        void FlushHand();
        void RemoveCard(Card card);
        Hand Hand { get; }
        bool HasBlackJack { get; }
        PlayerStatusType Status { get; set; }
        PlayerOutcomeType Outcome { get; set; }
        IStatistics Statistics { get; set; }
        string Name { get; set; }
        bool IsSplitPlayer { get; set; }
        IBankRoll BankRoll { get; set; }
        void PlaceBet(Decimal bet);
        void WonRound();
        void LostRound();
        void PushedRound();
        void SurrenderedRound();
        Decimal CurrentBet { get; set; }
        IBettingStrategy GetBettingStrategy();
        void PlaceBet();
    }
}
