/* Create
 * Author :Joqq Lin
 * DATE   :2018-03-30
 */
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ZeroRestAPI

{    /// <summary>
     /// URL         :https://www.huobipro.com/
     /// API Refrence:https://github.com/huobiapi/API_Docs_en/wiki
     /// </summary>
    public class HuobiRestfulApi:AbstractRestful
    {
        protected override int TimeOut => 3*1000 ; // 3秒斷現
        public override string URL { get => $"{HOSTURL}{ResourcePath}?symbol={TokenCoin}"; }

        public PriceStatus Price { get; private set; }

        public HuobiRestfulApi()
        {
        }
        
        /// <summary>
        /// 請求前清空
        /// </summary>
        public override  void  Clear()
        {
            this.Price = null;
            contect = string.Empty;
            this.TokenCoin = string.Empty;
            isError = false;
            errorCode = string.Empty;
        }
        public override void Parse(string msg)
        {
            contect = ToJsonFormat(msg);
            if (this.Contect.IndexOf("tick") != -1)
            {
                this.Price = Newtonsoft.Json.JsonConvert.DeserializeObject<PriceStatus>(this.Contect);
            }
            else if (this.Contect.IndexOf("error") != -1)
            {
                isError = true;
                errorCode = this.Contect;
            }
            else
            {

            }

        }

        #region Excel 

        public override decimal GetClose()
        {
            return this.Price != null ? (decimal)Price.tick.close : 0m;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idx">[0:price];[1:vol]</param>
        /// <returns></returns>
        public override decimal GetBid()
        {
            return this.Price != null ? (decimal)this.Price.tick.bid[0] : 0m;
        }

        public override decimal GetBidVol()
        {
            return this.Price != null ? (decimal)this.Price.tick.bid[1] : 0m;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idx">[0:price];[1:vol] </param>
        /// <returns></returns>
        public override decimal GetAsk()
        {
            return this.Price != null ? (decimal)this.Price.tick.ask[0] : 0m;
        }

        public override decimal GetAskVol()
        {
            return this.Price != null ? (decimal)this.Price.tick.ask[1] : 0m;
        }
        public override string GetErrorCode ()
        {
            return this.ErrorCode;
        }

        #endregion

        #region HTTP请求方法

        //public static async Task<bool ,string  ,string>SendRequest (string url )
        //{
        //    var success = false;
        //    var errMsg = false;
        //    try
        //    {

        //        var request = WebRequest.Create(url) as HttpWebRequest;
        //        var response = request.GetResponse() as HttpWebResponse; ////request.Method = "GET";   

        //        string msg = string.Empty;
        //        using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default))
        //        {
        //            msg = sr.ReadToEnd();
        //        }
        //        //parse json
        //        //var contect = JsonHelper.DeserializeToString(msg);
        //        if (msg.IndexOf("tick") != -1)
        //        {
        //            success = true;
        //        }
        //        else if (msg.IndexOf("error") != -1)
        //        {
        //            success = false;
        //        }
        //        else
        //        {

        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        cont
        //    }
        //    base.lastRevTime = DateTime.Now;
        //    return true;
        //}


        ///// <summary>
        ///// 发起Http请求
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="resourcePath">market</param>
        ///// <param name="parameters"></param>
        ///// <returns></returns>
        //public override bool SendRequest(string resourcePath, string parameters = "")
        //{
        //    this.Clear(); //送出請求前參數reset
        //    this.TokenCoin = parameters;
        //    this.URL = $"{HoubitHOSTURL}{resourcePath}?symbol={parameters}";  // ?GET /market/detail/merged?symbol=ethusdt */

        //    try
        //    {
        //        request = WebRequest.Create(this.URL) as HttpWebRequest;
        //        if (IsTimeOutLimit)
        //            request.Timeout = TimeOut;
        //        response = request.GetResponse() as HttpWebResponse; ////request.Method = "GET";   

        //        string msg = string.Empty;
        //        using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default))
        //        {
        //            msg = sr.ReadToEnd();
        //        }
        //        //parse json
        //        this.Contect = ToJsonFormat(msg);
        //        if (this.Contect.IndexOf("tick") != -1)
        //        {
        //            this.Price = Newtonsoft.Json.JsonConvert.DeserializeObject<PriceStatus>(this.Contect);
        //        }
        //        else if (this.Contect.IndexOf("error") != -1)
        //        {
        //            IsError = true;
        //            ErrorCode = this.Contect;
        //        }
        //        else
        //        {

        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        this.ErrorCode = ex.Message;
        //        IsError = true;
        //        return false;
        //    }
        //    base.lastRevTime = DateTime.Now;
        //    return true;


        //}


        #endregion

    }
}
