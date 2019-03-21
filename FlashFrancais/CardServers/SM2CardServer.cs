

using System;
using System.Collections.Generic;
using System.Linq;

namespace FlashFrancais.CardServers
{
    public class SM2CardServer : CardServer
    {
        // TODO Const vs not static final?
        private const float intervalModifier = 1f; // m
        private const float newInterval = 0f; // m0
        private const float easyIntervalModifier = 1.3f; // m4

        private const int _maxFreshCards = 30;
        private const float _amountOfDaysForStale = 3;

        private AnkiCardIntervalData _previousCardIntervalData = null;
        private IList<AnkiCardIntervalData> _activeCards = null;
        private IList<AnkiCardIntervalData> _inactiveCards = null;

        public override void RecordPreviousTrial(TrialPerformance trialPerformance)
        {
            if (_previousCardIntervalData == null)
            {
                throw new InvalidOperationException("Tried to record trial data without first getting a card. GetNextCard first!");
            }

            _previousCardIntervalData = CalculateCardIntervalData(_previousCardIntervalData.card); // HACK HACK FIXME TODO does this even work?

            int insertAt = 0;
            for (int i = 0; i < _activeCards.Count; i++)
            {
                insertAt = i + 1; // TODO FIXME This is a bug that nicely allows us to not see failed cards twice in a row but schedules things off by one
                if (_previousCardIntervalData.interval >= _activeCards[i].interval)
                {
                    continue; 
                } else
                {
                    break;
                }
            }

            _activeCards.Insert(insertAt, _previousCardIntervalData);
        }

        public override Card GetNextCard(TrialPerformance trialPerformance)
        {
            if (_inactiveCards == null || _activeCards == null)
            {
                InitCardData();
            }

            if (_activeCards.Count <= 0 && _inactiveCards.Count <= 0)
            {
                return null;
            }

            if (_previousCardIntervalData != null)
            {
                RecordPreviousTrial(trialPerformance);
            }

            if (ShouldActivateNewCard())
            {
                ActivateNewCard();
            }

            Card nextCard = _activeCards[0].card;
            _previousCardIntervalData = _activeCards[0];
            _activeCards.Remove(_previousCardIntervalData); // TODO slow?
            return nextCard;
        }

        private bool ShouldActivateNewCard()
        {
            if(_activeCards.Count < _maxFreshCards)
            {
                return true;
            }

            int freshCardCount = 0;
            foreach (AnkiCardIntervalData cardData in _activeCards)
            {
                if (cardData.interval < _amountOfDaysForStale)
                {
                    freshCardCount++;
                }
            }

            if (freshCardCount < _maxFreshCards)
            {
                return true;
            } else
            {
                return false;
            }
        }

        private void ActivateNewCard()
        {
            if (_inactiveCards == null || _inactiveCards.Count == 0)
            {
                return;
            }

            _activeCards.Insert(0, _inactiveCards[0]);
            _inactiveCards.RemoveAt(0);
        }

        private void InitCardData()
        {
            List<AnkiCardIntervalData> cardsWithIntervals = new List<AnkiCardIntervalData>();
            foreach (Card c in _cards)
            {
                AnkiCardIntervalData cardIntervalData = CalculateCardIntervalData(c);
                cardsWithIntervals.Add(cardIntervalData);
            }
            cardsWithIntervals.OrderBy(x => x.interval);
            _inactiveCards = cardsWithIntervals;
            _activeCards = new List<AnkiCardIntervalData>();

            for(int i = _inactiveCards.Count - 1; i >= 0; i--)
            {
                if(_inactiveCards[i].interval > 0)
                {
                    _activeCards.Add(_inactiveCards[i]);
                    _inactiveCards.RemoveAt(i);
                }
            }
        }

        private AnkiCardIntervalData CalculateCardIntervalData(Card c)
        {
            IEnumerable<CardHistoryEntry> history = c.HistoryEntries;
            history = history.OrderBy(x => x.EntryTime);
            double uf = 0;
            double interval = 0;
            CardHistoryEntry lastHistoryEntry = null;
            foreach(CardHistoryEntry historyEntry in history)
            {
                if(lastHistoryEntry == null)
                {
                    interval = newInterval;
                    lastHistoryEntry = historyEntry;
                }

                switch(historyEntry.TrialPerformance)
                {
                    case TrialPerformance.Fail:
                        uf = CalculateFailFactor(uf);
                        interval = interval * newInterval;
                        break;
                    case TrialPerformance.Easy:
                        uf = CalculateEasyFactor(uf);
                        interval = CalculateEasyInterval(lastHistoryEntry, historyEntry, interval, uf);
                        break;
                    case TrialPerformance.Normal:
                        uf = CalculateNormalFactor(uf);
                        interval = CalculateNormalInterval(lastHistoryEntry, historyEntry, interval, uf);
                        break;
                    case TrialPerformance.Hard:
                        uf = CalculateHardFactor(uf);
                        interval = CalculateHardInterval(lastHistoryEntry, historyEntry, interval);
                        break;
                    default:
                        throw new InvalidDataBaseOperationException(String.Format("Unsupported TrialPerformance value in card history: {0}", historyEntry.TrialPerformance));
                }

                lastHistoryEntry = historyEntry;
            }

            var cardIntervalData = new AnkiCardIntervalData()
            {
                lastReview = lastHistoryEntry?.EntryTime,
                card = c,
                interval = interval,
                understandingFactor = uf
            };

            return cardIntervalData;
        }

        private double CalculateReviewDelay(TimeSpan lastReview, TimeSpan currentReview, double desiredInterval)
        {
            double actualInterval = currentReview.TotalDays - lastReview.TotalDays;
            double reviewDelay = desiredInterval >= actualInterval ? 0 : actualInterval - desiredInterval;
            return reviewDelay;
        }

        private double CalculateFailFactor(double currentFactor)
        {
            double understandingFactor = Math.Max(1300, currentFactor - 200);
            understandingFactor = understandingFactor < 0 ? 0 : currentFactor;
            return understandingFactor;
        }
        

        private double CalculateHardInterval(CardHistoryEntry lastHistoryEntry, CardHistoryEntry currentHistoryEntry, double lastInterval)
        {
            double reviewDelay = CalculateReviewDelay(lastHistoryEntry.EntryTime.TimeOfDay, currentHistoryEntry.EntryTime.TimeOfDay, lastInterval);
            return Math.Max(lastInterval + 1, 
                ((lastInterval + (reviewDelay / 4.0)) * 1.2 * intervalModifier));
        }

        private double CalculateHardFactor(double currentFactor)
        {
            double understandingFactor = Math.Max(1300, currentFactor - 150);
            understandingFactor = understandingFactor < 0 ? 0 : currentFactor;
            return understandingFactor;
        }

        private double CalculateNormalInterval(CardHistoryEntry lastHistoryEntry, CardHistoryEntry currentHistoryEntry, double lastInterval, double understandingFactor)
        {
            double reviewDelay = CalculateReviewDelay(lastHistoryEntry.EntryTime.TimeOfDay, currentHistoryEntry.EntryTime.TimeOfDay, lastInterval);
            return Math.Max(CalculateHardInterval(lastHistoryEntry, currentHistoryEntry, lastInterval) + 1, 
                ((lastInterval + (reviewDelay / 2.0)) * (understandingFactor / 1000.0) * intervalModifier));
        }

        private double CalculateNormalFactor(double currentFactor)
        {
            return currentFactor;
        }

        private double CalculateEasyInterval(CardHistoryEntry lastHistoryEntry, CardHistoryEntry currentHistoryEntry, double lastInterval, double understandingFactor)
        {
            double reviewDelay = CalculateReviewDelay(lastHistoryEntry.EntryTime.TimeOfDay, currentHistoryEntry.EntryTime.TimeOfDay, lastInterval);
            return Math.Max(CalculateNormalInterval(lastHistoryEntry, currentHistoryEntry, lastInterval, understandingFactor) + 1,
                ((lastInterval + reviewDelay) * (understandingFactor / 1000.0) * intervalModifier * easyIntervalModifier));
        }

        private double CalculateEasyFactor(double currentFactor)
        {
            double understandingFactor = Math.Max(1300, currentFactor + 150);
            understandingFactor = understandingFactor < 0 ? 0 : currentFactor;
            return understandingFactor;
        }

        private class AnkiCardIntervalData
        {
            public DateTime? lastReview { get; set; }
            public Card card { get; set; }
            public double interval { get; set; }
            public double understandingFactor { get; set; }
        }
    }
}