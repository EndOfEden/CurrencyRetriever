using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyRetriever
{
    public class RateResultBE
    {
        public string Base { get; set; }
        public string Date { get; set; }
        //public IEnumerable<IDictionary<string, string>> Rates { get; set; }
        public IDictionary<string, string> Rates { get; set; }
    }

    //public class Rate
    //{
    //    public string Curren
    //}
}
