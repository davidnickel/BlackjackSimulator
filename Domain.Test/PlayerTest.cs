using NUnit.Framework;
using System;

namespace Domain.Test
{
    [TestFixture]
    public class PlayerTest
    {
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

            Assert.AreEqual(true, player1.ActiveHand.HasBlackJack);
            Assert.AreEqual(21, player1.ActiveHand.Value);

            player1.ActiveHand.Clear();

            Assert.AreEqual(0, player1.ActiveHand.Count);

            player1.ReceiveCard(ten);
            player1.ReceiveCard(six);
            player1.ReceiveCard(five);

            Assert.AreNotEqual(true, player1.ActiveHand.HasBlackJack);
            Assert.AreEqual(21, player1.ActiveHand.Value);
        }

      
    }
}
