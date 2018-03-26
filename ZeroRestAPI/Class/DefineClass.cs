using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ZeroRestAPI
{
    public abstract class AbstractRestful
    {

        #region Api配置信息
        /// <summary>
        /// API域名名称
        /// </summary>
        public string host = string.Empty;

        /// <summary>
        /// HOUST
        /// </summary>
        public string HOSTURL
        {
            get { return $"https://" + host; }  //https://api.huobipro.com/market
        }

        

        protected string contect = string.Empty;
        public string Contect { get { return contect; } }

        protected Boolean isError = false;
        public Boolean IsError { get { return isError; } }

        protected string errorCode = string.Empty;
        public string ErrorCode { get { return errorCode; } }

        protected string url = string.Empty;
        public string URL { get { return url; } }

        protected string tokenCoin = string.Empty;
        public string TokenCoin { get { return tokenCoin; } }

        protected virtual int TimeOut { get { return 5 * 1000; } } //預設五秒request斷現
        public bool IsTimeOutLimit { get { return true; } }

        protected DateTime lastRevTime = DateTime.MinValue;
        public string LastRevTime
        {
            get { return lastRevTime.ToString("HH:mm:ss"); }
        }
        #endregion

        protected HttpWebRequest client;//http请求客户端
        protected HttpWebRequest request;
        protected HttpWebResponse response;


        #region Method
        /// <summary>
        /// 僅提供Async使用
        /// </summary>
        /// <param name="resourcePath"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<bool> SendRequestAsync(string resourcePath, string parameters = "")
        {
            this.Clear(); //送出請求前參數reset
            tokenCoin = parameters;
            url = $"{HOSTURL}{resourcePath}?symbol={parameters}";  // ?GET /market/detail/merged?symbol=ethusdt */

            try
            {
                request = WebRequest.Create(this.URL) as HttpWebRequest;
                if (IsTimeOutLimit)
                    request.Timeout = TimeOut;
                response = await request.GetResponseAsync() as HttpWebResponse; ////request.Method = "GET";   

                string msg = string.Empty;
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default))
                {
                    msg = sr.ReadToEnd();
                }

                if (msg.Length < 1)
                    return false;

                //parse json
                FormatToObject(msg);

                lastRevTime = DateTime.Now;
            }
            catch (Exception ex)
            {
                errorCode = ex.Message;
                isError = true;
                return false;
            }
            return true;
        }


        

        ///// <summary>
        ///// 发起Http请求
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="resourcePath">market</param>
        ///// <param name="parameters"></param>
        ///// <returns></returns>
        public bool SendRequest(string resourcePath, string parameters = "")
        {
            this.Clear(); //送出請求前參數reset
            tokenCoin = parameters;
            url = $"{HOSTURL}{resourcePath}?symbol={parameters}";  // ?GET /market/detail/merged?symbol=ethusdt */

            try
            {
                request = WebRequest.Create(this.URL) as HttpWebRequest;
                if (IsTimeOutLimit)
                    request.Timeout = TimeOut;
                response = request.GetResponse() as HttpWebResponse; ////request.Method = "GET";   

                string msg = string.Empty;
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default))
                {
                    msg = sr.ReadToEnd();
                }

                if (msg.Length < 1)
                    return false;

                //parse json
                FormatToObject(msg);

                lastRevTime = DateTime.Now;
            }
            catch (Exception ex)
            {
                errorCode = ex.Message;
                isError = true;
                return false;
            }
            return true;
        }

        public string ToJsonFormat(string msg)
        {
            return JsonHelper.DeserializeToString(msg);
        }
        #endregion

        #region  Abastract Method
        public abstract decimal GetClose();
        public abstract decimal GetBid(int idx);
        public abstract decimal GetAsk(int idx);
        public abstract string GetErrorCode();

        public abstract void Init();
        public abstract void Clear();
        public abstract void FormatToObject(string msg);

        #endregion



    }

    #region  JsonFormat Object

    #region Houbi 
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
    #endregion

    #region KuCoin
    public class Rootobject
    {
        public bool success { get; set; }
        public string code { get; set; }
        public string msg { get; set; }
        public Data data { get; set; }
    }

    public class Data
    {
        public string coinType { get; set; }
        public bool trading { get; set; }
        public int lastDealPrice { get; set; }
        public int buy { get; set; }
        public int sell { get; set; }
        public string coinTypePair { get; set; }
        public int sort { get; set; }
        public float feeRate { get; set; }
        public int volValue { get; set; }
        public int high { get; set; }
        public long datetime { get; set; }
        public long vol { get; set; }
        public int low { get; set; }
        public float changeRate { get; set; }
    }

    #endregion

    #endregion
}
