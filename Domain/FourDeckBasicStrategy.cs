using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Performance;
using System.Reflection;

namespace Domain
{
    public class FourDeckBasicStrategy : IBasicStrategy
    {
        private IList<CardBasedStrategyCell> _strategyCard;
        private IList<ValueBasedStrategyCell> _strategyCard2;

        public static FourDeckBasicStrategy Instance
        {
            get { return Singleton<FourDeckBasicStrategy>.GetInstance(delegate { return new FourDeckBasicStrategy(); }); }
        }
    

        private FourDeckBasicStrategy()
        {
            long start = DateTime.Now.Ticks;

            _strategyCard = CreateStrategyCard();
            _strategyCard2 = CreateStrategyCard2();

            //ExecutionTimeManager.RecordExecutionTime(String.Format("{0}.{1}()",
            //        MethodInfo.GetCurrentMethod().DeclaringType.Name,
            //        MethodInfo.GetCurrentMethod().Name), start);
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

                foreach (CardBasedStrategyCell cell in _strategyCard)
                {
                    if (((cell._firstCard == playerHand[0].Rank && cell._secondCard == playerHand[1].Rank) || (cell._firstCard == playerHand[1].Rank && cell._secondCard == playerHand[0].Rank)) && cell._dealerCard == dealerUpCard.Rank)
                    {
                        DecisionType decision = cell._decision;
                        return decision;
                    }
                }

                foreach (ValueBasedStrategyCell cell in _strategyCard2)
                {
                    if ((cell._handValue == playerHand.Value) && cell._dealerCard == dealerUpCard.Rank)
                    {
                        DecisionType decision = cell._decision;
                        return decision;
                    }
                }

                throw new DecisionNotFoundException(String.Format("Decision Not Found for player cards {0} and {1} with value {2}, dealerUpCard {3}", playerHand[0], playerHand[1], playerHand.Value, dealerUpCard));
            }
            finally
            {
                //ExecutionTimeManager.RecordExecutionTime(String.Format("{0}.{1}()",
                //    MethodInfo.GetCurrentMethod().DeclaringType.Name,
                //    MethodInfo.GetCurrentMethod().Name), start);
            }

       }      

        private struct CardBasedStrategyCell
        {
            public CardRank _firstCard;
            public CardRank _secondCard;
            public int _handValue;
            public CardRank _dealerCard;
            public DecisionType _decision;

            public CardBasedStrategyCell(CardRank firstCard, CardRank secondCard, int handValue, CardRank dealerCard, DecisionType decision)
            {
                _firstCard = firstCard;
                _secondCard = secondCard;
                _handValue = handValue;
                _dealerCard = dealerCard;
                _decision = decision;
            }

        }

        private struct ValueBasedStrategyCell
        {            
            public int _handValue;
            public CardRank _dealerCard;
            public DecisionType _decision;

            public ValueBasedStrategyCell(int handValue, CardRank dealerCard, DecisionType decision)
            {
                _handValue = handValue;
                _dealerCard = dealerCard;
                _decision = decision;
            }
        }


        private IList<CardBasedStrategyCell> CreateStrategyCard()
        {
            long start = DateTime.Now.Ticks;

            IList<CardBasedStrategyCell> strategyCells = new List<CardBasedStrategyCell> 
            { 
                new CardBasedStrategyCell(CardRank.Two, CardRank.Two, 4, CardRank.Two, DecisionType.Split),
                new CardBasedStrategyCell(CardRank.Two, CardRank.Two, 4, CardRank.Three, DecisionType.Split),
                new CardBasedStrategyCell(CardRank.Two, CardRank.Two, 4, CardRank.Four, DecisionType.Split),
                new CardBasedStrategyCell(CardRank.Two, CardRank.Two, 4, CardRank.Five, DecisionType.Split),
                new CardBasedStrategyCell(CardRank.Two, CardRank.Two, 4, CardRank.Six, DecisionType.Split),
                new CardBasedStrategyCell(CardRank.Two, CardRank.Two, 4, CardRank.Seven, DecisionType.Split),
                new CardBasedStrategyCell(CardRank.Two, CardRank.Two, 4, CardRank.Eight, DecisionType.Hit),
                new CardBasedStrategyCell(CardRank.Two, CardRank.Two, 4, CardRank.Nine, DecisionType.Hit),
                new CardBasedStrategyCell(CardRank.Two, CardRank.Two, 4, CardRank.Ten, DecisionType.Hit),
                new CardBasedStrategyCell(CardRank.Two, CardRank.Two, 4, CardRank.Jack, DecisionType.Hit),
                new CardBasedStrategyCell(CardRank.Two, CardRank.Two, 4, CardRank.Queen, DecisionType.Hit),
                new CardBasedStrategyCell(CardRank.Two, CardRank.Two, 4, CardRank.King, DecisionType.Hit),
                new CardBasedStrategyCell(CardRank.Two, CardRank.Two, 4, CardRank.Ace, DecisionType.Hit),

                new CardBasedStrategyCell(CardRank.Three, CardRank.Three, 6, CardRank.Two, DecisionType.Split),
                new CardBasedStrategyCell(CardRank.Three, CardRank.Three, 6, CardRank.Three, DecisionType.Split),
                new CardBasedStrategyCell(CardRank.Three, CardRank.Three, 6, CardRank.Four, DecisionType.Split),
                new CardBasedStrategyCell(CardRank.Three, CardRank.Three, 6, CardRank.Five, DecisionType.Split),
                new CardBasedStrategyCell(CardRank.Three, CardRank.Three, 6, CardRank.Six, DecisionType.Split),
                new CardBasedStrategyCell(CardRank.Three, CardRank.Three, 6, CardRank.Seven, DecisionType.Split),
                new CardBasedStrategyCell(CardRank.Three, CardRank.Three, 6, CardRank.Eight, DecisionType.Hit),
                new CardBasedStrategyCell(CardRank.Three, CardRank.Three, 6, CardRank.Nine, DecisionType.Hit),
                new CardBasedStrategyCell(CardRank.Three, CardRank.Three, 6, CardRank.Ten, DecisionType.Hit),
                new CardBasedStrategyCell(CardRank.Three, CardRank.Three, 6, CardRank.Jack, DecisionType.Hit),
                new CardBasedStrategyCell(CardRank.Three, CardRank.Three, 6, CardRank.Queen, DecisionType.Hit),
                new CardBasedStrategyCell(CardRank.Three, CardRank.Three, 6, CardRank.King, DecisionType.Hit),
                new CardBasedStrategyCell(CardRank.Three, CardRank.Three, 6, CardRank.Ace, DecisionType.Hit),

                new CardBasedStrategyCell(CardRank.Four, CardRank.Four, 8, CardRank.Two, DecisionType.Hit),
                new CardBasedStrategyCell(CardRank.Four, CardRank.Four, 8, CardRank.Three, DecisionType.Hit),
                new CardBasedStrategyCell(CardRank.Four, CardRank.Four, 8, CardRank.Four, DecisionType.Hit),
                new CardBasedStrategyCell(CardRank.Four, CardRank.Four, 8, CardRank.Five, DecisionType.Split),
                new CardBasedStrategyCell(CardRank.Four, CardRank.Four, 8, CardRank.Six, DecisionType.Split),
                new CardBasedStrategyCell(CardRank.Four, CardRank.Four, 8, CardRank.Seven, DecisionType.Hit),
                new CardBasedStrategyCell(CardRank.Four, CardRank.Four, 8, CardRank.Eight, DecisionType.Hit),
                new CardBasedStrategyCell(CardRank.Four, CardRank.Four, 8, CardRank.Nine, DecisionType.Hit),
                new CardBasedStrategyCell(CardRank.Four, CardRank.Four, 8, CardRank.Ten, DecisionType.Hit),
                new CardBasedStrategyCell(CardRank.Four, CardRank.Four, 8, CardRank.Jack, DecisionType.Hit),
                new CardBasedStrategyCell(CardRank.Four, CardRank.Four, 8, CardRank.Queen, DecisionType.Hit),
                new CardBasedStrategyCell(CardRank.Four, CardRank.Four, 8, CardRank.King, DecisionType.Hit),
                new CardBasedStrategyCell(CardRank.Four, CardRank.Four, 8, CardRank.Ace, DecisionType.Hit),

                new CardBasedStrategyCell(CardRank.Five, CardRank.Five, 10, CardRank.Two, DecisionType.DoubleDown),
                new CardBasedStrategyCell(CardRank.Five, CardRank.Five, 10, CardRank.Three, DecisionType.DoubleDown),
                new CardBasedStrategyCell(CardRank.Five, CardRank.Five, 10, CardRank.Four, DecisionType.DoubleDown),
                new CardBasedStrategyCell(CardRank.Five, CardRank.Five, 10, CardRank.Five, DecisionType.DoubleDown),
                new CardBasedStrategyCell(CardRank.Five, CardRank.Five, 10, CardRank.Six, DecisionType.DoubleDown),
                new CardBasedStrategyCell(CardRank.Five, CardRank.Five, 10, CardRank.Seven, DecisionType.DoubleDown),
                new CardBasedStrategyCell(CardRank.Five, CardRank.Five, 10, CardRank.Eight, DecisionType.DoubleDown),
                new CardBasedStrategyCell(CardRank.Five, CardRank.Five, 10, CardRank.Nine, DecisionType.DoubleDown),
                new CardBasedStrategyCell(CardRank.Five, CardRank.Five, 10, CardRank.Ten, DecisionType.Hit),
                new CardBasedStrategyCell(CardRank.Five, CardRank.Five, 10, CardRank.Jack, DecisionType.Hit),
                new CardBasedStrategyCell(CardRank.Five, CardRank.Five, 10, CardRank.Queen, DecisionType.Hit),
                new CardBasedStrategyCell(CardRank.Five, CardRank.Five, 10, CardRank.King, DecisionType.Hit),
                new CardBasedStrategyCell(CardRank.Five, CardRank.Five, 10, CardRank.Ace, DecisionType.Hit),
                                 
                new CardBasedStrategyCell(CardRank.Six, CardRank.Six, 12, CardRank.Two, DecisionType.Split),
                new CardBasedStrategyCell(CardRank.Six, CardRank.Six, 12, CardRank.Three, DecisionType.Split),
                new CardBasedStrategyCell(CardRank.Six, CardRank.Six, 12, CardRank.Four, DecisionType.Split),
                new CardBasedStrategyCell(CardRank.Six, CardRank.Six, 12, CardRank.Five, DecisionType.Split),
                new CardBasedStrategyCell(CardRank.Six, CardRank.Six, 12, CardRank.Six, DecisionType.Split),
                new CardBasedStrategyCell(CardRank.Six, CardRank.Six, 12, CardRank.Seven, DecisionType.Hit),
                new CardBasedStrategyCell(CardRank.Six, CardRank.Six, 12, CardRank.Eight, DecisionType.Hit),
                new CardBasedStrategyCell(CardRank.Six, CardRank.Six, 12, CardRank.Nine, DecisionType.Hit),
                new CardBasedStrategyCell(CardRank.Six, CardRank.Six, 12, CardRank.Ten, DecisionType.Hit),
                new CardBasedStrategyCell(CardRank.Six, CardRank.Six, 12, CardRank.Jack, DecisionType.Hit),
                new CardBasedStrategyCell(CardRank.Six, CardRank.Six, 12, CardRank.Queen, DecisionType.Hit),
                new CardBasedStrategyCell(CardRank.Six, CardRank.Six, 12, CardRank.King, DecisionType.Hit),
                new CardBasedStrategyCell(CardRank.Six, CardRank.Six, 12, CardRank.Ace, DecisionType.Hit),
                  
                new CardBasedStrategyCell(CardRank.Seven, CardRank.Seven, 14, CardRank.Two, DecisionType.Split),
                new CardBasedStrategyCell(CardRank.Seven, CardRank.Seven, 14, CardRank.Three, DecisionType.Split),
                new CardBasedStrategyCell(CardRank.Seven, CardRank.Seven, 14, CardRank.Four, DecisionType.Split),
                new CardBasedStrategyCell(CardRank.Seven, CardRank.Seven, 14, CardRank.Five, DecisionType.Split),
                new CardBasedStrategyCell(CardRank.Seven, CardRank.Seven, 14, CardRank.Six, DecisionType.Split),
                new CardBasedStrategyCell(CardRank.Seven, CardRank.Seven, 14, CardRank.Seven, DecisionType.Split),
                new CardBasedStrategyCell(CardRank.Seven, CardRank.Seven, 14, CardRank.Eight, DecisionType.Hit),
                new CardBasedStrategyCell(CardRank.Seven, CardRank.Seven, 14, CardRank.Nine, DecisionType.Hit),
                new CardBasedStrategyCell(CardRank.Seven, CardRank.Seven, 14, CardRank.Ten, DecisionType.Hit),
                new CardBasedStrategyCell(CardRank.Seven, CardRank.Seven, 14, CardRank.Jack, DecisionType.Hit),
                new CardBasedStrategyCell(CardRank.Seven, CardRank.Seven, 14, CardRank.Queen, DecisionType.Hit),
                new CardBasedStrategyCell(CardRank.Seven, CardRank.Seven, 14, CardRank.King, DecisionType.Hit),
                new CardBasedStrategyCell(CardRank.Seven, CardRank.Seven, 14, CardRank.Ace, DecisionType.Hit),
                
                new CardBasedStrategyCell(CardRank.Eight, CardRank.Eight, 16, CardRank.Two, DecisionType.Split),
                new CardBasedStrategyCell(CardRank.Eight, CardRank.Eight, 16, CardRank.Three, DecisionType.Split),
                new CardBasedStrategyCell(CardRank.Eight, CardRank.Eight, 16, CardRank.Four, DecisionType.Split),
                new CardBasedStrategyCell(CardRank.Eight, CardRank.Eight, 16, CardRank.Five, DecisionType.Split),
                new CardBasedStrategyCell(CardRank.Eight, CardRank.Eight, 16, CardRank.Six, DecisionType.Split),
                new CardBasedStrategyCell(CardRank.Eight, CardRank.Eight, 16, CardRank.Seven, DecisionType.Split),
                new CardBasedStrategyCell(CardRank.Eight, CardRank.Eight, 16, CardRank.Eight, DecisionType.Split),
                new CardBasedStrategyCell(CardRank.Eight, CardRank.Eight, 16, CardRank.Nine, DecisionType.Split),
                new CardBasedStrategyCell(CardRank.Eight, CardRank.Eight, 16, CardRank.Ten, DecisionType.Split),
                new CardBasedStrategyCell(CardRank.Eight, CardRank.Eight, 16, CardRank.Jack, DecisionType.Split),
                new CardBasedStrategyCell(CardRank.Eight, CardRank.Eight, 16, CardRank.Queen, DecisionType.Split),
                new CardBasedStrategyCell(CardRank.Eight, CardRank.Eight, 16, CardRank.King, DecisionType.Split),
                new CardBasedStrategyCell(CardRank.Eight, CardRank.Eight, 16, CardRank.Ace, DecisionType.Split),

                new CardBasedStrategyCell(CardRank.Nine, CardRank.Nine, 18, CardRank.Two, DecisionType.Split),
                new CardBasedStrategyCell(CardRank.Nine, CardRank.Nine, 18, CardRank.Three, DecisionType.Split),
                new CardBasedStrategyCell(CardRank.Nine, CardRank.Nine, 18, CardRank.Four, DecisionType.Split),
                new CardBasedStrategyCell(CardRank.Nine, CardRank.Nine, 18, CardRank.Five, DecisionType.Split),
                new CardBasedStrategyCell(CardRank.Nine, CardRank.Nine, 18, CardRank.Six, DecisionType.Split),
                new CardBasedStrategyCell(CardRank.Nine, CardRank.Nine, 18, CardRank.Seven, DecisionType.Stand),
                new CardBasedStrategyCell(CardRank.Nine, CardRank.Nine, 18, CardRank.Eight, DecisionType.Split),
                new CardBasedStrategyCell(CardRank.Nine, CardRank.Nine, 18, CardRank.Nine, DecisionType.Split),
                new CardBasedStrategyCell(CardRank.Nine, CardRank.Nine, 18, CardRank.Ten, DecisionType.Stand),
                new CardBasedStrategyCell(CardRank.Nine, CardRank.Nine, 18, CardRank.Jack, DecisionType.Stand),
                new CardBasedStrategyCell(CardRank.Nine, CardRank.Nine, 18, CardRank.Queen, DecisionType.Stand),
                new CardBasedStrategyCell(CardRank.Nine, CardRank.Nine, 18, CardRank.King, DecisionType.Stand),
                new CardBasedStrategyCell(CardRank.Nine, CardRank.Nine, 18, CardRank.Ace, DecisionType.Stand),

                new CardBasedStrategyCell(CardRank.Ten, CardRank.Ten, 20, CardRank.Two, DecisionType.Stand),
                new CardBasedStrategyCell(CardRank.Ten, CardRank.Ten, 20, CardRank.Three, DecisionType.Stand),
                new CardBasedStrategyCell(CardRank.Ten, CardRank.Ten, 20, CardRank.Four, DecisionType.Stand),
                new CardBasedStrategyCell(CardRank.Ten, CardRank.Ten, 20, CardRank.Five, DecisionType.Stand),
                new CardBasedStrategyCell(CardRank.Ten, CardRank.Ten, 20, CardRank.Six, DecisionType.Stand),
                new CardBasedStrategyCell(CardRank.Ten, CardRank.Ten, 20, CardRank.Seven, DecisionType.Stand),
                new CardBasedStrategyCell(CardRank.Ten, CardRank.Ten, 20, CardRank.Eight, DecisionType.Stand),
                new CardBasedStrategyCell(CardRank.Ten, CardRank.Ten, 20, CardRank.Nine, DecisionType.Stand),
                new CardBasedStrategyCell(CardRank.Ten, CardRank.Ten, 20, CardRank.Ten, DecisionType.Stand),
                new CardBasedStrategyCell(CardRank.Ten, CardRank.Ten, 20, CardRank.Jack, DecisionType.Stand),
                new CardBasedStrategyCell(CardRank.Ten, CardRank.Ten, 20, CardRank.Queen, DecisionType.Stand),
                new CardBasedStrategyCell(CardRank.Ten, CardRank.Ten, 20, CardRank.King, DecisionType.Stand),
                new CardBasedStrategyCell(CardRank.Ten, CardRank.Ten, 20, CardRank.Ace, DecisionType.Stand),

                new CardBasedStrategyCell(CardRank.Ace, CardRank.Ace, 12, CardRank.Two, DecisionType.Split),
                new CardBasedStrategyCell(CardRank.Ace, CardRank.Ace, 12, CardRank.Three, DecisionType.Split),
                new CardBasedStrategyCell(CardRank.Ace, CardRank.Ace, 12, CardRank.Four, DecisionType.Split),
                new CardBasedStrategyCell(CardRank.Ace, CardRank.Ace, 12, CardRank.Five, DecisionType.Split),
                new CardBasedStrategyCell(CardRank.Ace, CardRank.Ace, 12, CardRank.Six, DecisionType.Split),
                new CardBasedStrategyCell(CardRank.Ace, CardRank.Ace, 12, CardRank.Seven, DecisionType.Split),
                new CardBasedStrategyCell(CardRank.Ace, CardRank.Ace, 12, CardRank.Eight, DecisionType.Split),
                new CardBasedStrategyCell(CardRank.Ace, CardRank.Ace, 12, CardRank.Nine, DecisionType.Split),
                new CardBasedStrategyCell(CardRank.Ace, CardRank.Ace, 12, CardRank.Ten, DecisionType.Split),
                new CardBasedStrategyCell(CardRank.Ace, CardRank.Ace, 12, CardRank.Jack, DecisionType.Split),
                new CardBasedStrategyCell(CardRank.Ace, CardRank.Ace, 12, CardRank.Queen, DecisionType.Split),
                new CardBasedStrategyCell(CardRank.Ace, CardRank.Ace, 12, CardRank.King, DecisionType.Split),
                new CardBasedStrategyCell(CardRank.Ace, CardRank.Ace, 12, CardRank.Ace, DecisionType.Split)
            };

            //ExecutionTimeManager.RecordExecutionTime(String.Format("{0}.{1}()",
            //        MethodInfo.GetCurrentMethod().DeclaringType.Name,
            //        MethodInfo.GetCurrentMethod().Name), start);

            return strategyCells;
            
        }

        private IList<ValueBasedStrategyCell> CreateStrategyCard2()
        {
            long start = DateTime.Now.Ticks;

            IList<ValueBasedStrategyCell> strategyCells = new List<ValueBasedStrategyCell> 
            { 
                new ValueBasedStrategyCell(5, CardRank.Two, DecisionType.Hit),
                new ValueBasedStrategyCell(5, CardRank.Three, DecisionType.Hit),
                new ValueBasedStrategyCell(5, CardRank.Four, DecisionType.Hit),
                new ValueBasedStrategyCell(5, CardRank.Five, DecisionType.Hit),
                new ValueBasedStrategyCell(5, CardRank.Six, DecisionType.Hit),
                new ValueBasedStrategyCell(5, CardRank.Seven, DecisionType.Hit),
                new ValueBasedStrategyCell(5, CardRank.Eight, DecisionType.Hit),
                new ValueBasedStrategyCell(5, CardRank.Nine, DecisionType.Hit),
                new ValueBasedStrategyCell(5, CardRank.Ten, DecisionType.Hit),
                new ValueBasedStrategyCell(5, CardRank.Jack, DecisionType.Hit),
                new ValueBasedStrategyCell(5, CardRank.Queen, DecisionType.Hit),
                new ValueBasedStrategyCell(5, CardRank.King, DecisionType.Hit),
                new ValueBasedStrategyCell(5, CardRank.Ace, DecisionType.Hit),

                new ValueBasedStrategyCell(6, CardRank.Two, DecisionType.Hit),
                new ValueBasedStrategyCell(6, CardRank.Three, DecisionType.Hit),
                new ValueBasedStrategyCell(6, CardRank.Four, DecisionType.Hit),
                new ValueBasedStrategyCell(6, CardRank.Five, DecisionType.Hit),
                new ValueBasedStrategyCell(6, CardRank.Six, DecisionType.Hit),
                new ValueBasedStrategyCell(6, CardRank.Seven, DecisionType.Hit),
                new ValueBasedStrategyCell(6, CardRank.Eight, DecisionType.Hit),
                new ValueBasedStrategyCell(6, CardRank.Nine, DecisionType.Hit),
                new ValueBasedStrategyCell(6, CardRank.Ten, DecisionType.Hit),
                new ValueBasedStrategyCell(6, CardRank.Jack, DecisionType.Hit),
                new ValueBasedStrategyCell(6, CardRank.Queen, DecisionType.Hit),
                new ValueBasedStrategyCell(6, CardRank.King, DecisionType.Hit),
                new ValueBasedStrategyCell(6, CardRank.Ace, DecisionType.Hit),

                new ValueBasedStrategyCell(7, CardRank.Two, DecisionType.Hit),
                new ValueBasedStrategyCell(7, CardRank.Three, DecisionType.Hit),
                new ValueBasedStrategyCell(7, CardRank.Four, DecisionType.Hit),
                new ValueBasedStrategyCell(7, CardRank.Five, DecisionType.Hit),
                new ValueBasedStrategyCell(7, CardRank.Six, DecisionType.Hit),
                new ValueBasedStrategyCell(7, CardRank.Seven, DecisionType.Hit),
                new ValueBasedStrategyCell(7, CardRank.Eight, DecisionType.Hit),
                new ValueBasedStrategyCell(7, CardRank.Nine, DecisionType.Hit),
                new ValueBasedStrategyCell(7, CardRank.Ten, DecisionType.Hit),
                new ValueBasedStrategyCell(7, CardRank.Jack, DecisionType.Hit),
                new ValueBasedStrategyCell(7, CardRank.Queen, DecisionType.Hit),
                new ValueBasedStrategyCell(7, CardRank.King, DecisionType.Hit),
                new ValueBasedStrategyCell(7, CardRank.Ace, DecisionType.Hit),

                new ValueBasedStrategyCell(7, CardRank.Two, DecisionType.Hit),
                new ValueBasedStrategyCell(7, CardRank.Three, DecisionType.Hit),
                new ValueBasedStrategyCell(7, CardRank.Four, DecisionType.Hit),
                new ValueBasedStrategyCell(7, CardRank.Five, DecisionType.Hit),
                new ValueBasedStrategyCell(7, CardRank.Six, DecisionType.Hit),
                new ValueBasedStrategyCell(7, CardRank.Seven, DecisionType.Hit),
                new ValueBasedStrategyCell(7, CardRank.Eight, DecisionType.Hit),
                new ValueBasedStrategyCell(7, CardRank.Nine, DecisionType.Hit),
                new ValueBasedStrategyCell(7, CardRank.Ten, DecisionType.Hit),
                new ValueBasedStrategyCell(7, CardRank.Jack, DecisionType.Hit),
                new ValueBasedStrategyCell(7, CardRank.Queen, DecisionType.Hit),
                new ValueBasedStrategyCell(7, CardRank.King, DecisionType.Hit),
                new ValueBasedStrategyCell(7, CardRank.Ace, DecisionType.Hit),

                new ValueBasedStrategyCell(8, CardRank.Two, DecisionType.Hit),
                new ValueBasedStrategyCell(8, CardRank.Three, DecisionType.Hit),
                new ValueBasedStrategyCell(8, CardRank.Four, DecisionType.Hit),
                new ValueBasedStrategyCell(8, CardRank.Five, DecisionType.Hit),
                new ValueBasedStrategyCell(8, CardRank.Six, DecisionType.Hit),
                new ValueBasedStrategyCell(8, CardRank.Seven, DecisionType.Hit),
                new ValueBasedStrategyCell(8, CardRank.Eight, DecisionType.Hit),
                new ValueBasedStrategyCell(8, CardRank.Nine, DecisionType.Hit),
                new ValueBasedStrategyCell(8, CardRank.Ten, DecisionType.Hit),
                new ValueBasedStrategyCell(8, CardRank.Jack, DecisionType.Hit),
                new ValueBasedStrategyCell(8, CardRank.Queen, DecisionType.Hit),
                new ValueBasedStrategyCell(8, CardRank.King, DecisionType.Hit),
                new ValueBasedStrategyCell(8, CardRank.Ace, DecisionType.Hit),
                
                new ValueBasedStrategyCell(9, CardRank.Two, DecisionType.Hit),
                new ValueBasedStrategyCell(9, CardRank.Three, DecisionType.DoubleDown),
                new ValueBasedStrategyCell(9, CardRank.Four, DecisionType.DoubleDown),
                new ValueBasedStrategyCell(9, CardRank.Five, DecisionType.DoubleDown),
                new ValueBasedStrategyCell(9, CardRank.Six, DecisionType.DoubleDown),
                new ValueBasedStrategyCell(9, CardRank.Seven, DecisionType.Hit),
                new ValueBasedStrategyCell(9, CardRank.Eight, DecisionType.Hit),
                new ValueBasedStrategyCell(9, CardRank.Nine, DecisionType.Hit),
                new ValueBasedStrategyCell(9, CardRank.Ten, DecisionType.Hit),
                new ValueBasedStrategyCell(9, CardRank.Jack, DecisionType.Hit),
                new ValueBasedStrategyCell(9, CardRank.Queen, DecisionType.Hit),
                new ValueBasedStrategyCell(9, CardRank.King, DecisionType.Hit),
                new ValueBasedStrategyCell(9, CardRank.Ace, DecisionType.Hit),
            
                new ValueBasedStrategyCell(10, CardRank.Two, DecisionType.DoubleDown),
                new ValueBasedStrategyCell(10, CardRank.Three, DecisionType.DoubleDown),
                new ValueBasedStrategyCell(10, CardRank.Four, DecisionType.DoubleDown),
                new ValueBasedStrategyCell(10, CardRank.Five, DecisionType.DoubleDown),
                new ValueBasedStrategyCell(10, CardRank.Six, DecisionType.DoubleDown),
                new ValueBasedStrategyCell(10, CardRank.Seven, DecisionType.DoubleDown),
                new ValueBasedStrategyCell(10, CardRank.Eight, DecisionType.DoubleDown),
                new ValueBasedStrategyCell(10, CardRank.Nine, DecisionType.DoubleDown),
                new ValueBasedStrategyCell(10, CardRank.Ten, DecisionType.Hit),
                new ValueBasedStrategyCell(10, CardRank.Jack, DecisionType.Hit),
                new ValueBasedStrategyCell(10, CardRank.Queen, DecisionType.Hit),
                new ValueBasedStrategyCell(10, CardRank.King, DecisionType.Hit),
                new ValueBasedStrategyCell(10, CardRank.Ace, DecisionType.Hit),

                new ValueBasedStrategyCell(11, CardRank.Two, DecisionType.DoubleDown),
                new ValueBasedStrategyCell(11, CardRank.Three, DecisionType.DoubleDown),
                new ValueBasedStrategyCell(11, CardRank.Four, DecisionType.DoubleDown),
                new ValueBasedStrategyCell(11, CardRank.Five, DecisionType.DoubleDown),
                new ValueBasedStrategyCell(11, CardRank.Six, DecisionType.DoubleDown),
                new ValueBasedStrategyCell(11, CardRank.Seven, DecisionType.DoubleDown),
                new ValueBasedStrategyCell(11, CardRank.Eight, DecisionType.DoubleDown),
                new ValueBasedStrategyCell(11, CardRank.Nine, DecisionType.DoubleDown),
                new ValueBasedStrategyCell(11, CardRank.Ten, DecisionType.DoubleDown),
                new ValueBasedStrategyCell(11, CardRank.Jack, DecisionType.DoubleDown),
                new ValueBasedStrategyCell(11, CardRank.Queen, DecisionType.DoubleDown),
                new ValueBasedStrategyCell(11, CardRank.King, DecisionType.DoubleDown),
                new ValueBasedStrategyCell(11, CardRank.Ace, DecisionType.Hit),
                
                new ValueBasedStrategyCell(12, CardRank.Two, DecisionType.Hit),
                new ValueBasedStrategyCell(12, CardRank.Three, DecisionType.Hit),
                new ValueBasedStrategyCell(12, CardRank.Four, DecisionType.Stand),
                new ValueBasedStrategyCell(12, CardRank.Five, DecisionType.Stand),
                new ValueBasedStrategyCell(12, CardRank.Six, DecisionType.Stand),
                new ValueBasedStrategyCell(12, CardRank.Seven, DecisionType.Hit),
                new ValueBasedStrategyCell(12, CardRank.Eight, DecisionType.Hit),
                new ValueBasedStrategyCell(12, CardRank.Nine, DecisionType.Hit),
                new ValueBasedStrategyCell(12, CardRank.Ten, DecisionType.Hit),
                new ValueBasedStrategyCell(12, CardRank.Jack, DecisionType.Hit),
                new ValueBasedStrategyCell(12, CardRank.Queen, DecisionType.Hit),
                new ValueBasedStrategyCell(12, CardRank.King, DecisionType.Hit),
                new ValueBasedStrategyCell(12, CardRank.Ace, DecisionType.Hit),

                new ValueBasedStrategyCell(13, CardRank.Two, DecisionType.Stand),
                new ValueBasedStrategyCell(13, CardRank.Three, DecisionType.Stand),
                new ValueBasedStrategyCell(13, CardRank.Four, DecisionType.Stand),
                new ValueBasedStrategyCell(13, CardRank.Five, DecisionType.Stand),
                new ValueBasedStrategyCell(13, CardRank.Six, DecisionType.Stand),
                new ValueBasedStrategyCell(13, CardRank.Seven, DecisionType.Hit),
                new ValueBasedStrategyCell(13, CardRank.Eight, DecisionType.Hit),
                new ValueBasedStrategyCell(13, CardRank.Nine, DecisionType.Hit),
                new ValueBasedStrategyCell(13, CardRank.Ten, DecisionType.Hit),
                new ValueBasedStrategyCell(13, CardRank.Jack, DecisionType.Hit),
                new ValueBasedStrategyCell(13, CardRank.Queen, DecisionType.Hit),
                new ValueBasedStrategyCell(13, CardRank.King, DecisionType.Hit),
                new ValueBasedStrategyCell(13, CardRank.Ace, DecisionType.Hit),
                
                new ValueBasedStrategyCell(14, CardRank.Two, DecisionType.Stand),
                new ValueBasedStrategyCell(14, CardRank.Three, DecisionType.Stand),
                new ValueBasedStrategyCell(14, CardRank.Four, DecisionType.Stand),
                new ValueBasedStrategyCell(14, CardRank.Five, DecisionType.Stand),
                new ValueBasedStrategyCell(14, CardRank.Six, DecisionType.Stand),
                new ValueBasedStrategyCell(14, CardRank.Seven, DecisionType.Hit),
                new ValueBasedStrategyCell(14, CardRank.Eight, DecisionType.Hit),
                new ValueBasedStrategyCell(14, CardRank.Nine, DecisionType.Hit),
                new ValueBasedStrategyCell(14, CardRank.Ten, DecisionType.Hit),
                new ValueBasedStrategyCell(14, CardRank.Jack, DecisionType.Hit),
                new ValueBasedStrategyCell(14, CardRank.Queen, DecisionType.Hit),
                new ValueBasedStrategyCell(14, CardRank.King, DecisionType.Hit),
                new ValueBasedStrategyCell(14, CardRank.Ace, DecisionType.Hit),
                
                new ValueBasedStrategyCell(15, CardRank.Two, DecisionType.Stand),
                new ValueBasedStrategyCell(15, CardRank.Three, DecisionType.Stand),
                new ValueBasedStrategyCell(15, CardRank.Four, DecisionType.Stand),
                new ValueBasedStrategyCell(15, CardRank.Five, DecisionType.Stand),
                new ValueBasedStrategyCell(15, CardRank.Six, DecisionType.Stand),
                new ValueBasedStrategyCell(15, CardRank.Seven, DecisionType.Hit),
                new ValueBasedStrategyCell(15, CardRank.Eight, DecisionType.Hit),
                new ValueBasedStrategyCell(15, CardRank.Nine, DecisionType.Hit),
                new ValueBasedStrategyCell(15, CardRank.Ten, DecisionType.Hit),
                new ValueBasedStrategyCell(15, CardRank.Jack, DecisionType.Hit),
                new ValueBasedStrategyCell(15, CardRank.Queen, DecisionType.Hit),
                new ValueBasedStrategyCell(15, CardRank.King, DecisionType.Hit),
                new ValueBasedStrategyCell(15, CardRank.Ace, DecisionType.Hit),
                
                new ValueBasedStrategyCell(16, CardRank.Two, DecisionType.Stand),
                new ValueBasedStrategyCell(16, CardRank.Three, DecisionType.Stand),
                new ValueBasedStrategyCell(16, CardRank.Four, DecisionType.Stand),
                new ValueBasedStrategyCell(16, CardRank.Five, DecisionType.Stand),
                new ValueBasedStrategyCell(16, CardRank.Six, DecisionType.Stand),
                new ValueBasedStrategyCell(16, CardRank.Seven, DecisionType.Hit),
                new ValueBasedStrategyCell(16, CardRank.Eight, DecisionType.Hit),
                new ValueBasedStrategyCell(16, CardRank.Nine, DecisionType.Hit),
                new ValueBasedStrategyCell(16, CardRank.Ten, DecisionType.Hit),
                new ValueBasedStrategyCell(16, CardRank.Jack, DecisionType.Hit),
                new ValueBasedStrategyCell(16, CardRank.Queen, DecisionType.Hit),
                new ValueBasedStrategyCell(16, CardRank.King, DecisionType.Hit),
                new ValueBasedStrategyCell(16, CardRank.Ace, DecisionType.Hit),
                
                new ValueBasedStrategyCell(17, CardRank.Two, DecisionType.Stand),
                new ValueBasedStrategyCell(17, CardRank.Three, DecisionType.Stand),
                new ValueBasedStrategyCell(17, CardRank.Four, DecisionType.Stand),
                new ValueBasedStrategyCell(17, CardRank.Five, DecisionType.Stand),
                new ValueBasedStrategyCell(17, CardRank.Six, DecisionType.Stand),
                new ValueBasedStrategyCell(17, CardRank.Seven, DecisionType.Stand),
                new ValueBasedStrategyCell(17, CardRank.Eight, DecisionType.Stand),
                new ValueBasedStrategyCell(17, CardRank.Nine, DecisionType.Stand),
                new ValueBasedStrategyCell(17, CardRank.Ten, DecisionType.Stand),
                new ValueBasedStrategyCell(17, CardRank.Jack, DecisionType.Stand),
                new ValueBasedStrategyCell(17, CardRank.Queen, DecisionType.Stand),
                new ValueBasedStrategyCell(17, CardRank.King, DecisionType.Stand),
                new ValueBasedStrategyCell(17, CardRank.Ace, DecisionType.Stand),
                
                new ValueBasedStrategyCell(18, CardRank.Two, DecisionType.Stand),
                new ValueBasedStrategyCell(18, CardRank.Three, DecisionType.Stand),
                new ValueBasedStrategyCell(18, CardRank.Four, DecisionType.Stand),
                new ValueBasedStrategyCell(18, CardRank.Five, DecisionType.Stand),
                new ValueBasedStrategyCell(18, CardRank.Six, DecisionType.Stand),
                new ValueBasedStrategyCell(18, CardRank.Seven, DecisionType.Stand),
                new ValueBasedStrategyCell(18, CardRank.Eight, DecisionType.Stand),
                new ValueBasedStrategyCell(18, CardRank.Nine, DecisionType.Stand),
                new ValueBasedStrategyCell(18, CardRank.Ten, DecisionType.Stand),
                new ValueBasedStrategyCell(18, CardRank.Jack, DecisionType.Stand),
                new ValueBasedStrategyCell(18, CardRank.Queen, DecisionType.Stand),
                new ValueBasedStrategyCell(18, CardRank.King, DecisionType.Stand),
                new ValueBasedStrategyCell(18, CardRank.Ace, DecisionType.Stand),
                
                new ValueBasedStrategyCell(19, CardRank.Two, DecisionType.Stand),
                new ValueBasedStrategyCell(19, CardRank.Three, DecisionType.Stand),
                new ValueBasedStrategyCell(19, CardRank.Four, DecisionType.Stand),
                new ValueBasedStrategyCell(19, CardRank.Five, DecisionType.Stand),
                new ValueBasedStrategyCell(19, CardRank.Six, DecisionType.Stand),
                new ValueBasedStrategyCell(19, CardRank.Seven, DecisionType.Stand),
                new ValueBasedStrategyCell(19, CardRank.Eight, DecisionType.Stand),
                new ValueBasedStrategyCell(19, CardRank.Nine, DecisionType.Stand),
                new ValueBasedStrategyCell(19, CardRank.Ten, DecisionType.Stand),
                new ValueBasedStrategyCell(19, CardRank.Jack, DecisionType.Stand),
                new ValueBasedStrategyCell(19, CardRank.Queen, DecisionType.Stand),
                new ValueBasedStrategyCell(19, CardRank.King, DecisionType.Stand),
                new ValueBasedStrategyCell(19, CardRank.Ace, DecisionType.Stand),
                
                new ValueBasedStrategyCell(20, CardRank.Two, DecisionType.Stand),
                new ValueBasedStrategyCell(20, CardRank.Three, DecisionType.Stand),
                new ValueBasedStrategyCell(20, CardRank.Four, DecisionType.Stand),
                new ValueBasedStrategyCell(20, CardRank.Five, DecisionType.Stand),
                new ValueBasedStrategyCell(20, CardRank.Six, DecisionType.Stand),
                new ValueBasedStrategyCell(20, CardRank.Seven, DecisionType.Stand),
                new ValueBasedStrategyCell(20, CardRank.Eight, DecisionType.Stand),
                new ValueBasedStrategyCell(20, CardRank.Nine, DecisionType.Stand),
                new ValueBasedStrategyCell(20, CardRank.Ten, DecisionType.Stand),
                new ValueBasedStrategyCell(20, CardRank.Jack, DecisionType.Stand),
                new ValueBasedStrategyCell(20, CardRank.Queen, DecisionType.Stand),
                new ValueBasedStrategyCell(20, CardRank.King, DecisionType.Stand),
                new ValueBasedStrategyCell(20, CardRank.Ace, DecisionType.Stand),
               
                new ValueBasedStrategyCell(21, CardRank.Two, DecisionType.Stand),
                new ValueBasedStrategyCell(21, CardRank.Three, DecisionType.Stand),
                new ValueBasedStrategyCell(21, CardRank.Four, DecisionType.Stand),
                new ValueBasedStrategyCell(21, CardRank.Five, DecisionType.Stand),
                new ValueBasedStrategyCell(21, CardRank.Six, DecisionType.Stand),
                new ValueBasedStrategyCell(21, CardRank.Seven, DecisionType.Stand),
                new ValueBasedStrategyCell(21, CardRank.Eight, DecisionType.Stand),
                new ValueBasedStrategyCell(21, CardRank.Nine, DecisionType.Stand),
                new ValueBasedStrategyCell(21, CardRank.Ten, DecisionType.Stand),
                new ValueBasedStrategyCell(21, CardRank.Jack, DecisionType.Stand),
                new ValueBasedStrategyCell(21, CardRank.Queen, DecisionType.Stand),
                new ValueBasedStrategyCell(21, CardRank.King, DecisionType.Stand),
                new ValueBasedStrategyCell(21, CardRank.Ace, DecisionType.Stand)
            };

            //ExecutionTimeManager.RecordExecutionTime(String.Format("{0}.{1}()",
            //        MethodInfo.GetCurrentMethod().DeclaringType.Name,
            //        MethodInfo.GetCurrentMethod().Name), start);

            return strategyCells;

        }
    }
}
