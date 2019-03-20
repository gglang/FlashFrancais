using System;

namespace FlashFrancais
{
    public enum TrialPerformance
    {
        Fail = 0,
        Hard = 1,
        Normal = 2,
        Easy = 3
    }

    public class CardHistoryEntry
    {
        public CardHistoryEntry(DateTime entryTime, TrialPerformance trialPerformance)
        {
            EntryTime = entryTime;
            TrialPerformance = trialPerformance;
        }

        public DateTime EntryTime;
        public TrialPerformance TrialPerformance { get; set; }
    }
}
