using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Collections;
using Domain.Performance;
using System.Reflection;

namespace Domain
{
    public abstract class Player : IPlayer
    {
        private Hand _hand;
        private string _name;
        private PlayerOutcomeType _outcome;
        private PlayerStatusType _status;
        private readonly ILog Log = LogManager.GetLogger(typeof(Player));
        private IStatistics _statistics;
        private IList<IPlayer> _splitPlayers;
        private bool _isSplitPlayer;
        private IBankRoll _bankRoll;
        private Decimal _currentBet;

        private static readonly int SoftAceValue = 11;
        private static readonly int HandValueLimit = 21; 

        public Player(string name)
        {
            _name = name;
            _hand = new Hand();        
            _status = PlayerStatusType.Active;
            _statistics = new Statistics();
            _splitPlayers = new List<IPlayer>();
            _isSplitPlayer = false;
            _bankRoll = new BankRoll(1000.00m);
        }

        public abstract DecisionType MakeDecision(Card dealerCard);

        public virtual void PlaceBet()
        {
            _currentBet = GetBettingStrategy().MakeBet();
        }

        public abstract IBettingStrategy GetBettingStrategy();

        public IList<IPlayer> SplitPlayers
        {
            get { return _splitPlayers; }
        }
        
        public void ReceiveCard(Card card)
        {
            long start = DateTime.Now.Ticks;

            try
            {
                Hand.Add(card);

                if (Hand.Count == 1 && card.Rank == CardRank.Ace)
                {
                    card.TrueValue = SoftAceValue;
                }

                if (HasBlackJack)
                {
                    Status = PlayerStatusType.BlackJack;
                    Log.InfoFormat("{0} has been dealt a blackjack!", this);
                    Statistics.BlackJacks++;
                    return;
                }

                while (Hand.Value > HandValueLimit && Hand.HasSoftAce)
                {
                    Hand.ConvertSoftAceToHard();
                }

                if (Hand.Value > HandValueLimit)
                {
                    Status = PlayerStatusType.Busted;
                    Log.InfoFormat("{0} has busted.", this);
                }
            }
            finally
            {
                ExecutionTimeManager.RecordExecutionTime(String.Format("{0}.{1}()",
                    MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name), start);
            }
        }

        public virtual void PlaceBet(Decimal bet)
        {

            _currentBet = bet;
        }

        public Decimal CurrentBet
        {
            get { return _currentBet; }
            set { _currentBet = value; }
        }

        public void WonRound()
        {
            Outcome = PlayerOutcomeType.Win;
            Statistics.Wins++;
        }

        public void LostRound()
        {
            Outcome = PlayerOutcomeType.Loss;
            Statistics.Losses++;
        }

        public void PushedRound()
        {
            Outcome = PlayerOutcomeType.Push;
            Statistics.Pushes++;
        }

        public void SurrenderedRound()
        {
            Outcome = PlayerOutcomeType.Surrender;
            Statistics.Losses++;
        }


        public void FlushHand()
        {
            Hand.Clear();
        }

        public void RemoveCard(Card card)
        {
            Hand.Remove(card);
        }

        public override string ToString()
        {
            return Name;
        }

        public Hand Hand
        {
            get { return _hand; }
        }

        public bool HasBlackJack
        {
            get { return Hand.Value == 21 && Hand.Count == 2; }
        }              

        public PlayerStatusType Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public PlayerOutcomeType Outcome
        {
            get { return _outcome; }
            set { _outcome = value; }
        }

        public IStatistics Statistics
        {
            get { return _statistics; }
            set { _statistics = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public bool IsSplitPlayer
        {
            get { return _isSplitPlayer; }
            set { _isSplitPlayer = value; }
        }

        public IBankRoll BankRoll
        {
            get { return _bankRoll; }
            set { _bankRoll = value; }
        }
    }
}
