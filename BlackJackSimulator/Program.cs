using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Domain;
using log4net;
using log4net.Config;
using System.Reflection;


namespace BlackJackSimulator
{
    public class Program
    {
        static ILog Log = LogManager.GetLogger(typeof(Program));

        public static void Main(string[] args)
        {
           // Stopwatch watch = Stopwatch.StartNew();

            XmlConfigurator.Configure();
           

            TableRules.Instance.NumberOfDecks = 4;
            TableRules.Instance.BlackJackPayout = 3 / 2m;
            TableRules.Instance.DealerHitsSoft17 = true;

            DealerPlayer dealer = new DealerPlayer();

            Table.Instance.AddPlayer(new BasicStrategyPlayer());
            //Table.Instance.AddPlayer(new CardCounterPlayer());
            //Table.Instance.AddPlayer(new MimicDealerPlayer());
            Table.Instance.AddPlayer(dealer);

            dealer.ShuffleDeck();

            for (int i = 0; i < 1000000; i++)
            {
                decimal discard = Table.Instance.DiscardedCards.Count;
                decimal deck = Table.Instance.Deck.Cards.Count;
                decimal totalCards = discard + deck;

                if ((discard/totalCards) >= .7m)
                {
                    //todo: if discard list is not used for stats at some point it can be removed
                    // put a 70% penetration flag on the deck and just rebuild a new deck instance and shuffle it
                    
                    Table.Instance.Deck.Cards.AddRange(Table.Instance.DiscardedCards);
                    dealer.ShuffleDeck();
                    Table.Instance.DiscardedCards.Clear();
                }

                foreach (IPlayer player in Table.Instance.Players)
                {
                    player.Hands.Clear();
                    player.Hands.Add(new Hand()); // todo: remove all but first, then reset defaults on first
                }

                dealer.Deal();

                //LogPlayerInfo();

                if (dealer.ActiveHand.HasBlackJack)
                {
                    foreach (IPlayer player in Table.Instance.Players)
                    {
                        foreach (Hand hand in player.Hands)
                        {
                            hand.Status = HandStatusType.Completed;

                            if (player != dealer)
                            {
                                dealer.DetermineOutcome(player, hand);
                                //Log.InfoFormat("{0}'s outcome is {1}", player, player.Outcome);
                            }

                            Table.Instance.DiscardedCards.AddRange(hand);
                            hand.Clear();
                        }
                    }
                    continue;
                }

                foreach (IPlayer player in Table.Instance.Players)
                {
                    while (player.ActiveHand != null && player.ActiveHand.Status == HandStatusType.Active)
                    {
                        var playerDecision = dealer.PromptPlayerForDecision(player);
                        dealer.HandlePlayerDecision(player, playerDecision);

                        //LogPlayerInfo();
                    }
                }

                foreach (IPlayer player in Table.Instance.Players)
                {
                    if (player != dealer)
                    {
                        foreach (Hand hand in player.Hands)
                        {
                            dealer.DetermineOutcome(player, hand);
                            dealer.DeterminePayout(player, hand);

                            // it shouldn't matter that discards are not collected at the actual time of a bust etc
                            // since the shuffle is theorectically random, the discard pile ordering should not come into play
                            Table.Instance.DiscardedCards.AddRange(hand);
                            hand.Clear();
                        }
                    }
                    else
                    {
                        Table.Instance.DiscardedCards.AddRange(player.ActiveHand);
                        player.ActiveHand.Clear();
                    }
                }
            }

         //   watch.Stop();
//Console.WriteLine("total: " + watch.ElapsedMilliseconds);

            //Console.ReadLine();
        }

        private static void LogPlayerInfo()
        {
            foreach (IPlayer player in Table.Instance.Players)
            {
                Log.InfoFormat("--{0}--", player);
                for (int p = 0; p < player.Hands.Count; p++)
                {
                    Log.InfoFormat("Hand{0}:\n{1}", p, player.Hands[p]);
                }
            }
        }
    }
}
         