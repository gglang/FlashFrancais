using System;

namespace FlashFrancais
{
    public class CardHistoryEntry
    {
        public CardHistoryEntry(DateTime entryTime, bool success)
        {
            EntryTime = entryTime;
            Success = success;
        }

        public DateTime EntryTime;
        public bool Success { get; set; }
    }
}
