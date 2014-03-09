using System;
using System.Linq;
using System.Reflection;
using Domain.Performance;
using log4net;

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
            Card card = Table.Instance.Deck.Draw();

            Log.DebugFormat("Dealing {0} to {1}", card, player);

            player.ReceiveCard(card);            

            Table.Instance.CardsInPlay.Add(card);
            

            //BUG if there's 52 cards in play still, next move crashes
            //todo: implement correct shuffle logic, they don't deal out the deck to zero cards
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
        }

        public void Deal()
        {
            for (int i = 0; i < NUMBER_OF_CARDS; i++)
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
            Log.DebugFormat("Prompting {0} for decision...", player);

            return player.MakeDecision(this.UpCard);
        }
             
        public void HandlePlayerDecision(IPlayer player, DecisionType decisionType)
        {
            switch (decisionType)
            {
                case DecisionType.Hit:
                    DealCardToPlayer(player);
                    if (player.ActiveHand.Value > 21)
                    {
                        player.ActiveHand.Status = HandStatusType.Completed;
                        //todo: clear cards into discard ?
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
                    activeHand.Remove(splitCard);

                    var splitHand = new Hand() {IsSplitHand = true, Status = HandStatusType.Active };
                    splitHand.Add(splitCard);
                    
                    player.Hands.Add(splitHand);

                    //todo: handle split after aces, only 1 card... etc other rules

                    break;
                case DecisionType.Surrender:
                    foreach (Card card in player.ActiveHand)
                    {
                        Table.Instance.CardsInPlay.Remove(card);
                        Table.Instance.DiscardedCards.Add(card);
                    }

                    player.ActiveHand.Clear();
                    player.ActiveHand.Status = HandStatusType.Surrendered;

                    //todo: handle bet reconciliation

                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        public void DetermineOutcome(IPlayer player, Hand hand)
        {
            if (hand.Value > 21)
            {
                hand.Outcome = HandOutcomeType.Loss;
                return;
            }

            if (hand.Status == HandStatusType.Surrendered)
            {
                hand.Outcome = HandOutcomeType.Surrender;
                return;
            }
            
            if (this.ActiveHand.Value > 21)
            {
                hand.Outcome = HandOutcomeType.Win;
                return;
            }

            if (hand.Value == this.ActiveHand.Value)
            {
                hand.Outcome = HandOutcomeType.Push;
                return;
            }

            if (hand.Value > this.ActiveHand.Value)
            {
                hand.Outcome = HandOutcomeType.Win;
                return;
            }

            if (hand.Value < this.ActiveHand.Value)
            {
                hand.Outcome = HandOutcomeType.Loss;
                return;
            }

            return;
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
