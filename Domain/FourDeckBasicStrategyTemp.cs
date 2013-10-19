using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Performance;
using System.Reflection;

namespace Domain.Temp
{
    public class FourDeckBasicStrategyTemp : IBasicStrategy
    {
        private CardBasedStrategy _rankBasedStrategyCard;
        private ValueBasedStrategy _valueBasedStrategyCard;

        public FourDeckBasicStrategyTemp()
        {
            _rankBasedStrategyCard = CreateStrategyCard();
            _valueBasedStrategyCard = CreateValueStrategyCard();
        }

        public DecisionType GetDecision(Hand playerHand, Card dealerUpCard)
        {
            long start = DateTime.Now.Ticks;

            try
            {
                if (playerHand.Count == 1)
                {
                    return DecisionType.Hit;
                }

                try
                {
                    return _rankBasedStrategyCard.Get(playerHand[0].Rank, playerHand[1].Rank, playerHand.Value, dealerUpCard.Rank);
                }
                catch (Exception nse)
                { }

                try
                {
                    return _valueBasedStrategyCard.Get(playerHand.Value, dealerUpCard.Rank);
                }
                catch (Exception nse)
                { }

                throw new DecisionNotFoundException(String.Format("Decision Not Found for player cards {0} and {1} with value {2}, dealerUpCard {3}", playerHand[0], playerHand[1], playerHand.Value, dealerUpCard));
            }
            finally
            {
                ExecutionTimeManager.RecordExecutionTime(String.Format("{0}.{1}()",
                    MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name), start);
            }
        }

        private class CardBasedStrategy
        {
            private Dictionary<CardRank, Dictionary<CardRank, Dictionary<int, Dictionary<CardRank, DecisionType>>>> _strategy;

            public CardBasedStrategy()
            {
                _strategy = new Dictionary<CardRank, Dictionary<CardRank, Dictionary<int, Dictionary<CardRank, DecisionType>>>>();
            }

            public DecisionType Get(CardRank firstCard, CardRank secondCard, int handValue, CardRank dealerCard)
            {
                //if (!_strategy.ContainsKey(firstCard))
                //{
                //    throw new NotSupportedException();
                //}
                //if (!_strategy[firstCard].ContainsKey(secondCard))
                //{
                //    throw new NotSupportedException();
                //}
                //if (!_strategy[firstCard][secondCard].ContainsKey(handValue))
                //{
                //    throw new NotSupportedException();
                //}
                //if (!_strategy[firstCard][secondCard][handValue].ContainsKey(dealerCard))
                //{
                //    throw new NotSupportedException();
                //}

                return _strategy[firstCard][secondCard][handValue][dealerCard];

            }

            public void Add(CardRank firstCard, CardRank secondCard, int handValue, CardRank dealerCard, DecisionType decision)
            {
                if (!_strategy.ContainsKey(firstCard))
                {
                    _strategy.Add(firstCard, new Dictionary<CardRank, Dictionary<int, Dictionary<CardRank, DecisionType>>>());
                }

                if (!_strategy[firstCard].ContainsKey(secondCard))
                {
                    _strategy[firstCard].Add(secondCard, new Dictionary<int, Dictionary<CardRank, DecisionType>>());
                }

                if (!_strategy[firstCard][secondCard].ContainsKey(handValue))
                {
                    _strategy[firstCard][secondCard].Add(handValue, new Dictionary<CardRank, DecisionType>());
                }

                if (!_strategy[firstCard][secondCard][handValue].ContainsKey(dealerCard))
                {
                    _strategy[firstCard][secondCard][handValue].Add(dealerCard, decision);
                }
            }
        }

        private class ValueBasedStrategy
        {
            private Dictionary<int, Dictionary<CardRank, DecisionType>> _strategy;

            public ValueBasedStrategy()
            {
                _strategy = new Dictionary<int, Dictionary<CardRank, DecisionType>>();
            }

            public void Add(int handValue, CardRank dealerCard, DecisionType decision)
            {
                if (!_strategy.ContainsKey(handValue))
                {
                    _strategy.Add(handValue, new Dictionary<CardRank, DecisionType>());
                }

                if (!_strategy[handValue].ContainsKey(dealerCard))
                {
                    _strategy[handValue].Add(dealerCard, decision);
                }
            }

            public DecisionType Get(int handvalue, CardRank dealerCard)
            {
                //if (!_strategy.ContainsKey(handvalue))
                //{
                //    throw new NotSupportedException();

                //}
                //if (!_strategy[handvalue].ContainsKey(dealerCard))
                //{
                //    throw new NotSupportedException();
                //}

                return _strategy[handvalue][dealerCard];
            }
        }



        private CardBasedStrategy CreateStrategyCard()
        {
            CardBasedStrategy strategy = new CardBasedStrategy();

            strategy.Add(CardRank.Two, CardRank.Two, 4, CardRank.Two, DecisionType.Split);
            strategy.Add(CardRank.Two, CardRank.Two, 4, CardRank.Three, DecisionType.Split);
            strategy.Add(CardRank.Two, CardRank.Two, 4, CardRank.Four, DecisionType.Split);
            strategy.Add(CardRank.Two, CardRank.Two, 4, CardRank.Five, DecisionType.Split);
            strategy.Add(CardRank.Two, CardRank.Two, 4, CardRank.Six, DecisionType.Split);
            strategy.Add(CardRank.Two, CardRank.Two, 4, CardRank.Seven, DecisionType.Split);
            strategy.Add(CardRank.Two, CardRank.Two, 4, CardRank.Eight, DecisionType.Hit);
            strategy.Add(CardRank.Two, CardRank.Two, 4, CardRank.Nine, DecisionType.Hit);
            strategy.Add(CardRank.Two, CardRank.Two, 4, CardRank.Ten, DecisionType.Hit);
            strategy.Add(CardRank.Two, CardRank.Two, 4, CardRank.Jack, DecisionType.Hit);
            strategy.Add(CardRank.Two, CardRank.Two, 4, CardRank.Queen, DecisionType.Hit);
            strategy.Add(CardRank.Two, CardRank.Two, 4, CardRank.King, DecisionType.Hit);
            strategy.Add(CardRank.Two, CardRank.Two, 4, CardRank.Ace, DecisionType.Hit);

            strategy.Add(CardRank.Three, CardRank.Three, 6, CardRank.Two, DecisionType.Split);
            strategy.Add(CardRank.Three, CardRank.Three, 6, CardRank.Three, DecisionType.Split);
            strategy.Add(CardRank.Three, CardRank.Three, 6, CardRank.Four, DecisionType.Split);
            strategy.Add(CardRank.Three, CardRank.Three, 6, CardRank.Five, DecisionType.Split);
            strategy.Add(CardRank.Three, CardRank.Three, 6, CardRank.Six, DecisionType.Split);
            strategy.Add(CardRank.Three, CardRank.Three, 6, CardRank.Seven, DecisionType.Split);
            strategy.Add(CardRank.Three, CardRank.Three, 6, CardRank.Eight, DecisionType.Hit);
            strategy.Add(CardRank.Three, CardRank.Three, 6, CardRank.Nine, DecisionType.Hit);
            strategy.Add(CardRank.Three, CardRank.Three, 6, CardRank.Ten, DecisionType.Hit);
            strategy.Add(CardRank.Three, CardRank.Three, 6, CardRank.Jack, DecisionType.Hit);
            strategy.Add(CardRank.Three, CardRank.Three, 6, CardRank.Queen, DecisionType.Hit);
            strategy.Add(CardRank.Three, CardRank.Three, 6, CardRank.King, DecisionType.Hit);
            strategy.Add(CardRank.Three, CardRank.Three, 6, CardRank.Ace, DecisionType.Hit);

            strategy.Add(CardRank.Four, CardRank.Four, 8, CardRank.Two, DecisionType.Hit);
            strategy.Add(CardRank.Four, CardRank.Four, 8, CardRank.Three, DecisionType.Hit);
            strategy.Add(CardRank.Four, CardRank.Four, 8, CardRank.Four, DecisionType.Hit);
            strategy.Add(CardRank.Four, CardRank.Four, 8, CardRank.Five, DecisionType.Split);
            strategy.Add(CardRank.Four, CardRank.Four, 8, CardRank.Six, DecisionType.Split);
            strategy.Add(CardRank.Four, CardRank.Four, 8, CardRank.Seven, DecisionType.Hit);
            strategy.Add(CardRank.Four, CardRank.Four, 8, CardRank.Eight, DecisionType.Hit);
            strategy.Add(CardRank.Four, CardRank.Four, 8, CardRank.Nine, DecisionType.Hit);
            strategy.Add(CardRank.Four, CardRank.Four, 8, CardRank.Ten, DecisionType.Hit);
            strategy.Add(CardRank.Four, CardRank.Four, 8, CardRank.Jack, DecisionType.Hit);
            strategy.Add(CardRank.Four, CardRank.Four, 8, CardRank.Queen, DecisionType.Hit);
            strategy.Add(CardRank.Four, CardRank.Four, 8, CardRank.King, DecisionType.Hit);
            strategy.Add(CardRank.Four, CardRank.Four, 8, CardRank.Ace, DecisionType.Hit);

            strategy.Add(CardRank.Five, CardRank.Five, 10, CardRank.Two, DecisionType.DoubleDown);
            strategy.Add(CardRank.Five, CardRank.Five, 10, CardRank.Three, DecisionType.DoubleDown);
            strategy.Add(CardRank.Five, CardRank.Five, 10, CardRank.Four, DecisionType.DoubleDown);
            strategy.Add(CardRank.Five, CardRank.Five, 10, CardRank.Five, DecisionType.DoubleDown);
            strategy.Add(CardRank.Five, CardRank.Five, 10, CardRank.Six, DecisionType.DoubleDown);
            strategy.Add(CardRank.Five, CardRank.Five, 10, CardRank.Seven, DecisionType.DoubleDown);
            strategy.Add(CardRank.Five, CardRank.Five, 10, CardRank.Eight, DecisionType.DoubleDown);
            strategy.Add(CardRank.Five, CardRank.Five, 10, CardRank.Nine, DecisionType.DoubleDown);
            strategy.Add(CardRank.Five, CardRank.Five, 10, CardRank.Ten, DecisionType.Hit);
            strategy.Add(CardRank.Five, CardRank.Five, 10, CardRank.Jack, DecisionType.Hit);
            strategy.Add(CardRank.Five, CardRank.Five, 10, CardRank.Queen, DecisionType.Hit);
            strategy.Add(CardRank.Five, CardRank.Five, 10, CardRank.King, DecisionType.Hit);
            strategy.Add(CardRank.Five, CardRank.Five, 10, CardRank.Ace, DecisionType.Hit);

            strategy.Add(CardRank.Six, CardRank.Six, 12, CardRank.Two, DecisionType.Split);
            strategy.Add(CardRank.Six, CardRank.Six, 12, CardRank.Three, DecisionType.Split);
            strategy.Add(CardRank.Six, CardRank.Six, 12, CardRank.Four, DecisionType.Split);
            strategy.Add(CardRank.Six, CardRank.Six, 12, CardRank.Five, DecisionType.Split);
            strategy.Add(CardRank.Six, CardRank.Six, 12, CardRank.Six, DecisionType.Split);
            strategy.Add(CardRank.Six, CardRank.Six, 12, CardRank.Seven, DecisionType.Hit);
            strategy.Add(CardRank.Six, CardRank.Six, 12, CardRank.Eight, DecisionType.Hit);
            strategy.Add(CardRank.Six, CardRank.Six, 12, CardRank.Nine, DecisionType.Hit);
            strategy.Add(CardRank.Six, CardRank.Six, 12, CardRank.Ten, DecisionType.Hit);
            strategy.Add(CardRank.Six, CardRank.Six, 12, CardRank.Jack, DecisionType.Hit);
            strategy.Add(CardRank.Six, CardRank.Six, 12, CardRank.Queen, DecisionType.Hit);
            strategy.Add(CardRank.Six, CardRank.Six, 12, CardRank.King, DecisionType.Hit);
            strategy.Add(CardRank.Six, CardRank.Six, 12, CardRank.Ace, DecisionType.Hit);

            strategy.Add(CardRank.Seven, CardRank.Seven, 14, CardRank.Two, DecisionType.Split);
            strategy.Add(CardRank.Seven, CardRank.Seven, 14, CardRank.Three, DecisionType.Split);
            strategy.Add(CardRank.Seven, CardRank.Seven, 14, CardRank.Four, DecisionType.Split);
            strategy.Add(CardRank.Seven, CardRank.Seven, 14, CardRank.Five, DecisionType.Split);
            strategy.Add(CardRank.Seven, CardRank.Seven, 14, CardRank.Six, DecisionType.Split);
            strategy.Add(CardRank.Seven, CardRank.Seven, 14, CardRank.Seven, DecisionType.Split);
            strategy.Add(CardRank.Seven, CardRank.Seven, 14, CardRank.Eight, DecisionType.Hit);
            strategy.Add(CardRank.Seven, CardRank.Seven, 14, CardRank.Nine, DecisionType.Hit);
            strategy.Add(CardRank.Seven, CardRank.Seven, 14, CardRank.Ten, DecisionType.Hit);
            strategy.Add(CardRank.Seven, CardRank.Seven, 14, CardRank.Jack, DecisionType.Hit);
            strategy.Add(CardRank.Seven, CardRank.Seven, 14, CardRank.Queen, DecisionType.Hit);
            strategy.Add(CardRank.Seven, CardRank.Seven, 14, CardRank.King, DecisionType.Hit);
            strategy.Add(CardRank.Seven, CardRank.Seven, 14, CardRank.Ace, DecisionType.Hit);

            strategy.Add(CardRank.Eight, CardRank.Eight, 16, CardRank.Two, DecisionType.Split);
            strategy.Add(CardRank.Eight, CardRank.Eight, 16, CardRank.Three, DecisionType.Split);
            strategy.Add(CardRank.Eight, CardRank.Eight, 16, CardRank.Four, DecisionType.Split);
            strategy.Add(CardRank.Eight, CardRank.Eight, 16, CardRank.Five, DecisionType.Split);
            strategy.Add(CardRank.Eight, CardRank.Eight, 16, CardRank.Six, DecisionType.Split);
            strategy.Add(CardRank.Eight, CardRank.Eight, 16, CardRank.Seven, DecisionType.Split);
            strategy.Add(CardRank.Eight, CardRank.Eight, 16, CardRank.Eight, DecisionType.Split);
            strategy.Add(CardRank.Eight, CardRank.Eight, 16, CardRank.Nine, DecisionType.Split);
            strategy.Add(CardRank.Eight, CardRank.Eight, 16, CardRank.Ten, DecisionType.Split);
            strategy.Add(CardRank.Eight, CardRank.Eight, 16, CardRank.Jack, DecisionType.Split);
            strategy.Add(CardRank.Eight, CardRank.Eight, 16, CardRank.Queen, DecisionType.Split);
            strategy.Add(CardRank.Eight, CardRank.Eight, 16, CardRank.King, DecisionType.Split);
            strategy.Add(CardRank.Eight, CardRank.Eight, 16, CardRank.Ace, DecisionType.Split);

            strategy.Add(CardRank.Nine, CardRank.Nine, 18, CardRank.Two, DecisionType.Split);
            strategy.Add(CardRank.Nine, CardRank.Nine, 18, CardRank.Three, DecisionType.Split);
            strategy.Add(CardRank.Nine, CardRank.Nine, 18, CardRank.Four, DecisionType.Split);
            strategy.Add(CardRank.Nine, CardRank.Nine, 18, CardRank.Five, DecisionType.Split);
            strategy.Add(CardRank.Nine, CardRank.Nine, 18, CardRank.Six, DecisionType.Split);
            strategy.Add(CardRank.Nine, CardRank.Nine, 18, CardRank.Seven, DecisionType.Stand);
            strategy.Add(CardRank.Nine, CardRank.Nine, 18, CardRank.Eight, DecisionType.Split);
            strategy.Add(CardRank.Nine, CardRank.Nine, 18, CardRank.Nine, DecisionType.Split);
            strategy.Add(CardRank.Nine, CardRank.Nine, 18, CardRank.Ten, DecisionType.Stand);
            strategy.Add(CardRank.Nine, CardRank.Nine, 18, CardRank.Jack, DecisionType.Stand);
            strategy.Add(CardRank.Nine, CardRank.Nine, 18, CardRank.Queen, DecisionType.Stand);
            strategy.Add(CardRank.Nine, CardRank.Nine, 18, CardRank.King, DecisionType.Stand);
            strategy.Add(CardRank.Nine, CardRank.Nine, 18, CardRank.Ace, DecisionType.Stand);

            strategy.Add(CardRank.Ten, CardRank.Ten, 20, CardRank.Two, DecisionType.Stand);
            strategy.Add(CardRank.Ten, CardRank.Ten, 20, CardRank.Three, DecisionType.Stand);
            strategy.Add(CardRank.Ten, CardRank.Ten, 20, CardRank.Four, DecisionType.Stand);
            strategy.Add(CardRank.Ten, CardRank.Ten, 20, CardRank.Five, DecisionType.Stand);
            strategy.Add(CardRank.Ten, CardRank.Ten, 20, CardRank.Six, DecisionType.Stand);
            strategy.Add(CardRank.Ten, CardRank.Ten, 20, CardRank.Seven, DecisionType.Stand);
            strategy.Add(CardRank.Ten, CardRank.Ten, 20, CardRank.Eight, DecisionType.Stand);
            strategy.Add(CardRank.Ten, CardRank.Ten, 20, CardRank.Nine, DecisionType.Stand);
            strategy.Add(CardRank.Ten, CardRank.Ten, 20, CardRank.Ten, DecisionType.Stand);
            strategy.Add(CardRank.Ten, CardRank.Ten, 20, CardRank.Jack, DecisionType.Stand);
            strategy.Add(CardRank.Ten, CardRank.Ten, 20, CardRank.Queen, DecisionType.Stand);
            strategy.Add(CardRank.Ten, CardRank.Ten, 20, CardRank.King, DecisionType.Stand);
            strategy.Add(CardRank.Ten, CardRank.Ten, 20, CardRank.Ace, DecisionType.Stand);

            strategy.Add(CardRank.Ace, CardRank.Ace, 12, CardRank.Two, DecisionType.Split);
            strategy.Add(CardRank.Ace, CardRank.Ace, 12, CardRank.Three, DecisionType.Split);
            strategy.Add(CardRank.Ace, CardRank.Ace, 12, CardRank.Four, DecisionType.Split);
            strategy.Add(CardRank.Ace, CardRank.Ace, 12, CardRank.Five, DecisionType.Split);
            strategy.Add(CardRank.Ace, CardRank.Ace, 12, CardRank.Six, DecisionType.Split);
            strategy.Add(CardRank.Ace, CardRank.Ace, 12, CardRank.Seven, DecisionType.Split);
            strategy.Add(CardRank.Ace, CardRank.Ace, 12, CardRank.Eight, DecisionType.Split);
            strategy.Add(CardRank.Ace, CardRank.Ace, 12, CardRank.Nine, DecisionType.Split);
            strategy.Add(CardRank.Ace, CardRank.Ace, 12, CardRank.Ten, DecisionType.Split);
            strategy.Add(CardRank.Ace, CardRank.Ace, 12, CardRank.Jack, DecisionType.Split);
            strategy.Add(CardRank.Ace, CardRank.Ace, 12, CardRank.Queen, DecisionType.Split);
            strategy.Add(CardRank.Ace, CardRank.Ace, 12, CardRank.King, DecisionType.Split);
            strategy.Add(CardRank.Ace, CardRank.Ace, 12, CardRank.Ace, DecisionType.Split);

            return strategy;
        }

        private ValueBasedStrategy CreateValueStrategyCard()
        {
            ValueBasedStrategy strategy = new ValueBasedStrategy();
            strategy.Add(5, CardRank.Two, DecisionType.Hit);
            strategy.Add(5, CardRank.Three, DecisionType.Hit);
            strategy.Add(5, CardRank.Four, DecisionType.Hit);
            strategy.Add(5, CardRank.Five, DecisionType.Hit);
            strategy.Add(5, CardRank.Six, DecisionType.Hit);
            strategy.Add(5, CardRank.Seven, DecisionType.Hit);
            strategy.Add(5, CardRank.Eight, DecisionType.Hit);
            strategy.Add(5, CardRank.Nine, DecisionType.Hit);
            strategy.Add(5, CardRank.Ten, DecisionType.Hit);
            strategy.Add(5, CardRank.Jack, DecisionType.Hit);
            strategy.Add(5, CardRank.Queen, DecisionType.Hit);
            strategy.Add(5, CardRank.King, DecisionType.Hit);
            strategy.Add(5, CardRank.Ace, DecisionType.Hit);

            strategy.Add(6, CardRank.Two, DecisionType.Hit);
            strategy.Add(6, CardRank.Three, DecisionType.Hit);
            strategy.Add(6, CardRank.Four, DecisionType.Hit);
            strategy.Add(6, CardRank.Five, DecisionType.Hit);
            strategy.Add(6, CardRank.Six, DecisionType.Hit);
            strategy.Add(6, CardRank.Seven, DecisionType.Hit);
            strategy.Add(6, CardRank.Eight, DecisionType.Hit);
            strategy.Add(6, CardRank.Nine, DecisionType.Hit);
            strategy.Add(6, CardRank.Ten, DecisionType.Hit);
            strategy.Add(6, CardRank.Jack, DecisionType.Hit);
            strategy.Add(6, CardRank.Queen, DecisionType.Hit);
            strategy.Add(6, CardRank.King, DecisionType.Hit);
            strategy.Add(6, CardRank.Ace, DecisionType.Hit);

            strategy.Add(7, CardRank.Two, DecisionType.Hit);
            strategy.Add(7, CardRank.Three, DecisionType.Hit);
            strategy.Add(7, CardRank.Four, DecisionType.Hit);
            strategy.Add(7, CardRank.Five, DecisionType.Hit);
            strategy.Add(7, CardRank.Six, DecisionType.Hit);
            strategy.Add(7, CardRank.Seven, DecisionType.Hit);
            strategy.Add(7, CardRank.Eight, DecisionType.Hit);
            strategy.Add(7, CardRank.Nine, DecisionType.Hit);
            strategy.Add(7, CardRank.Ten, DecisionType.Hit);
            strategy.Add(7, CardRank.Jack, DecisionType.Hit);
            strategy.Add(7, CardRank.Queen, DecisionType.Hit);
            strategy.Add(7, CardRank.King, DecisionType.Hit);
            strategy.Add(7, CardRank.Ace, DecisionType.Hit);

            strategy.Add(7, CardRank.Two, DecisionType.Hit);
            strategy.Add(7, CardRank.Three, DecisionType.Hit);
            strategy.Add(7, CardRank.Four, DecisionType.Hit);
            strategy.Add(7, CardRank.Five, DecisionType.Hit);
            strategy.Add(7, CardRank.Six, DecisionType.Hit);
            strategy.Add(7, CardRank.Seven, DecisionType.Hit);
            strategy.Add(7, CardRank.Eight, DecisionType.Hit);
            strategy.Add(7, CardRank.Nine, DecisionType.Hit);
            strategy.Add(7, CardRank.Ten, DecisionType.Hit);
            strategy.Add(7, CardRank.Jack, DecisionType.Hit);
            strategy.Add(7, CardRank.Queen, DecisionType.Hit);
            strategy.Add(7, CardRank.King, DecisionType.Hit);
            strategy.Add(7, CardRank.Ace, DecisionType.Hit);

            strategy.Add(8, CardRank.Two, DecisionType.Hit);
            strategy.Add(8, CardRank.Three, DecisionType.Hit);
            strategy.Add(8, CardRank.Four, DecisionType.Hit);
            strategy.Add(8, CardRank.Five, DecisionType.Hit);
            strategy.Add(8, CardRank.Six, DecisionType.Hit);
            strategy.Add(8, CardRank.Seven, DecisionType.Hit);
            strategy.Add(8, CardRank.Eight, DecisionType.Hit);
            strategy.Add(8, CardRank.Nine, DecisionType.Hit);
            strategy.Add(8, CardRank.Ten, DecisionType.Hit);
            strategy.Add(8, CardRank.Jack, DecisionType.Hit);
            strategy.Add(8, CardRank.Queen, DecisionType.Hit);
            strategy.Add(8, CardRank.King, DecisionType.Hit);
            strategy.Add(8, CardRank.Ace, DecisionType.Hit);


            strategy.Add(9, CardRank.Two, DecisionType.Hit);
            strategy.Add(9, CardRank.Three, DecisionType.DoubleDown);
            strategy.Add(9, CardRank.Four, DecisionType.DoubleDown);
            strategy.Add(9, CardRank.Five, DecisionType.DoubleDown);
            strategy.Add(9, CardRank.Six, DecisionType.DoubleDown);
            strategy.Add(9, CardRank.Seven, DecisionType.Hit);
            strategy.Add(9, CardRank.Eight, DecisionType.Hit);
            strategy.Add(9, CardRank.Nine, DecisionType.Hit);
            strategy.Add(9, CardRank.Ten, DecisionType.Hit);
            strategy.Add(9, CardRank.Jack, DecisionType.Hit);
            strategy.Add(9, CardRank.Queen, DecisionType.Hit);
            strategy.Add(9, CardRank.King, DecisionType.Hit);
            strategy.Add(9, CardRank.Ace, DecisionType.Hit);

            strategy.Add(10, CardRank.Two, DecisionType.DoubleDown);
            strategy.Add(10, CardRank.Three, DecisionType.DoubleDown);
            strategy.Add(10, CardRank.Four, DecisionType.DoubleDown);
            strategy.Add(10, CardRank.Five, DecisionType.DoubleDown);
            strategy.Add(10, CardRank.Six, DecisionType.DoubleDown);
            strategy.Add(10, CardRank.Seven, DecisionType.DoubleDown);
            strategy.Add(10, CardRank.Eight, DecisionType.DoubleDown);
            strategy.Add(10, CardRank.Nine, DecisionType.DoubleDown);
            strategy.Add(10, CardRank.Ten, DecisionType.Hit);
            strategy.Add(10, CardRank.Jack, DecisionType.Hit);
            strategy.Add(10, CardRank.Queen, DecisionType.Hit);
            strategy.Add(10, CardRank.King, DecisionType.Hit);
            strategy.Add(10, CardRank.Ace, DecisionType.Hit);

            strategy.Add(11, CardRank.Two, DecisionType.DoubleDown);
            strategy.Add(11, CardRank.Three, DecisionType.DoubleDown);
            strategy.Add(11, CardRank.Four, DecisionType.DoubleDown);
            strategy.Add(11, CardRank.Five, DecisionType.DoubleDown);
            strategy.Add(11, CardRank.Six, DecisionType.DoubleDown);
            strategy.Add(11, CardRank.Seven, DecisionType.DoubleDown);
            strategy.Add(11, CardRank.Eight, DecisionType.DoubleDown);
            strategy.Add(11, CardRank.Nine, DecisionType.DoubleDown);
            strategy.Add(11, CardRank.Ten, DecisionType.DoubleDown);
            strategy.Add(11, CardRank.Jack, DecisionType.DoubleDown);
            strategy.Add(11, CardRank.Queen, DecisionType.DoubleDown);
            strategy.Add(11, CardRank.King, DecisionType.DoubleDown);
            strategy.Add(11, CardRank.Ace, DecisionType.Hit);


            strategy.Add(12, CardRank.Two, DecisionType.Hit);
            strategy.Add(12, CardRank.Three, DecisionType.Hit);
            strategy.Add(12, CardRank.Four, DecisionType.Stand);
            strategy.Add(12, CardRank.Five, DecisionType.Stand);
            strategy.Add(12, CardRank.Six, DecisionType.Stand);
            strategy.Add(12, CardRank.Seven, DecisionType.Hit);
            strategy.Add(12, CardRank.Eight, DecisionType.Hit);
            strategy.Add(12, CardRank.Nine, DecisionType.Hit);
            strategy.Add(12, CardRank.Ten, DecisionType.Hit);
            strategy.Add(12, CardRank.Jack, DecisionType.Hit);
            strategy.Add(12, CardRank.Queen, DecisionType.Hit);
            strategy.Add(12, CardRank.King, DecisionType.Hit);
            strategy.Add(12, CardRank.Ace, DecisionType.Hit);

            strategy.Add(13, CardRank.Two, DecisionType.Stand);
            strategy.Add(13, CardRank.Three, DecisionType.Stand);
            strategy.Add(13, CardRank.Four, DecisionType.Stand);
            strategy.Add(13, CardRank.Five, DecisionType.Stand);
            strategy.Add(13, CardRank.Six, DecisionType.Stand);
            strategy.Add(13, CardRank.Seven, DecisionType.Hit);
            strategy.Add(13, CardRank.Eight, DecisionType.Hit);
            strategy.Add(13, CardRank.Nine, DecisionType.Hit);
            strategy.Add(13, CardRank.Ten, DecisionType.Hit);
            strategy.Add(13, CardRank.Jack, DecisionType.Hit);
            strategy.Add(13, CardRank.Queen, DecisionType.Hit);
            strategy.Add(13, CardRank.King, DecisionType.Hit);
            strategy.Add(13, CardRank.Ace, DecisionType.Hit);


            strategy.Add(14, CardRank.Two, DecisionType.Stand);
            strategy.Add(14, CardRank.Three, DecisionType.Stand);
            strategy.Add(14, CardRank.Four, DecisionType.Stand);
            strategy.Add(14, CardRank.Five, DecisionType.Stand);
            strategy.Add(14, CardRank.Six, DecisionType.Stand);
            strategy.Add(14, CardRank.Seven, DecisionType.Hit);
            strategy.Add(14, CardRank.Eight, DecisionType.Hit);
            strategy.Add(14, CardRank.Nine, DecisionType.Hit);
            strategy.Add(14, CardRank.Ten, DecisionType.Hit);
            strategy.Add(14, CardRank.Jack, DecisionType.Hit);
            strategy.Add(14, CardRank.Queen, DecisionType.Hit);
            strategy.Add(14, CardRank.King, DecisionType.Hit);
            strategy.Add(14, CardRank.Ace, DecisionType.Hit);


            strategy.Add(15, CardRank.Two, DecisionType.Stand);
            strategy.Add(15, CardRank.Three, DecisionType.Stand);
            strategy.Add(15, CardRank.Four, DecisionType.Stand);
            strategy.Add(15, CardRank.Five, DecisionType.Stand);
            strategy.Add(15, CardRank.Six, DecisionType.Stand);
            strategy.Add(15, CardRank.Seven, DecisionType.Hit);
            strategy.Add(15, CardRank.Eight, DecisionType.Hit);
            strategy.Add(15, CardRank.Nine, DecisionType.Hit);
            strategy.Add(15, CardRank.Ten, DecisionType.Hit);
            strategy.Add(15, CardRank.Jack, DecisionType.Hit);
            strategy.Add(15, CardRank.Queen, DecisionType.Hit);
            strategy.Add(15, CardRank.King, DecisionType.Hit);
            strategy.Add(15, CardRank.Ace, DecisionType.Hit);


            strategy.Add(16, CardRank.Two, DecisionType.Stand);
            strategy.Add(16, CardRank.Three, DecisionType.Stand);
            strategy.Add(16, CardRank.Four, DecisionType.Stand);
            strategy.Add(16, CardRank.Five, DecisionType.Stand);
            strategy.Add(16, CardRank.Six, DecisionType.Stand);
            strategy.Add(16, CardRank.Seven, DecisionType.Hit);
            strategy.Add(16, CardRank.Eight, DecisionType.Hit);
            strategy.Add(16, CardRank.Nine, DecisionType.Hit);
            strategy.Add(16, CardRank.Ten, DecisionType.Hit);
            strategy.Add(16, CardRank.Jack, DecisionType.Hit);
            strategy.Add(16, CardRank.Queen, DecisionType.Hit);
            strategy.Add(16, CardRank.King, DecisionType.Hit);
            strategy.Add(16, CardRank.Ace, DecisionType.Hit);


            strategy.Add(17, CardRank.Two, DecisionType.Stand);
            strategy.Add(17, CardRank.Three, DecisionType.Stand);
            strategy.Add(17, CardRank.Four, DecisionType.Stand);
            strategy.Add(17, CardRank.Five, DecisionType.Stand);
            strategy.Add(17, CardRank.Six, DecisionType.Stand);
            strategy.Add(17, CardRank.Seven, DecisionType.Stand);
            strategy.Add(17, CardRank.Eight, DecisionType.Stand);
            strategy.Add(17, CardRank.Nine, DecisionType.Stand);
            strategy.Add(17, CardRank.Ten, DecisionType.Stand);
            strategy.Add(17, CardRank.Jack, DecisionType.Stand);
            strategy.Add(17, CardRank.Queen, DecisionType.Stand);
            strategy.Add(17, CardRank.King, DecisionType.Stand);
            strategy.Add(17, CardRank.Ace, DecisionType.Stand);


            strategy.Add(18, CardRank.Two, DecisionType.Stand);
            strategy.Add(18, CardRank.Three, DecisionType.Stand);
            strategy.Add(18, CardRank.Four, DecisionType.Stand);
            strategy.Add(18, CardRank.Five, DecisionType.Stand);
            strategy.Add(18, CardRank.Six, DecisionType.Stand);
            strategy.Add(18, CardRank.Seven, DecisionType.Stand);
            strategy.Add(18, CardRank.Eight, DecisionType.Stand);
            strategy.Add(18, CardRank.Nine, DecisionType.Stand);
            strategy.Add(18, CardRank.Ten, DecisionType.Stand);
            strategy.Add(18, CardRank.Jack, DecisionType.Stand);
            strategy.Add(18, CardRank.Queen, DecisionType.Stand);
            strategy.Add(18, CardRank.King, DecisionType.Stand);
            strategy.Add(18, CardRank.Ace, DecisionType.Stand);


            strategy.Add(19, CardRank.Two, DecisionType.Stand);
            strategy.Add(19, CardRank.Three, DecisionType.Stand);
            strategy.Add(19, CardRank.Four, DecisionType.Stand);
            strategy.Add(19, CardRank.Five, DecisionType.Stand);
            strategy.Add(19, CardRank.Six, DecisionType.Stand);
            strategy.Add(19, CardRank.Seven, DecisionType.Stand);
            strategy.Add(19, CardRank.Eight, DecisionType.Stand);
            strategy.Add(19, CardRank.Nine, DecisionType.Stand);
            strategy.Add(19, CardRank.Ten, DecisionType.Stand);
            strategy.Add(19, CardRank.Jack, DecisionType.Stand);
            strategy.Add(19, CardRank.Queen, DecisionType.Stand);
            strategy.Add(19, CardRank.King, DecisionType.Stand);
            strategy.Add(19, CardRank.Ace, DecisionType.Stand);


            strategy.Add(20, CardRank.Two, DecisionType.Stand);
            strategy.Add(20, CardRank.Three, DecisionType.Stand);
            strategy.Add(20, CardRank.Four, DecisionType.Stand);
            strategy.Add(20, CardRank.Five, DecisionType.Stand);
            strategy.Add(20, CardRank.Six, DecisionType.Stand);
            strategy.Add(20, CardRank.Seven, DecisionType.Stand);
            strategy.Add(20, CardRank.Eight, DecisionType.Stand);
            strategy.Add(20, CardRank.Nine, DecisionType.Stand);
            strategy.Add(20, CardRank.Ten, DecisionType.Stand);
            strategy.Add(20, CardRank.Jack, DecisionType.Stand);
            strategy.Add(20, CardRank.Queen, DecisionType.Stand);
            strategy.Add(20, CardRank.King, DecisionType.Stand);
            strategy.Add(20, CardRank.Ace, DecisionType.Stand);

            strategy.Add(21, CardRank.Two, DecisionType.Stand);
            strategy.Add(21, CardRank.Three, DecisionType.Stand);
            strategy.Add(21, CardRank.Four, DecisionType.Stand);
            strategy.Add(21, CardRank.Five, DecisionType.Stand);
            strategy.Add(21, CardRank.Six, DecisionType.Stand);
            strategy.Add(21, CardRank.Seven, DecisionType.Stand);
            strategy.Add(21, CardRank.Eight, DecisionType.Stand);
            strategy.Add(21, CardRank.Nine, DecisionType.Stand);
            strategy.Add(21, CardRank.Ten, DecisionType.Stand);
            strategy.Add(21, CardRank.Jack, DecisionType.Stand);
            strategy.Add(21, CardRank.Queen, DecisionType.Stand);
            strategy.Add(21, CardRank.King, DecisionType.Stand);
            strategy.Add(21, CardRank.Ace, DecisionType.Stand);

            return strategy;
        }
    }
}


      

            

       
    

