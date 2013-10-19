using NUnit.Framework;

namespace Domain.Test
{
    [TestFixture]
    public class DealerDecisionMakerTest
    {
        private TableRules _rules;
        private DealerDecisionMaker _decisionMaker;        
        private Player _player;

        private void SetUp()
        {
            _rules = TableRules.Instance;            
            _decisionMaker = new DealerDecisionMaker(_rules);
            _player = new DealerPlayer();
        }

        [Test]
        public void LessThan17()
        {
            SetUp();

            Card ten = new Card(CardRank.Ten, CardSuit.Clubs);
            Card six = new Card(CardRank.Six, CardSuit.Clubs);

            _player.ReceiveCard(ten);
            _player.ReceiveCard(six);

            DecisionType decision;

            _rules.DealerHitsSoft17 = true;
            decision = _decisionMaker.DecidePlay(_player, new Card(CardRank.Two, CardSuit.Clubs));
            Assert.AreEqual(DecisionType.Hit, decision);

            _rules.DealerHitsSoft17 = false;
            decision = _decisionMaker.DecidePlay(_player, new Card(CardRank.Two, CardSuit.Clubs));
            Assert.AreEqual(DecisionType.Hit, decision);
        }

        [Test]
        public void Soft17()
        {
            SetUp();

            Card ace = new Card(CardRank.Ace, CardSuit.Clubs);
            Card six = new Card(CardRank.Six, CardSuit.Clubs);

            _player.ReceiveCard(ace);
            _player.ReceiveCard(six);

            DecisionType decision;

            _rules.DealerHitsSoft17 = true;
            decision = _decisionMaker.DecidePlay(_player, new Card(CardRank.Two, CardSuit.Clubs));
            Assert.AreEqual(DecisionType.Hit, decision);

            _rules.DealerHitsSoft17 = false;
            decision = _decisionMaker.DecidePlay(_player, new Card(CardRank.Two, CardSuit.Clubs));
            Assert.AreEqual(DecisionType.Stand, decision);
        }


        [Test]
        public void Hard17()
        {
            SetUp();

            Card ten = new Card(CardRank.Ten, CardSuit.Clubs);
            Card seven = new Card(CardRank.Seven, CardSuit.Clubs);

            _player.ReceiveCard(ten);
            _player.ReceiveCard(seven);

            DecisionType decision;

            _rules.DealerHitsSoft17 = true;
            decision = _decisionMaker.DecidePlay(_player, new Card(CardRank.Two, CardSuit.Clubs));
            Assert.AreEqual(DecisionType.Stand, decision);

            _rules.DealerHitsSoft17 = false;
            decision = _decisionMaker.DecidePlay(_player, new Card(CardRank.Two, CardSuit.Clubs));
            Assert.AreEqual(DecisionType.Stand, decision);
        }

    }
}
