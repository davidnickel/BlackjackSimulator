using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Domain.Test
{
    [TestFixture]
    public class HandTest
    {
        [Test]
        public void AddAddsCard()
        {
            var card = new Card(CardRank.Eight, CardSuit.Clubs);

            var hand = new Hand();

            hand.Add(card);

            Assert.AreEqual(1, hand.Count);
            Assert.AreEqual(card, hand[0]);
            Assert.AreEqual(8, hand.Value);
        }

        [Test]
        public void ValueAddedCorrectly1()
        {
            var ten = new Card(CardRank.King, CardSuit.Clubs);
            var hand = new Hand();
            hand.Add(ten);

            Assert.AreEqual(10, hand.Value);

            var eight = new Card(CardRank.Eight, CardSuit.Clubs);
            hand.Add(eight);

            Assert.AreEqual(18, hand.Value);
            Assert.IsFalse(hand.ContainsSoftAce);

            var six = new Card(CardRank.Six, CardSuit.Clubs);
            hand.Add(six);
            Assert.AreEqual(24, hand.Value);
            Assert.IsFalse(hand.ContainsSoftAce);
        }

        [Test]
        public void ValueAddedCorrectly2()
        {
            var ten = new Card(CardRank.King, CardSuit.Clubs);
            var ace = new Card(CardRank.Ace, CardSuit.Clubs);

            var hand = new Hand();
            hand.Add(ten);
            hand.Add(ace);
            Assert.AreEqual(21, hand.Value);
            Assert.IsTrue(hand.ContainsSoftAce);
        }

        [Test]
        public void ValueAddedCorrectly3()
        {
            var ten = new Card(CardRank.King, CardSuit.Clubs);
            var ace = new Card(CardRank.Ace, CardSuit.Clubs);
            var ace2 = new Card(CardRank.Ace, CardSuit.Diamonds);

            var hand = new Hand();
            hand.Add(ten);
            hand.Add(ace);
            hand.Add(ace2);
            Assert.AreEqual(12, hand.Value);
            Assert.IsFalse(hand.ContainsSoftAce);
        }

        [Test]
        public void ValueAddedCorrectly4()
        {
            var ace = new Card(CardRank.Ace, CardSuit.Clubs);
            var hand = new Hand();
            hand.Add(ace);
            Assert.AreEqual(11, hand.Value);
            Assert.IsTrue(hand.ContainsSoftAce);
        }

        [Test]
        public void ValueAddedCorrectly5()
        {
            var ace = new Card(CardRank.Ace, CardSuit.Clubs);
            var ace2 = new Card(CardRank.Ace, CardSuit.Hearts);
            var ace3 = new Card(CardRank.Ace, CardSuit.Diamonds);

            var hand = new Hand();
            hand.Add(ace);
            hand.Add(ace2);
            hand.Add(ace3);
            Assert.AreEqual(13, hand.Value);
            Assert.IsTrue(hand.ContainsSoftAce);
        }

        [Test]
        public void TestClear()
        {
            var hand = new Hand();
            hand.Add(new Card(CardRank.Eight, CardSuit.Diamonds));
            hand.Add(new Card(CardRank.Ace, CardSuit.Diamonds));
            

            Assert.AreEqual(2, hand.Count);
            Assert.IsTrue(hand.ContainsSoftAce);
            hand.Clear();

            Assert.AreEqual(0, hand.Count);
            Assert.IsFalse(hand.ContainsSoftAce);
        }

        [Test]
        public void TestRemove()
        {
            var card = new Card(CardRank.Eight, CardSuit.Clubs);
            var hand = new Hand();
            hand.Add(card);

            hand.Remove(card);

            Assert.AreEqual(0, hand.Count);
            Assert.AreEqual(0, hand.Value);
            Assert.IsFalse(hand.ContainsSoftAce);
        }

        [Test]
        public void TestRemove2()
        {
            var seven = new Card(CardRank.Seven, CardSuit.Clubs);
            var eight = new Card(CardRank.Eight, CardSuit.Clubs);
            var hand = new Hand();
            hand.Add(seven);
            hand.Add(eight);

            hand.Remove(eight);

            Assert.AreEqual(1, hand.Count);
            Assert.AreEqual(7, hand.Value);
            Assert.IsFalse(hand.ContainsSoftAce);
        }

        [Test]
        public void TestRemove3()
        {
            var seven = new Card(CardRank.Seven, CardSuit.Clubs);
            var eight = new Card(CardRank.Eight, CardSuit.Clubs);
            var ace = new Card(CardRank.Ace, CardSuit.Clubs);

            var hand = new Hand();
            hand.Add(seven);
            hand.Add(eight);
            hand.Add(ace);

            hand.Remove(ace);

            Assert.AreEqual(2, hand.Count);
            Assert.AreEqual(15, hand.Value);
            Assert.IsFalse(hand.ContainsSoftAce);
        }

        [Test]
        public void TestRemove4()
        {
            var seven = new Card(CardRank.Seven, CardSuit.Clubs);
            var eight = new Card(CardRank.Eight, CardSuit.Clubs);
            var ace = new Card(CardRank.Ace, CardSuit.Clubs);

            var hand = new Hand();
            hand.Add(seven);
            hand.Add(eight);
            hand.Add(ace);

            hand.Remove(eight);

            Assert.AreEqual(2, hand.Count);
            Assert.AreEqual(18, hand.Value);
            Assert.IsTrue(hand.ContainsSoftAce);

            hand.Remove(seven);

            Assert.AreEqual(1, hand.Count);
            Assert.AreEqual(11, hand.Value);
            Assert.IsTrue(hand.ContainsSoftAce);
        }

        [Test]
        public void TestRemove5()
        {
            var seven = new Card(CardRank.Seven, CardSuit.Clubs);
            var eight = new Card(CardRank.Eight, CardSuit.Clubs);
            var ace = new Card(CardRank.Ace, CardSuit.Clubs);
            var ace2 = new Card(CardRank.Ace, CardSuit.Clubs);

            var hand = new Hand();
            hand.Add(seven);
            hand.Add(eight);
            hand.Add(ace);
            hand.Add(ace2);

            hand.Remove(eight);
            hand.Remove(seven);

            Assert.AreEqual(2, hand.Count);
            Assert.AreEqual(12, hand.Value);
            Assert.IsTrue(hand.ContainsSoftAce);

            hand.Remove(ace2);

            Assert.AreEqual(1, hand.Count);
            Assert.AreEqual(11, hand.Value);
            Assert.IsTrue(hand.ContainsSoftAce);

            hand.Add(seven);
            hand.Add(eight);
            hand.Add(ace2);

            Assert.AreEqual(4, hand.Count);
            Assert.AreEqual(17, hand.Value);
            Assert.IsFalse(hand.ContainsSoftAce);
        }
    }
}
