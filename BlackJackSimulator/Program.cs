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

                Log.DebugFormat("Resetting to original players...");
                int playerListCount = Table.Instance.Players.Count;
                for (int index = 0; index < playerListCount; index++)
                {
                    IPlayer player = Table.Instance.Players[index];
                    player.SplitPlayers.Clear();
                    player.Outcome = PlayerOutcomeType.InProgress;
                    player.Status = PlayerStatusType.Active;

                    if (player.IsSplitPlayer)
                    {
                        Log.DebugFormat("Removing {0}", player);
                        Table.Instance.Players.RemoveAt(index);
                        index--;
                        playerListCount--;
                    }
                }

                if (Table.Instance.Players.Count != 4)
                {
                    throw new NotSupportedException("more than 4 players encountered");
                }

                dealer.Deal();

                if (dealer.HasBlackJack)
                {
                    foreach (IPlayer player in Table.Instance.Players)
                    {
                        if (player != dealer)
                        {
                            dealer.DetermineOutcome(player);
                            Log.InfoFormat("{0}'s outcome is {1}", player, player.Outcome);
                        }

                        player.FlushHand();
                    }

                    continue;
                }

                playerListCount = Table.Instance.Players.Count;
                for (int index = 0; index < playerListCount; index++)
                {
                    DecisionType playerDecision;

                    do
                    {
                        playerDecision = dealer.PromptPlayerForDecision(Table.Instance.Players[index]);

                        dealer.HandlePlayerDecision(Table.Instance.Players[index], playerDecision);

                    } while (Table.Instance.Players[index].Status != PlayerStatusType.Busted && playerDecision != DecisionType.Stand && playerDecision != DecisionType.Surrender && playerDecision != DecisionType.DoubleDown);

                    playerListCount = Table.Instance.Players.Count;

                }

                foreach (IPlayer player in Table.Instance.Players)
                {
                    if (player != dealer)
                    {
                        dealer.DetermineOutcome(player);

                        Log.InfoFormat("{0}'s outcome is {1}", player, player.Outcome);

                        dealer.DeterminePayout(player);
                    }

                    player.FlushHand();
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
         