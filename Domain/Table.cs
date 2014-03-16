using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace Domain
{
    public class Table
    {
        private IList<IPlayer> _players;        
        private Deck _deck;
        private readonly ILog Log = LogManager.GetLogger(typeof(Table));
        private List<Card> _discardedCards;        

        /// <summary>
        /// Returns a singleton instance of Table
        /// </summary>
        /// <returns>singleton instance</returns>
        public static Table Instance
        {
            get { return Singleton<Table>.GetInstance(delegate { return new Table(); }); }
        }

        private Table()
        {            
            _players = new List<IPlayer>();
            _deck = new Deck();
            _discardedCards = new List<Card>();
        }

        public void AddPlayer(IPlayer player)
        {
            _players.Add(player);

          //  Log.InfoFormat("Added {0} to the table.", player);
        }

        public void RemovePlayer(Player player)
        {
            _players.Remove(player);

         //   Log.InfoFormat("Removed {0} from the table.", player);
        }

        public IList<IPlayer> Players
        {
            get { return _players; }
        }

        public Deck Deck
        {
            get { return _deck; }
        }

        public List<Card> DiscardedCards
        {
            get { return _discardedCards; }
            set { _discardedCards = value; }
        }
    }
}
