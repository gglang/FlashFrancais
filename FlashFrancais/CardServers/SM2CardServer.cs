

using System;
using System.Collections.Generic;
/**
* Copyright (C) 2011 Jeremy Wall <jeremy@marzhillstudios.com>
* All contents Licensed under the terms of the Apache License 2.0
* http://www.apache.org/licenses/LICENSE-2.0.html
*//**
* An implementation of the Spaced Repetition algorithm.
*
* The algorithm is inspired by the SuperMemo 2 algorithm:
* http://www.supermemo.com/english/ol/sm2.htm
*
* @author Jeremy Wall <jeremy@marzhillstudios.com>
*
*/
namespace FlashFrancais.CardServers
{
    public class SM2CardServer : CardServer
    {
        /*
        // TODO(jwall): Make these configurable in an advanced settings pane?
        public static final int MAX_QUALITY = 5;
        public static final int QUALITY_SUBTRACTOR = 5;
        public static final float E_FACTOR_FLOOR = 1.3f;
        public static final int INTERVAL_FLOOR = 1;

    
         // Score a Card and set it's correct E-Factor.
        public static void scoreCard(Card card, int quality)
        {
            float qFactor = (QUALITY_SUBTRACTOR - quality);
            float newFactor = card.getEFactor() + (0.1f - qFactor * (0.08f + qFactor * 0.02f));
            if (newFactor < E_FACTOR_FLOOR)
            {
                newFactor = E_FACTOR_FLOOR;
            }
            card.setEFactor(newFactor);
        }

        public static void calculateInterval(Card card)
        {
            int interval = card.getInterval();
            if (card.getEFactor() < 3f)
            {
                card.setCount(1);
            }
            int count = card.getCount();
            if (count == 2)
            {
                interval = 6;
            }
            else if (count > 2)
            {
                interval = Math.round(card.getInterval() * card.getEFactor());
            }
            card.setInterval(interval);
        }

        public static void scoreCardAndCalculateInterval(Card card, int quality)
        {
            scoreCard(card, quality);
            calculateInterval(card);
        }*/

        // TODO Const vs not static final?
        private const float intervalModifier = 1f;
        private const float newInterval = 0f;
        private const float easyIntervalModifier = 1.3f;

        public override Card GetNextCard()
        {
            IList<Tuple<Card, float>> cardToIntervalMapping = GetCardToIntervalMapping();

            // Select card with shortest interval and serve it?
            return null;
        }

        private IList<Tuple<Card, float>> GetCardToIntervalMapping()
        {
            List<Tuple<Card, float>> cardsWithIntervals = new List<Tuple<Card, float>>();
            foreach (Card card in _cards)
            {
                float cardInterval = CalculateCardInterval(card);
                var cardIntervalTuple = new Tuple<Card, float>(card, cardInterval);
                cardsWithIntervals.Add(cardIntervalTuple);
            }
            return cardsWithIntervals;
        }

        private float CalculateCardInterval(Card card)
        {
            return -1f;
        }
    }
}