/* Create
 * Author :Joqq Lin
 * DATE   :2018-03-29
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/* Create
 * Author :Joqq Lin
 * DATE   :2018-xx-xx
 */

namespace ZeroRestAPI
{
    /// <summary>
    /// URL         :https://www.kucoin.com/
    /// API Refrence:https://kucoinapidocs.docs.apiary.io/#introduction/language-parameters
    /// </summary>
    public class KuCoinRestfulApi : AbstractRestful
    {
        protected override int TimeOut => 3 * 1000; // 3秒斷現
        public override string URL => $"{HOSTURL}{ResourcePath}";//https://api.kucoin.com/v1/ETH-BTC/open/tick

        public string SecondURL => $"{HOSTURL}{ResourcePath2}";//https://api.kucoin.com/v1/ETH-BTC/open/orders

        public string OrderBookURL => URL.Replace("tick", "orders");

        public KuCoinPriceStatus Price { get; private set; }

        public KuCoinBidAsk BidAsk { get; private set; }

        public KuCoinRestfulApi()
        { }

        
        /// <summary>
        /// 清除tick object 及請求token
        /// </summary>
        public override void Clear()
        {
            this.Price = null;
            this.BidAsk = null;
            contect = string.Empty;
            this.TokenCoin = string.Empty;
            isError = false;
            errorCode = string.Empty;
        }

        public override void Parse(string msg)
        {
            contect = ToJsonFormat(msg);
            if (contect.IndexOf("lastDealPrice") != -1) //price status
            {
                //parse price status
                this.Price = Newtonsoft.Json.JsonConvert.DeserializeObject<KuCoinPriceStatus>(this.Contect);
            }
            else if (contect.IndexOf("SELL") != -1 && contect.IndexOf("BUY") != -1)
            {
                // order tick 
                string newMsg = this.contect.Replace("data" , "KuTick"); ////data to TickData 解析成TickDataObj
                this.BidAsk = Newtonsoft.Json.JsonConvert.DeserializeObject<KuCoinBidAsk>(newMsg);
            }
            else if (!this.Contect.Contains("success:true"))
            {
                isError = true;
                errorCode = this.Contect;
            }
            else
            {

            }
        }

        #region Excel 

        public override string GetErrorCode()
        {
            return this.ErrorCode;
        }
        public override decimal GetClose()
        {
            return this.Price != null ? this.Price.data.lastDealPrice : 0m;
        }
        public override decimal GetBid()
        {
            if (this.BidAsk == null)
                return 0m;
            return this.BidAsk.KuTick != null  ? this.BidAsk.KuTick.BUY[0][0] : 0m;
        }

        public override decimal GetBidVol()
        {
            if (this.BidAsk == null)
                return 0m;
            return this.BidAsk.KuTick != null ? this.BidAsk.KuTick.BUY[0][1] : 0m;
        }

        public override decimal GetAsk()
        {
            if (this.BidAsk == null)
                return 0m;
            return this.BidAsk.KuTick != null ? this.BidAsk.KuTick.SELL[0][0] : 0m;
        }

        public override decimal GetAskVol()
        {
            if (this.BidAsk == null)
                return 0m;
            return this.BidAsk.KuTick != null ? this.BidAsk.KuTick.SELL[0][1] : 0m;
        }
        #endregion


    }
}
