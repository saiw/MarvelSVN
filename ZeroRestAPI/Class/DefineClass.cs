using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ZeroRestAPI
{
    public abstract class AbstractRestful:IDisposable
    {

        #region Api配置信息
        /// <summary>
        /// API域名名称
        /// </summary>
        //private string host = string.Empty;

        public string Host { get; set; }
        /// <summary>
        /// HOUST
        /// </summary>
        public string HOSTURL
        {
            get { return $"https://" + Host; }  //https://api.huobipro.com/market
        }

        public abstract string  URL { get; }

        //protected string tokenCoin = string.Empty;
        public string TokenCoin {  get; set; }

        //protected string path = string.Empty;
        public string ResourcePath { get; set; }

        public string ResourcePath2 { get; set; }


        protected string contect = string.Empty;
        public string Contect { get { return contect; } }

        protected Boolean isError = false;
        public Boolean IsError { get { return isError; } }

        protected string errorCode = string.Empty;
        public string ErrorCode { get { return errorCode; } }

        protected virtual int TimeOut { get { return 5 * 1000; } } //預設五秒request斷現
        public bool IsTimeOutLimit { get { return true; } }

        protected DateTime lastRevTime = DateTime.MinValue;
        public string LastRevTime
        {
            get { return lastRevTime.ToString("HH:mm:ss"); }
        }
        #endregion

        //protected HttpWebRequest client;//http请求客户端
        protected HttpWebRequest request;
        protected HttpWebResponse response;


        #region Method
        /// <summary>
        /// 僅提供Async使用
        /// </summary>
        /// <param name="resourcePath"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<bool> SendRequestAsync(string url)
        {
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
                Parse(msg);

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

        public bool SendRequest(string url )
        {
            try
            {
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ContentType = "application/json";
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.71 Safari/537.36";

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
                Parse(msg);

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

        /// <summary>
        ///  提供兩個url request 組成需要資料
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool SendCombineRequest(string url ,string sec_url  )
        {
            if (url == sec_url)
            {
                this.errorCode = "Double URL!";
                return false;
            } 
            
            string msg = string.Empty;

            try
            {
                request = WebRequest.Create(url) as HttpWebRequest;
                /*URL*/
                request.ContentType = "application/json";
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.71 Safari/537.36";
                if (IsTimeOutLimit)
                    request.Timeout = TimeOut;
                request.Method = "GET";
                response = request.GetResponse() as HttpWebResponse; ////request.Method = "GET";   
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default))
                {
                    msg = sr.ReadToEnd();
                }

                if (msg.Length < 1)
                    return false;   
                Parse(msg);  //parse json

                /* URL 2*/ 
                request = WebRequest.Create(sec_url) as HttpWebRequest;
                request.ContentType = "application/json";
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.71 Safari/537.36";
                if (IsTimeOutLimit)
                    request.Timeout = TimeOut;
                request.Method = "GET";
                response = request.GetResponse() as HttpWebResponse;            ////request.Method = "GET";   
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default))
                {
                    msg = sr.ReadToEnd();
                }

                if (msg.Length < 1)
                    return false;
                Parse(msg);  //parse json




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
        public abstract decimal GetBid();  //目前都為買賣一檔
        public abstract decimal GetBidVol();
        public abstract decimal GetAsk();
        public abstract decimal GetAskVol();

        //public abstract decimal GetBid(int idx);
        //public abstract decimal GetAsk(int idx);
        public abstract string GetErrorCode();

        //public abstract void Init();
        public abstract void Clear();
        public abstract void Parse(string msg);

        #region IDisposable Support

        private bool disposed = false; // To detect redundant calls
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    //return;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                
                GC.Collect();
                disposed = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        ~AbstractRestful()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        /// <summary>
        /// be careful use dispose 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);              // Do not change this code. Put cleanup code in Dispose(bool disposing) above.  
            GC.SuppressFinalize(this); //  TODO: uncomment the following line if the finalizer is overridden above.
        }
        #endregion



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

    public class KuCoinPriceStatus
    {
        public bool success { get; set; }
        public string code { get; set; }
        public string msg { get; set; }
        public long timestamp { get; set; }
        public Data data { get; set; }
    }

    public class Data
    {
        public string coinType { get; set; }
        public bool trading { get; set; }
        public string symbol { get; set; }
        public decimal lastDealPrice { get; set; }
        public decimal buy { get; set; }
        public decimal sell { get; set; }
        public decimal change { get; set; }
        public string coinTypePair { get; set; }
        public int sort { get; set; }
        public float feeRate { get; set; }
        public decimal volValue { get; set; }
        public decimal high { get; set; }
        public long datetime { get; set; }
        public decimal vol { get; set; }
        public decimal low { get; set; }
        public decimal changeRate { get; set; }
    }

    public class KuCoinBidAsk
    {
        public bool success { get; set; }
        public string code { get; set; }
        public string msg { get; set; }
        public long timestamp { get; set; }
        public KuTick KuTick { get; set; }
    }

    public class KuTick
    {
        public decimal[][] SELL { get; set; }
        public decimal[][] BUY { get; set; }
    }

    #endregion

    #region Binance

    public class BinanceError
    {
        public int code { get; set; }
        public string msg { get; set; }
    }
    
    public class BinancePriceStatus
    {
        public string symbol { get; set; }
        public string priceChange { get; set; }
        public string priceChangePercent { get; set; }
        public string weightedAvgPrice { get; set; }
        public string prevClosePrice { get; set; }
        public string lastPrice { get; set; }
        public string lastQty { get; set; }
        public string bidPrice { get; set; }
        public string bidQty { get; set; }
        public string askPrice { get; set; }
        public string askQty { get; set; }
        public string openPrice { get; set; }
        public string highPrice { get; set; }
        public string lowPrice { get; set; }
        public string volume { get; set; }
        public string quoteVolume { get; set; }
        public long openTime { get; set; }
        public long closeTime { get; set; }
        public int firstId { get; set; }
        public int lastId { get; set; }
        public int count { get; set; }
    }
    #endregion

    #region OKex

    public class OKexPrice
    {
        public string date { get; set; }
        public OKexticker OKexticker { get; set; }
    }
    public class OKexticker
    {
        public string high { get; set; }
        public string vol { get; set; }
        public string last { get; set; }
        public string low { get; set; }
        public string buy { get; set; }
        public string sell { get; set; }
    }
    public class OKexBidAsk
    {
        public decimal[][] asks { get; set; }
        public decimal[][] bids { get; set; }
    }

    /// <summary>
    /// Errocode Json
    /// </summary>
    public class OkexErrorCode
    {
        public int error_code { get; set; }
    }


    #endregion

    #region Hitbic


    /// <summary>
    /// 
    /// </summary>
    /// https://api.hitbtc.com/api/1/public/LTCETH/ticker
    public class HTBPrice
    {
        public string ask { get; set; }
        public string bid { get; set; }
        public string last { get; set; }
        public string low { get; set; }
        public string high { get; set; }
        public string open { get; set; }
        public string volume { get; set; }
        public string volume_quote { get; set; }
        public long timestamp { get; set; }
    }
    public class HTBBidAsk
    {
        public string[][] asks { get; set; }
        public string[][] bids { get; set; }
    }


    #endregion

    #endregion
}
