using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Reflection;
using Domain.Performance;

namespace Domain
{
    public class DealerPlayer : Player
    {
        private const int NUMBER_OF_CARDS = 2;
        private readonly DealerDecisionMaker _decisionMaker;
        private static readonly ILog Log = LogManager.GetLogger(typeof(DealerPlayer));
        private IPlayer _lastPlayerDealtTo;                


        public DealerPlayer() : base("DealerPlayer") 
        {
            _decisionMaker = new DealerDecisionMaker(TableRules.Instance);
        }

        public override DecisionType MakeDecision(Card dealerCard)
        {
            return _decisionMaker.DecidePlay(this, dealerCard);
        }

        public void ShuffleDeck()
        {
            Table.Instance.Deck.Shuffle();
        }

        public void PromptPlayersForBet()
        {
            foreach (IPlayer player in Table.Instance.Players)
            {
                player.PlaceBet();
            }
        }

        private void DealCardToPlayer(IPlayer player)
        {
            long start = DateTime.Now.Ticks;

            Card card = Table.Instance.Deck.Draw();

            Log.DebugFormat("Dealing {0} to {1}", card, player);

            player.ReceiveCard(card);            

            Table.Instance.CardsInPlay.Add(card);
            

            //BUG if there's 52 cards in play still, next move crashes
            //probably will never happen though
            if (Table.Instance.Deck.Cards.Count <= 0)
            {
                Log.DebugFormat("Deck out of cards, reclaiming discards...");
                foreach (Card discard in Table.Instance.DiscardedCards)
                {
                    Table.Instance.Deck.Cards.Add(discard);
                }

                Table.Instance.Deck.Shuffle();

                Table.Instance.DiscardedCards.Clear();

                //Log.DebugFormat("Cards in play still are:");
                //foreach (Card inPlayCard in Table.Instance.CardsInPlay)
                //{
                //    Log.DebugFormat("{0}", inPlayCard);
                //}
            }

            ExecutionTimeManager.RecordExecutionTime(String.Format("{0}.{1}()",
                    MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name), start);
        
        }

        public void Deal()
        {
            long start = DateTime.Now.Ticks;

            for (int i = 0; i < NUMBER_OF_CARDS; i++)
            {
                foreach (IPlayer player in Table.Instance.Players)
                {
                    DealCardToPlayer(player);
                }
            }

            ExecutionTimeManager.RecordExecutionTime(String.Format("{0}.{1}()",
                    MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name), start);
        }

        public Card UpCard
        {
            get
            {
                if (this.Hand.Count <= 0)
                {
                    return null;
                }

                return this.Hand[0];
            }
        }


        public DecisionType PromptPlayerForDecision(IPlayer player)
        {
            Log.DebugFormat("Prompting {0} for decision...", player);

            return player.MakeDecision(this.UpCard);
        }
             
        public void HandlePlayerDecision(IPlayer player, DecisionType decisionType)
        {
            long start = DateTime.Now.Ticks;

            switch (decisionType)
            {
                case DecisionType.Hit:
                    DealCardToPlayer(player);
                    break;
                case DecisionType.Stand:
                    break;
                case DecisionType.DoubleDown:
                    break;
                case DecisionType.Split:

                    int i = Table.Instance.Players.IndexOf(player);

                    Type t = player.GetType();

                    // Get constructor info. 
                    ConstructorInfo[] ci = t.GetConstructors();

                    
                    bool hasMatchingConstructor = false;
                    int x;
                    for (x = 0; x < ci.Length; x++)
                    {
                        ParameterInfo[] pi = ci[x].GetParameters();

                        if (pi.Length == 1 && pi[0].ParameterType == typeof(string))
                        {
                            hasMatchingConstructor = true;
                            break;
                        }
                    }

                    if (hasMatchingConstructor)
                    {
                        // Construct the object.   
                        object[] consargs = new object[1];
                        consargs[0] = player.Name + "-SplitHand" + player.SplitPlayers.Count.ToString();
                        IPlayer splitPlayer = (Player)ci[x].Invoke(consargs);
                        splitPlayer.IsSplitPlayer = true;

                        Card splitCard = player.Hand[0];                        

                        splitPlayer.ReceiveCard(splitCard);

                        Log.DebugFormat("Created {0}", splitPlayer);

                        player.RemoveCard(splitCard);

                        player.SplitPlayers.Add(splitPlayer);

                        Table.Instance.Players.Insert(i + 1, splitPlayer);
                    }
                    
                    break;
                case DecisionType.Surrender:
                    foreach (Card card in player.Hand)
                    {
                        Table.Instance.CardsInPlay.Remove(card);
                        Table.Instance.DiscardedCards.Add(card);
                    }

                    player.Hand.Clear();
                    player.Status = PlayerStatusType.Surrendered;

                    break;
                default:
                    throw new NotSupportedException();
            }

            ExecutionTimeManager.RecordExecutionTime(String.Format("{0}.{1}()",
                    MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name), start);
        }

        public void DetermineOutcome(IPlayer player)
        {
            foreach (IPlayer splitPlayer in player.SplitPlayers)
            {
                //Recursive call to add up split players statistics to base player
                DetermineOutcome(splitPlayer);
                player.Statistics.Wins += splitPlayer.Statistics.Wins;
                player.Statistics.Losses += splitPlayer.Statistics.Losses;
                player.Statistics.Pushes += splitPlayer.Statistics.Pushes;
                player.Statistics.BlackJacks += splitPlayer.Statistics.BlackJacks;
            }

            if (player.Status == PlayerStatusType.Busted)
            {
                player.LostRound();
                return;
            }

            if (player.Status == PlayerStatusType.Surrendered)
            {
                player.SurrenderedRound();
                return;
            }

            if (this.Status == PlayerStatusType.Busted)
            {
                player.WonRound();
                return;
            }

            if (player.Hand.Value == this.Hand.Value)
            {
                player.PushedRound();
            }

            if (player.Hand.Value > this.Hand.Value)
            {
                player.WonRound();
            }

            if (player.Hand.Value < this.Hand.Value)
            {
                player.LostRound();
            }

            return;
        }

        public void DeterminePayout(IPlayer player)
        {
            Decimal payout;

            switch (player.Outcome)
            {
                case PlayerOutcomeType.Surrender:
                    
                    payout = player.CurrentBet / 2;
                    this.BankRoll.Add(payout);
                    player.BankRoll.Decrease(payout);
                    break;

                case PlayerOutcomeType.Loss:
                    
                    payout = player.CurrentBet;
                    this.BankRoll.Add(payout);
                    player.BankRoll.Decrease(payout);
                    break;

                case PlayerOutcomeType.Win:

                    payout = player.CurrentBet;
                    if (player.HasBlackJack)
                    {
                        payout = player.CurrentBet * TableRules.Instance.BlackJackPayout;
                    }

                    this.BankRoll.Decrease(payout);
                    player.BankRoll.Add(payout);
                    break;
            }
        }

        public IPlayer LastPlayerDealtTo
        {
            get { return _lastPlayerDealtTo; }
            set { _lastPlayerDealtTo = value; }
        }

        public override IBettingStrategy GetBettingStrategy()
        {
            throw new NotImplementedException();
        }
    }
}
