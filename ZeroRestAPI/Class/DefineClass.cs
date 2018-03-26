using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ZeroRestAPI
{

    public abstract class AbstractRequest
    {
        public string Contect { get; private set; }
        public string URL { get; private set; }
        public string TokenCoin { get; private set; }
        public bool IsError = false;
        public string ErrorCode { get; private set; }

        private DateTime lastRevTime = DateTime.MinValue;
        public string LastRevTime
        {
            get { return lastRevTime.ToString("HH:mm:ss"); }
        }
        //private RestClient client;//http请求客户端
        private HttpWebRequest client;//http请求客户端

        public abstract void Init();

        public abstract Boolean SendRequest(string resourcePath, string parameters = "");

        public abstract string ToJsonFormat(string msg);
    }
    public interface IExcel
    {
        decimal GetClose();
    }

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
