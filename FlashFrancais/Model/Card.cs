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
        public int ID { get; set; }
        public string Front { get; set; }
        public string Back { get; set; }

        public IList<CardHistoryEntry> HistoryEntries { get; }

        public Card(string front, string back, IList<CardHistoryEntry> historyEntries = null, int id = -1)
        {
            HistoryEntries = historyEntries ?? new List<CardHistoryEntry>();
            ID = id;
            Front = front;
            Back = back;
        }

        public void AddHistoryEntry(TrialPerformance trialPerformance) // TODO Violating SRP? Move history management to another class?
        {
            var entry = new CardHistoryEntry(DateTime.Now, trialPerformance);
            HistoryEntries.Add(entry);
        }

        public override string ToString()
        {
            string result = string.Format("{0} ===== {1}", Front, Back);
            return result;
        }
    }
}
