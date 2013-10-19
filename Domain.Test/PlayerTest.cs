using NUnit.Framework;
using System;

namespace Domain.Test
{
    [TestFixture]
    public class PlayerTest
    {
        [Test]
        public void CannotBustFromSoftAceTest()
        {
            Console.WriteLine("***Begin cannotBustFromSoftAceTest***");
            Card ace = new Card(CardRank.Ace, CardSuit.Clubs);
            Card six = new Card(CardRank.Six, CardSuit.Clubs);
            Card secondAce = new Card(CardRank.Ace, CardSuit.Diamonds);

            Player dealerPlayer = new DealerPlayer();

            dealerPlayer.ReceiveCard(ace);
            dealerPlayer.ReceiveCard(six);
            dealerPlayer.ReceiveCard(secondAce);

            Assert.AreEqual(18, dealerPlayer.Hand.Value);

            Console.WriteLine(dealerPlayer.Hand[0] + " value of " + dealerPlayer.Hand[0].TrueValue);
            Console.WriteLine(dealerPlayer.Hand[1] + " value of " + dealerPlayer.Hand[1].TrueValue);
            Console.WriteLine(dealerPlayer.Hand[2] + " value of " + dealerPlayer.Hand[2].TrueValue);
            Console.WriteLine("***End cannotBustFromSoftAceTest***");
        }

        [Test]
        public void HasBlackJackWorksCorrectly()
        {
            Card ace = new Card(CardRank.Ace, CardSuit.Clubs);
            Card ten = new Card(CardRank.Ten, CardSuit.Clubs);
            Card six = new Card(CardRank.Six, CardSuit.Clubs);
            Card five = new Card(CardRank.Five, CardSuit.Clubs);

            Player player1 = new DealerPlayer();
            player1.ReceiveCard(ace);
            player1.ReceiveCard(ten);

            Assert.IsTrue(player1.HasBlackJack);

            player1.FlushHand();

            Assert.AreEqual(0, player1.Hand.Count);

            player1.ReceiveCard(ten);
            player1.ReceiveCard(six);
            player1.ReceiveCard(five);

            Assert.IsFalse(player1.HasBlackJack);
        }
    }
}
