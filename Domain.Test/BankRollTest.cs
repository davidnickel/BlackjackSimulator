using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Domain.Test
{
    [TestFixture]
    public class BankRollTest
    {
        [Test]
        public void PlaceBetDecrements()
        {
            IBankRoll bank = new BankRoll(100.42m);
            bank.PlaceBet(10.02m);

            Assert.AreEqual(90.40m, bank.Value);
            Console.WriteLine(bank);
        }

        [Test]
        public void AddAdds()
        {
            IBankRoll bank = new BankRoll(100.42m);
            bank.Add(7.0m);

            Assert.AreEqual(107.42m, bank.Value);
            Console.WriteLine(bank);
        }

    }
}
