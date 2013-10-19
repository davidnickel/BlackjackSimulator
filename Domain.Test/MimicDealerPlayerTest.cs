using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Domain.Test
{
    [TestFixture]
    public class MimicDealerPlayerTest
    {
        [Test]
        public void MimicDealerUsesDealerStrategy()
        {
            MimicDealerPlayer player = new MimicDealerPlayer();

            Assert.AreEqual(typeof(DealerDecisionMaker), player._decisionMaker.GetType());
        }

    }
}
