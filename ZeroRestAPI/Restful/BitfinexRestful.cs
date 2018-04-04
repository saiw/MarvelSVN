using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroRestAPI
{    /// <summary>
     /// URL         :https://www.bitfinex.com/
     /// API Refrence:https://bitfinex.readme.io/v2/reference#rest-public-tickers
     /// </summary>
    public class BitfinexRestful : AbstractRestful
    {


        public override string URL => $"{HOSTURL}{ResourcePath}?symbols=t{TokenCoin}"; //https://api.bitfinex.com/v2/tickers?symbols=tEOSETH

        private string[] Price;
        //private BitFinexPrice Price;
        public override void Clear()
        {
            Price = null;
            contect = string.Empty;
            this.TokenCoin = string.Empty;
            isError = false;
            errorCode = string.Empty;
        }


        public override void Parse(string msg)
        {
            /// how to parse 
            if (msg.Contains(this.TokenCoin))
            {
                Price = msg.Replace("[", "").Replace("]", "").Split(',');
                //this.Price = Newtonsoft.Json.JsonConvert.DeserializeObject<BitFinexPrice>(contect);
                //()=> x= Price.AsEnumerable(){ Console.WriteLine(x); }
            }
            else
            {
                this.isError = false;
                this.errorCode = $"No Content! {msg}";
            }

            contect = ToJsonFormat(msg);

        }
        public BitfinexRestful() { }

        #region Method 
        public override decimal GetAsk()
        {
            return this.Price!=null ? decimal.Parse(Price[(int)Bitfinex.Tick.ASK]) :0m ;
        }
        public override decimal GetAskVol()
        {
            return this.Price != null ? decimal.Parse(Price[(int)Bitfinex.Tick.ASK_SIZE]) : 0m;
        }
        public override decimal GetBid()
        {
            return this.Price != null ? decimal.Parse(Price[(int)Bitfinex.Tick.BID]) : 0m;
        }
        public override decimal GetBidVol()
        {
            return this.Price != null ? decimal.Parse(Price[(int)Bitfinex.Tick.BID_SIZE]) : 0m;
        }
        public override decimal GetClose()
        {
            return this.Price != null ? decimal.Parse(Price[(int)Bitfinex.Tick.LAST_PRICE]) : 0m;
        }
        public override string GetErrorCode()
        {
            return this.ErrorCode;
        }
        #endregion


    }
}
