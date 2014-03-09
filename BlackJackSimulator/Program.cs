using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain;
using log4net;
using log4net.Config;
using System.Reflection;
using Domain.Performance;

namespace BlackJackSimulator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            long start = DateTime.Now.Ticks;


            XmlConfigurator.Configure();
            ILog Log = LogManager.GetLogger(typeof(Program));

            TableRules.Instance.NumberOfDecks = 4;
            TableRules.Instance.BlackJackPayout = 3 / 2m;
            TableRules.Instance.DealerHitsSoft17 = true;

            DealerPlayer dealer = new DealerPlayer();

            Table.Instance.AddPlayer(new BasicStrategyPlayer());
            Table.Instance.AddPlayer(new CardCounterPlayer());
            Table.Instance.AddPlayer(new MimicDealerPlayer());
            Table.Instance.AddPlayer(dealer);

            dealer.ShuffleDeck();

            for (int i = 0; i < 20000; i++)
            {
                Log.InfoFormat("**************************");
                Log.InfoFormat("Beginning Round {0} of {1}", i, 20000);
                Log.InfoFormat("**************************");

                foreach (Card card in Table.Instance.CardsInPlay)
                {
                    Table.Instance.DiscardedCards.Add(card);
                }

                //Log.DebugFormat("Discard List Contains:");
                //foreach (Card card in Table.Instance.DiscardedCards)
                //{
                //    Log.DebugFormat("{0},\n", card);
                //}

                //Log.DebugFormat("Cards in play were:");
                //foreach (Card card in Table.Instance.CardsInPlay)
                //{
                //    Log.DebugFormat("{0},\n", card);
                //}

                Table.Instance.CardsInPlay.Clear();

                //Log.DebugFormat("Playerlist contains:");
                //foreach (IPlayer player in Table.Instance.Players)
                //{
                //    Log.DebugFormat("{0}", player);
                //}
                
                foreach (IPlayer player in Table.Instance.Players)
                {
                    player.Hands.Clear();
                    //todo: implement reset logic, there's prob more to do, reclaim cards etc ?
                }

                dealer.Deal();

                if (dealer.ActiveHand.HasBlackJack)
                {
                    foreach (IPlayer player in Table.Instance.Players)
                    {
                        foreach (Hand hand in player.Hands)
                        {
                            hand.Status = HandStatusType.Completed;

                            if (player != dealer)
                            {
                                //todo: make sure BJ's push
                                dealer.DetermineOutcome(player, hand);
                                Log.InfoFormat("{0}'s outcome is {1}", player, player.Outcome);
                            }

                            //todo: reclaim cards ? 
                        }
                    }

                    continue;
                }

                foreach (IPlayer player in Table.Instance.Players)
                {
                    while (player.ActiveHand != null)
                    {
                        var playerDecision = dealer.PromptPlayerForDecision(player);
                        dealer.HandlePlayerDecision(player, playerDecision);
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
                            hand.Clear();
                        }
                    }
                    else
                    {
                        player.ActiveHand.Clear();
                    }
                }
            }

            foreach (IPlayer player in Table.Instance.Players)
            {
                if (player != dealer)
                {
                    Log.DebugFormat("{0} Statistics - {1}", player, player.Statistics);
                }
            }

            ExecutionTimeManager.RecordExecutionTime("Entire Game", start);

            if (ExecutionTimeManager.IsRecording())
            {
                Log.Debug(ExecutionTimeManager.GetExecutionTimes().ToString());
                ExecutionTimeManager.Clear();
            }
        }
    }
}
         