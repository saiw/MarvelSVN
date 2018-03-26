using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ZeroRestAPI
{



    public class PriceStatus
    {
        public string status { get; set; }
        public string ch { get; set; }
        public long ts { get; set; }
        public Tick tick { get; set; }



    }

    public class Tick
    {
        public decimal amount { get; set; }
        public decimal open { get; set; }
        public decimal close { get; set; }
        public decimal high { get; set; }
        public long id { get; set; }
        public int count { get; set; }
        public decimal low { get; set; }
        public long version { get; set; }
        
        public decimal[] ask { get; set; }//public float[] ask { get; set; }
        public decimal vol { get; set; }
        
        public decimal[] bid { get; set; } //public float[] bid { get; set; }
    }


}
