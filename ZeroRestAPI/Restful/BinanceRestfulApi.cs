/* Create
 * Author :Joqq Lin
 * DATE   :2018-03-30
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroRestAPI
{
    /// <summary>
    /// URL         :https://www.binance.com/
    /// API Refrence:https://github.com/binance-exchange/binance-official-api-docs/blob/master/rest-api.md
    /// </summary>
    public class BinanceRestfulApi : AbstractRestful
    {
        public override string URL => $"{HOSTURL}{ResourcePath}?symbol={TokenCoin}";    //https://api.binance.com/api/v1/ticker/24hr?symbol=LTCBTC

        public BinancePriceStatus Price { get; private set; }
        private BinanceError errMsg;

        public BinanceRestfulApi()
        { }

        public override void Clear()
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
            if (contect.IndexOf("symbol") != -1 && contect.IndexOf("lastPrice") != -1)
            {
                this.Price = Newtonsoft.Json.JsonConvert.DeserializeObject<BinancePriceStatus>(contect);
            }
            else if (contect.IndexOf("code") != -1  && contect.IndexOf ("msg") !=-1)
            {
                isError = true;

                errMsg  = Newtonsoft.Json.JsonConvert.DeserializeObject<BinanceError>(contect);

                errorCode = $"errorCode :{errMsg.code} ;{errMsg.msg}";
            }
            else
            {

            }
        }

        #region  Excel 


        public override decimal GetAsk()
        {
            return Price != null ? Price.askPrice : 0m;


            //if (this.Price != null)
            //    return decimal.TryParse(Price.askPrice, out ask) ? ask : 0m;

        }

        public override decimal GetAskVol()
        {
            return Price != null ? Price.askQty : 0m;

            //decimal aVol = 0m;
            //if (this.Price != null)
            //    return decimal.TryParse(Price.askQty, out aVol) ? aVol : 0m;

            //return aVol;
        }

        public override decimal GetBid()
        {
            return Price != null ? Price.bidPrice : 0m;

            //decimal bid = 0m;
            //if (this.Price != null)
            //    return decimal.TryParse(Price.bidPrice, out bid) ? bid : 0m;

            //return bid;
        }

        public override decimal GetBidVol()
        {
            return Price != null ? Price.bidQty : 0m;

            //decimal vol = 0m;
            //if (this.Price != null)
            //    return decimal.TryParse(Price.bidQty, out vol) ? vol : 0m;
            //return vol;
        }

        public override decimal GetClose()
        {
            return Price != null ? Price.lastPrice : 0m;
            //decimal close  = 0m;
            //if (this.Price != null)
            //    return decimal.TryParse(Price.lastPrice, out close) ? close : 0m;
            //return close;
        }

        public override string GetErrorCode()
        {
            return   this.errorCode;
        }
        #endregion
    }
}
