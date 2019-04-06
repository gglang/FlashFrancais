using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace FlashFrancais
{
    public class Card
    {
        public int ID { get; } = -1;
        public string Front { get; set; }
        public string Back { get; set; }

        public IList<CardHistoryEntry> HistoryEntries { get; }

        public Card(string front, string back)
        {
            HistoryEntries = new List<CardHistoryEntry>();
            Front = front;
            Back = back;
        }

        public Card(int id, string front, string back)
        {
            HistoryEntries = new List<CardHistoryEntry>();
            ID = id;
            Front = front;
            Back = back;
        }

        public void AddHistoryEntry(TrialPerformance trialPerformance) // TODO Violating SRP? Move history management to another class?
        {
            var entry = new CardHistoryEntry(DateTime.Now, trialPerformance);
            HistoryEntries.Add(entry);
            
        }
    }
}
