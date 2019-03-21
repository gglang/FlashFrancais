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

        public abstract void RecordPreviousTrial(TrialPerformance trialPerformance);
        public abstract Card GetNextCard(TrialPerformance trialPerformance);
    }
}
