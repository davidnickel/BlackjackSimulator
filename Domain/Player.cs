using System;
using System.Collections.Generic;
using System.Linq;
using log4net;

namespace Domain
{
    public abstract class Player : IPlayer
    {
        private string _name;
        private HandOutcomeType _outcome;
        private readonly ILog Log = LogManager.GetLogger(typeof(Player));
        private IStatistics _statistics;
        private IBankRoll _bankRoll;
        private Decimal _currentBet;
        
        public Player(string name)
        {
            _name = name;
            Hands = new List<Hand>() {new Hand()};
            _statistics = new Statistics();
            _bankRoll = new BankRoll(1000.00m);
        }

        public abstract DecisionType MakeDecision(Card dealerCard);

        public virtual void PlaceBet()
        {
            _currentBet = GetBettingStrategy().MakeBet();
        }

        public Hand ActiveHand
        {
            get
            {
                return Hands.Last(hand => hand.Status == HandStatusType.Active);
            }

        }

        public abstract IBettingStrategy GetBettingStrategy();

      
        public void ReceiveCard(Card card)
        {
            if (ActiveHand != null)
            {
                ActiveHand.Add(card);
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
            Outcome = HandOutcomeType.Win;
            Statistics.Wins++;
        }

        public void LostRound()
        {
            Outcome = HandOutcomeType.Loss;
            Statistics.Losses++;
        }

        public void PushedRound()
        {
            Outcome = HandOutcomeType.Push;
            Statistics.Pushes++;
        }

        public void SurrenderedRound()
        {
            Outcome = HandOutcomeType.Surrender;
            Statistics.Losses++;
        }
        
        public override string ToString()
        {
            return Name;
        }

        public List<Hand> Hands { get; set; }

        public HandOutcomeType Outcome
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

      
        public IBankRoll BankRoll
        {
            get { return _bankRoll; }
            set { _bankRoll = value; }
        }
    }
}
