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
        public string Front { get; set; }
        public string Back { get; set; }

        public IList<CardHistoryEntry> HistoryEntries { get; }

        public Card(string front, string back)
        {
            HistoryEntries = new List<CardHistoryEntry>();
            Front = front;
            Back = back;
        }

        private void AddHistoryEntry(CardHistoryEntry entry) // TODO Violating SRP? Move history management to another class?
        {
            HistoryEntries.Add(entry);
        }

        public void AddHistoryEntry(bool success)
        {
            AddHistoryEntry(new CardHistoryEntry(DateTime.Now, success));
        }
    }
}
