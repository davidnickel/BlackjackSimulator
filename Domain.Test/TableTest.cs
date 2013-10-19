using NUnit.Framework;

namespace Domain.Test
{
    [TestFixture]
    public class TableTest
    {
        private Table _table;        

        private void SetUp()
        {
            _table = Table.Instance;                     
        }

        [Test]
        public void TableIsSingleton()
        {
            Table table = Table.Instance;
            Table duplicateTable = Table.Instance;

            Assert.IsTrue(table == duplicateTable);
        }

        [Test]
        public void AddPlayerTest()
        {
            SetUp();

            _table.Players.Clear();
            _table.AddPlayer(new DealerPlayer());

            Assert.AreEqual(_table.Players.Count, 1);
        }

        [Test]
        public void RemovePlayerTest()
        {
            SetUp();

            IPlayer testPlayer = new DealerPlayer();
            _table.Players.Clear();
            _table.AddPlayer(testPlayer);
            _table.RemovePlayer((Player) testPlayer);

            Assert.AreEqual(_table.Players.Count, 0);
        }
    }
}
