using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Domain.Performance;
using System.Reflection;

namespace Domain
{
    public class Deck
    {
        private IList<Card> _cards;
        private static readonly ILog Log = LogManager.GetLogger(typeof(Deck));

        public Deck()
        {
            _cards = new List<Card>();

            foreach (string rank in Enum.GetNames(typeof(CardRank)))
            {
                foreach (string suit in Enum.GetNames(typeof(CardSuit)))
                {

                    Card card = new Card(EnumUtil<CardRank>.Parse(rank),
                        EnumUtil<CardSuit>.Parse(suit));

                    _cards.Add(card);
                }
            }
            
        }

        public Card Draw()
        {
            long start = DateTime.Now.Ticks;

            if (_cards.Count <= 0)
            {
                throw new NotSupportedException("Cannot draw card from empty deck.");
            }

            Card card = _cards[0];
            _cards.RemoveAt(0);

            ExecutionTimeManager.RecordExecutionTime(String.Format("{0}.{1}()",
               MethodInfo.GetCurrentMethod().DeclaringType.Name,
               MethodInfo.GetCurrentMethod().Name), start);

            return card;
        }

        public void Shuffle()
        {
            long start = DateTime.Now.Ticks;

            Log.DebugFormat("Shuffing deck...");

            IList<Card> shuffledDeck = new List<Card>();
            Random random = new Random();

            int fullDeckCount = _cards.Count;

            for (int i = 0; i < fullDeckCount; i++)
            {
                int randomIndex = random.Next(0, _cards.Count);

                //if (_cards[randomIndex].Rank == CardRank.Ace)
                //{
                //    _cards[randomIndex].TrueValue = 11;
                //}

                shuffledDeck.Add(_cards[randomIndex]);                
                _cards.RemoveAt(randomIndex);
            }

            _cards = shuffledDeck;

            ExecutionTimeManager.RecordExecutionTime(String.Format("{0}.{1}()",
               MethodInfo.GetCurrentMethod().DeclaringType.Name,
               MethodInfo.GetCurrentMethod().Name), start);
        }       

        public IList<Card> Cards
        {
            get { return _cards; }            
        }
    }
}
