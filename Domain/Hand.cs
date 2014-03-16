using System;
using System.Collections;
using System.Collections.Generic;

namespace Domain
{
    public class Hand : IEnumerable<Card>
    {
        private int _value;
        private int _aceCount;
        private bool _containsSoftAce;
        private readonly List<Card> _cards;
        
        public Hand()
        {
            _cards = new List<Card>();
            SetDefaults();
        }

        private void SetDefaults()
        {
            _value = 0;
            _aceCount = 0;
            _containsSoftAce = false;
            Status = HandStatusType.Active;
            Outcome = HandOutcomeType.InProgress;
        }

        public void Add(Card card)
        {
            _cards.Add(card);
            _value += card.Value;

            if (card.Rank == CardRank.Ace)
            {
                _aceCount ++;
            }

            if (_aceCount > 0)
            {
                if (_value < 12 && !_containsSoftAce)
                {
                    _containsSoftAce = true;
                    _value += 10;
                    return;
                }
                if (_value > 21 && _containsSoftAce)
                {
                    _containsSoftAce = false;
                    _value -= 10;
                    return;
                }
            }
        }

        public bool Remove(Card card)
        {
            if (!_cards.Remove(card))
            {
                return false;
            }
            _value -= card.Value;

            if (card.Rank == CardRank.Ace)
            {
                _aceCount --;
                if (_aceCount == 0)
                {
                    _containsSoftAce = false;
                }
            }

            if (_aceCount > 0)
            {
                if (_value < 12 && !_containsSoftAce)
                {
                    _containsSoftAce = true;
                    _value += 10;
                }
            }
            
            return true;
        }

        public void Clear()
        {
            _cards.Clear();
            SetDefaults();
        }

        public int Count
        {
            get { return _cards.Count; }
        }

        public int Value
        {
            get { return _value; }
        }
        

        public Card this[int key]
        {
            get { return _cards[key]; }
        }

        public IEnumerator<Card> GetEnumerator()
        {
            return _cards.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool ContainsSoftAce
        {
            get
            {
                return _containsSoftAce;
            }
        }

        //todo: remove this from Hand, hand should just contain a value, not do this logic 
        public bool HasBlackJack
        {
            get
            {
                return _value == 21 && Count == 2 && !IsSplitHand && _containsSoftAce;
            }
        }

        public HandStatusType Status
        {
            get;
            set;
        }

        public HandOutcomeType Outcome
        {
            get;
            set;
        }


        public bool IsSplitHand
        {
            get;
            set;
        }

        public override string ToString()
        {
            string result = String.Empty;

            foreach (var card in this)
            {
                result += card.ToString() + Environment.NewLine;
            }

            result += "Value: " + this.Value + Environment.NewLine;

            return result;
        }
        
    }
}
