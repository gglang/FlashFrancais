using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashFrancais.CardServers
{
    public abstract class CardServer
    {
        public CardServer(FlashDeck deck)
        {

        }

        public abstract Card GetNextCard();
    }
}
