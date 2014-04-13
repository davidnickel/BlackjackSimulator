using System;
using System.Linq;
using System.Reflection;

using log4net;

namespace Domain
{
    public class DealerPlayer : Player
    {
        private const int NumberOfCards = 2;
        private readonly DealerDecisionMaker _decisionMaker;
        private static readonly ILog Log = LogManager.GetLogger(typeof(DealerPlayer));
        
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

        public void DealCardToPlayer(IPlayer player)
        {
            Card card = Table.Instance.Deck.Draw();
            if (card == null)
            {
                throw new ArgumentNullException("Drew a null card from the deck, cannot continue.");
            }

            player.ReceiveCard(card);            
        }

        public void Deal()
        {
            for (int i = 0; i < NumberOfCards; i++)
            {
                foreach (IPlayer player in Table.Instance.Players)
                {
                    DealCardToPlayer(player);
                }
            }
        }

        public Card UpCard
        {
            get
            {
                if (this.ActiveHand.Count <= 0)
                {
                    return null;
                }

                return this.ActiveHand[0];
            }
        }


        public DecisionType PromptPlayerForDecision(IPlayer player)
        {
           // Log.DebugFormat("Prompting {0} for decision...", player);

            return player.MakeDecision(this.UpCard);
        }
             
        public void HandlePlayerDecision(IPlayer player, DecisionType decisionType)
        {

            if (ActiveHand.IsSplitHand && ActiveHand.Count >= 2)
            {
                if (player.ActiveHand.ParentHand.Count > 1)
                {
                    throw new NotSupportedException("more than 1 card in parent hand of split child hand.");
                }

                if (player.ActiveHand.ParentHand[0].Rank == CardRank.Ace)
                {
                    if (decisionType != DecisionType.Split)
                    {
                        decisionType = DecisionType.Stand;
                    }
                }
            }

            switch (decisionType)
            {
                case DecisionType.Hit:
                    DealCardToPlayer(player);
                    if (player.ActiveHand.Value > 21)
                    {
                        //Table.Instance.DiscardedCards.AddRange(player.ActiveHand);
                        //player.ActiveHand.Clear(); // cards are still needed here for outcome determination at end otherwise value is 0
                        player.ActiveHand.Status = HandStatusType.Completed;
                    }
                    break;
                case DecisionType.Stand:
                    player.ActiveHand.Status = HandStatusType.Completed;
                    break;
                case DecisionType.DoubleDown:
                    DealCardToPlayer(player);
                    player.ActiveHand.Status = HandStatusType.Completed;
                    break;
                case DecisionType.Split:

                    var activeHand = player.ActiveHand;
                    var splitCard = activeHand.Last();
                    activeHand.Remove(splitCard);  //todo: don't use Remove(), just remove the last index no ?

                    var splitHand = new Hand() {ParentHand = activeHand};
                    splitHand.Add(splitCard);
                    
                    player.Hands.Add(splitHand);
                    
                    break;
                case DecisionType.Surrender:

                    player.ActiveHand.Status = HandStatusType.Surrendered;

                    //todo: handle bet reconciliation

                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        public void DetermineOutcome(IPlayer player, Hand playerHand)
        {
            if (playerHand.Value > 21)
            {
                playerHand.Outcome = HandOutcomeType.Loss;
                return;
            }

            if (playerHand.Status == HandStatusType.Surrendered)
            {
                playerHand.Outcome = HandOutcomeType.Surrender;
                return;
            }
            
            if (this.ActiveHand.Value > 21)
            {
                playerHand.Outcome = HandOutcomeType.Win;
                return;
            }

            if (playerHand.Value == this.ActiveHand.Value)
            {
                playerHand.Outcome = HandOutcomeType.Push;
                return;
            }

            if (playerHand.Value > this.ActiveHand.Value)
            {
                playerHand.Outcome = HandOutcomeType.Win;
                return;
            }

            if (playerHand.Value < this.ActiveHand.Value)
            {
                playerHand.Outcome = HandOutcomeType.Loss;
                return;
            }

            return;
        }

        public override Hand ActiveHand
        {
            get
            {
                if (Hands.Any())
                {
                    return Hands.First();
                }
                return null;
            }
        }

        public void DeterminePayout(IPlayer player, Hand hand)
        {
            //Decimal payout;

            //switch (player.Outcome)
            //{
            //    case PlayerOutcomeType.Surrender:
                    
            //        payout = player.CurrentBet / 2;
            //        this.BankRoll.Add(payout);
            //        player.BankRoll.Decrease(payout);
            //        break;

            //    case PlayerOutcomeType.Loss:
                    
            //        payout = player.CurrentBet;
            //        this.BankRoll.Add(payout);
            //        player.BankRoll.Decrease(payout);
            //        break;

            //    case PlayerOutcomeType.Win:

            //        payout = player.CurrentBet;
            //        if (player.Hand.Status == HandStatusType.BlackJack)
            //        {
            //            payout = player.CurrentBet * TableRules.Instance.BlackJackPayout;
            //        }

            //        this.BankRoll.Decrease(payout);
            //        player.BankRoll.Add(payout);
            //        break;
            //}
        }

        public override IBettingStrategy GetBettingStrategy()
        {
            throw new NotImplementedException();
        }
    }
}
