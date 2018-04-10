/* Create
 * Author :Joqq Lin
 * DATE   :2018-xx-xx
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroRestAPI
{
    /// <summary>
    /// URL         :https://www.qryptos.com/
    /// API Refrence:https://developers.quoine.com/#introduction
    /// </summary>
    public class QryptosRestfulApi : AbstractRestful
    {
        public string ID = string.Empty;
        public override string URL => $"{HOSTURL}{ResourcePath}";
        public string SecondURL => $"{HOSTURL}{ResourcePath2}";

        /// <summary>
        ///  
        /// </summary>
        /// <remarks> utf-8 </remarks>
        protected override Encoding encoding { get => Encoding.UTF8; }

        public QryptosPrice Price { get; private set; }
        public QryptosBidAsk BidAsk { get; private set; }


        public override void Clear()
        {
            this.Price = null;
            this.BidAsk = null;
            isError = false;
            errorCode = string.Empty;

            contect = string.Empty;
            this.TokenCoin = string.Empty;

        }
        public override void Parse(string msg)
        {
            //TO DO Parse 
            contect = ToJsonFormat(msg); //parse Error
            if (msg.IndexOf("currency_pair_code") != -1) 
            {
                //parse price status
                this.Price = Newtonsoft.Json.JsonConvert.DeserializeObject<QryptosPrice>(msg);

                //token Coin currency pair
                TokenCoin = (Price !=null)? Price.currency_pair_code :string.Empty ; 
            }
            else if (msg.IndexOf("buy_price_levels") != -1 && msg.IndexOf("sell_price_levels") != -1)
            {
                // order tick 
                this.BidAsk = Newtonsoft.Json.JsonConvert.DeserializeObject<QryptosBidAsk>(msg);
            }
            else if (msg.IndexOf("message") != -1)
            {
                //error
                isError = true;
                errorCode = contect;
            }
            else
            {

            }
        }

        #region Excel 

        //    //[tick][0]:price
        //    //[tick][1]:vol
        public decimal GetAsk(int tick = 0)
        {
            return this.Price != null ? (decimal)this.BidAsk.sell_price_levels[tick][0] : 0m;
        }

        public override decimal GetAsk()
        {
            return GetAsk(0);
        }

        public override decimal GetAskVol()
        {
            return this.Price != null ? (decimal)this.BidAsk.sell_price_levels[0][1] : 0m;
        }

        public override decimal GetBid()
        {
            return this.Price != null ? (decimal)this.BidAsk.buy_price_levels[0][0] : 0m;
        }

        public override decimal GetBidVol()
        {
            return this.Price != null ? (decimal)this.BidAsk.buy_price_levels[0][1] : 0m;
        }

        public override decimal GetClose()
        {
            return this.Price != null ? (decimal)Price.last_traded_price : 0m;
        }

        public override string GetErrorCode()
        {
            return errorCode;
        }
        #endregion

    }
}
