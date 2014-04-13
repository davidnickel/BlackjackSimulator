using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

using System.Reflection;

namespace Domain
{
    public class Deck
    {
        private List<Card> _cards;
        private static readonly ILog Log = LogManager.GetLogger(typeof(Deck));
        
        //todo: create multiple decks, probably can still call it a Deck class or Shoe but its really still 1 list of cards regardless
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
            if (_cards.Count <= 0)
            {
                throw new NotSupportedException("Cannot draw card from empty deck.");
            }

            //todo: could we draw from bottom of deck to reduce array moves ?  Should we use a linked list instead ? 
            // We're never indexing anything but head or tail so really a linked list would work better no ?
            // false, shuffling indexes in the middle somewhere, run some analysis

            Card card = _cards[0];
            _cards.RemoveAt(0);

            return card;
        }

        public void Shuffle()
        {
           // Log.DebugFormat("Shuffing deck...");

            Random random = new Random();

            int fullDeckCount = _cards.Count;

            for (int i = fullDeckCount - 1; i > 0; i--)
            {
                int randomIndex = random.Next(i + 1);

                var tempCard = _cards[i];
                _cards[i] = _cards[randomIndex];
                _cards[randomIndex] = tempCard;
            }

            
        }       

        public List<Card> Cards
        {
            get { return _cards; }            
        }
    }
}
