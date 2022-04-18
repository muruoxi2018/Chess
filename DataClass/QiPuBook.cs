using System;

namespace Chess.DataClass
{
    internal class QiPuBook
    {
        public int RowID { get; set; }
        public DateTime date { get; set; }
        public string type { get; set; }
        public string title { get; set; }
        public string author { get; set; }
        public string video { get; set; }
        public string memo { get; set; }
        public string Recode { get; set; }
    }
}
