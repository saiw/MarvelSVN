using System;
using System.IO;
using System.Net;
using System.Text;

namespace ZeroRestAPI
{
    public class HuobiRestfulApi
    {
        #region HuoBiApi配置信息
        /// <summary>
        /// API域名名称
        /// </summary>
        public string HoubitHOST = string.Empty;
        /// <summary>
        /// HOUST
        /// </summary>
        private string HoubitHOSTURL
        {
            get { return $"https://" + HoubitHOST; }  //https://api.huobipro.com/market
        }


        #endregion
   
        public PriceStatus Price { get; private set; }
        public string Contect { get; private set; }
        public string URL { get; private set; }
        public string TokenCoin { get; private set;  }
        public bool IsError = false;
        public string ErrorCode { get; private set; }
        public string LastRevTime
        {
            get { return lastRevTime.ToString("HH:mm:ss"); }
        }

        //private RestClient client;//http请求客户端
        private HttpWebRequest client;//http请求客户端
        private DateTime lastRevTime = DateTime.MinValue;


        private HttpWebRequest request;
        private HttpWebResponse response;
        public HuobiRestfulApi()
        {
        }
        
        public void Init()
        {
            client = WebRequest.Create(this.HoubitHOSTURL) as HttpWebRequest;
            client.ContentType = "application/json";
            client.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.71 Safari/537.36";

            ////client = new RestClient(HoubitHOSTURL);
            ////client.AddDefaultHeader("Content-Type", "application/json");
            ////client.AddDefaultHeader("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.71 Safari/537.36");
        }

        /// <summary>
        /// 請求前清空
        /// </summary>
        public void Clear()
        {
            this.Price = null;
            this.Contect = string.Empty;
            this.TokenCoin = string.Empty;
            this.IsError = false;
            this.ErrorCode = string.Empty;
        }

        #region HTTP请求方法

        /// <summary>
        /// 发起Http请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resourcePath">market</param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public Boolean SendRequest(string resourcePath, string parameters = "")
        {
            this.Clear(); //送出請求前參數reset
            this.TokenCoin = parameters;
            this.URL = $"{HoubitHOSTURL}{resourcePath}?symbol={parameters}";  // ?GET /market/detail/merged?symbol=ethusdt */

            try
            {
                request = WebRequest.Create(this.URL) as HttpWebRequest;
                response = request.GetResponse() as HttpWebResponse; ////request.Method = "GET";   

                string msg = string.Empty;
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default))
                {
                    msg = sr.ReadToEnd();
                }
                //parse json
                this.Contect = ToJsonFormat(msg);
                if (this.Contect.IndexOf("tick") != -1)
                {
                    this.Price = Newtonsoft.Json.JsonConvert.DeserializeObject<PriceStatus>(this.Contect);
                }
                else if (this.Contect.IndexOf("error") != -1)
                {
                    IsError = true;
                    ErrorCode = this.Contect;
                }
                else
                {

                }


            }
            catch (Exception ex)
            {
                this.ErrorCode = ex.Message;
                IsError = true;
                return false;
            }
            lastRevTime = DateTime.Now;
            return true;

            #region Restful sharp
            //this.Clear(); //送出請求前參數reset
            //this.TokenCoin = parameters; 
            //this.URL = $"{HoubitHOSTURL}{resourcePath}?symbol={parameters}";  // ?GET /market/detail/merged?symbol=ethusdt */
            ////Console.WriteLine(url);
            //var request = new RestRequest(this.URL, Method.GET);
            //var result = client.GetResponse();
            ////var result = client.Execute(request);
            //this.Contect = ToJsonFormat (result.Content);
            //if (result.Content.IndexOf("tick") != -1)
            //{ 
            //    this.Price = Newtonsoft.Json.JsonConvert.DeserializeObject<PriceStatus>(this.Contect);           
            //}
            //else if (result.Content.IndexOf("error") != -1)
            //{
            //    IsError = true;
            //}
            //else
            //{

            //}

            #endregion

            //return this.Contect;
        }

        public string ToJsonFormat(string msg)
        {
            return JsonHelper.DeserializeToString(msg);
        }

        #endregion

        #region Excel Method

        public decimal GetClose()
        {
            return this.Price != null ? (decimal)Price.tick.close : 0m;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idx">[0:price];[1:vol]</param>
        /// <returns></returns>
        public decimal GetBid(int idx)
        {
            return this.Price != null ? (decimal)this.Price.tick.bid[idx] : 0m;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idx">[0:price];[1:vol] </param>
        /// <returns></returns>
        public decimal GetAsk(int idx)
        {
            return this.Price != null ? (decimal)this.Price.tick.ask[idx] : 0m;
        }
        public string GetErrorCode ()
        {
            return this.ErrorCode;
        }


        #endregion

        


        #region No Use 

        /////// <summary>
        /////// 行情api 
        /////// </summary>
        /////// <remarks>不需要accesKey </remarks>
        /////// <param name="huobi_host"></param>
        ////public HuobiRestfulApi(string host)
        ////{
        ////    HoubitHOST = host;
        ////    //HUOBI_HOST_URL = "https://" + HUOBI_HOST; //https://api.huobipro.com/market	

        ////    if (string.IsNullOrEmpty(HoubitHOST))
        ////        ErrorCode = "HUOBI_HOST  Cannt Be Null Or Empty";
        ////    //throw new ArgumentException("HUOBI_HOST  Cannt Be Null Or Empty");

        ////    //client = new RestClient(HoubitHOSTURL);
        ////    //client.AddDefaultHeader("Content-Type", "application/json");
        ////    //client.AddDefaultHeader("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.71 Safari/537.36");
        ////}

        ///// <summary>
        ///// Uri参数值进行转义
        ///// </summary>
        ///// <param name="parameters">参数字符串</param>
        ///// <returns></returns>
        //private string UriEncodeParameterValue(string parameters)
        //{
        //    var sb = new StringBuilder();
        //    var paraArray = parameters.Split('&');
        //    var sortDic = new SortedDictionary<string, string>();
        //    foreach (var item in paraArray)
        //    {
        //        var para = item.Split('=');
        //        sortDic.Add(para.First(), UrlEncode(para.Last()));
        //    }
        //    foreach (var item in sortDic)
        //    {
        //        sb.Append(item.Key).Append("=").Append(item.Value).Append("&");
        //    }
        //    return sb.ToString().TrimEnd('&');
        //}
        ///// <summary>
        ///// 转义字符串
        ///// </summary>
        ///// <param name="str"></param>
        ///// <returns></returns>
        //public string UrlEncode(string str)
        //{
        //    StringBuilder builder = new StringBuilder();
        //    foreach (char c in str)
        //    {
        //        if (HttpUtility.UrlEncode(c.ToString(), Encoding.UTF8).Length > 1)
        //        {
        //            builder.Append(HttpUtility.UrlEncode(c.ToString(), Encoding.UTF8).ToUpper());
        //        }
        //        else
        //        {
        //            builder.Append(c);
        //        }
        //    }
        //    return builder.ToString();
        //}
        ///// <summary>
        ///// Hmacsha256加密
        ///// </summary>
        ///// <param name="text"></param>
        ///// <param name="secretKey"></param>
        ///// <returns></returns>
        //private static string CalculateSignature256(string text, string secretKey)
        //{
        //    using (var hmacsha256 = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey)))
        //    {
        //        byte[] hashmessage = hmacsha256.ComputeHash(Encoding.UTF8.GetBytes(text));
        //        return Convert.ToBase64String(hashmessage);
        //    }
        //}
        ///// <summary>
        ///// 请求参数签名
        ///// </summary>
        ///// <param name="method">请求方法</param>
        ///// <param name="host">API域名</param>
        ///// <param name="resourcePath">资源地址</param>
        ///// <param name="parameters">请求参数</param>
        ///// <returns></returns>
        //private string GetSignatureStr(Method method, string host, string resourcePath, string parameters)
        //{
        //    var sign = string.Empty;
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append(method.ToString().ToUpper()).Append("\n")
        //        .Append(host).Append("\n")
        //        .Append(resourcePath).Append("\n");
        //    //参数排序
        //    var paraArray = parameters.Split('&');
        //    List<string> parametersList = new List<string>();
        //    foreach (var item in paraArray)
        //    {
        //        parametersList.Add(item);
        //    }
        //    parametersList.Sort(delegate (string s1, string s2) { return string.CompareOrdinal(s1, s2); });
        //    foreach (var item in parametersList)
        //    {
        //        sb.Append(item).Append("&");
        //    }
        //    sign = sb.ToString().TrimEnd('&');
        //    //计算签名，将以下两个参数传入加密哈希函数
        //    sign = CalculateSignature256(sign, SECRET_KEY);
        //    return UrlEncode(sign);
        //}

        #region 构造函数

        [Obsolete("Dont Need USE Trade")]
        public HuobiRestfulApi(string accessKey, string secretKey, string huobi_host = "api.huobi.pro")
        {
            //ACCESS_KEY = accessKey;
            //SECRET_KEY = secretKey;
            //HUOBI_HOST = huobi_host;
            ////HUOBI_HOST_URL = "https://" + HUOBI_HOST;
            //if (string.IsNullOrEmpty(ACCESS_KEY))
            //    throw new ArgumentException("ACCESS_KEY Cannt Be Null Or Empty");
            //if (string.IsNullOrEmpty(SECRET_KEY))
            //    throw new ArgumentException("SECRET_KEY  Cannt Be Null Or Empty");
            //if (string.IsNullOrEmpty(HUOBI_HOST))
            //    throw new ArgumentException("HUOBI_HOST  Cannt Be Null Or Empty");
            //client = new RestClient(HoubitHOSTURL);
            //client.AddDefaultHeader("Content-Type", "application/json");
            //client.AddDefaultHeader("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.71 Safari/537.36");
        }
        #endregion

        #region HuoBiApi方法
        //public List<Account> GetAllAccount()
        //{
        //    var result = SendRequest<List<Account>>(API_ACCOUNBT_ALL);
        //    return result.Data;
        //}
        ////public HBResponse<long> OrderPlace(OrderPlaceRequest req)
        ////{
        ////    var bodyParas = new Dictionary<string, string>();
        ////    var result = SendRequest<long, OrderPlaceRequest>(API_ORDERS_PLACE, req);
        ////    return result;
        ////}

        #endregion

        ///交易請求
        ///// <summary>
        ///// 发起Http请求
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="resourcePath">market</param>
        ///// <param name="parameters"></param>
        ///// <returns></returns>
        //public HBResponse<T> SendRequest<T>(string resourcePath, string parameters = "") where T : new()
        //{
        //    //parameters = UriEncodeParameterValue(GetCommonParameters() + parameters);//请求参数
        //    ////var sign = GetSignatureStr(Method.GET, HUOBI_HOST, resourcePath, parameters);//签名
        //    ////parameters += $"&Signature={sign}";

        //    var url = $"{HUOBI_HOST_URL}{resourcePath}?{parameters}";  // ?GET /market/detail/merged
        //    Console.WriteLine(url);


        //    var request = new RestRequest(url, Method.GET);
        //    var result = client.Execute<HBResponse<T>>(request);
        //    return result.Data;
        //}


        //private HBResponse<T> SendRequest<T, P>(string resourcePath, P postParameters) where T : new()
        //{
        //    var parameters = UriEncodeParameterValue(GetCommonParameters());//请求参数
        //    var sign = GetSignatureStr(Method.POST, HUOBI_HOST, resourcePath, parameters);//签名
        //    parameters += $"&Signature={sign}";

        //    var url = $"{HUOBI_HOST_URL}{resourcePath}?{parameters}";
        //    Console.WriteLine(url);
        //    var request = new RestRequest(url, Method.POST);
        //    request.AddJsonBody(postParameters);
        //    foreach (var item in request.Parameters)
        //    {
        //        item.Value = item.Value.ToString().Replace("_", "-");
        //    }
        //    var result = client.Execute<HBResponse<T>>(request);
        //    return result.Data;
        //}
        ///// <summary>
        ///// 获取通用签名参数
        ///// </summary>
        ///// <returns></returns>
        //private string GetCommonParameters()
        //{
        //    return $"AccessKeyId={ACCESS_KEY}&SignatureMethod={HUOBI_SIGNATURE_METHOD}&SignatureVersion={HUOBI_SIGNATURE_VERSION}&Timestamp={DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss")}";
        //}

        #endregion

    }
}
