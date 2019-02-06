using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace FlashFrancais
{
    public class FlashCard
    {
        public string Front { get; set; }
        public string Back { get; set; }

        public int successfulReadingCount { get; set; } // TODO Violating SRP?

        public FlashCard(string front, string back)
        {
            successfulReadingCount = 0;
            Front = front;
            Back = back;
        }

        public void ResetReadHistory()
        {
            successfulReadingCount = 0;
        }
    }
}
