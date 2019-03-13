using System.Collections.Generic;

namespace FlashFrancais.CardServers
{
    public abstract class CardServer
    {
        protected IList<Card> _cards;

        public void Init(IList<Card> cards)
        {
            _cards = cards;
        }

        public abstract Card GetNextCard();
    }
}
