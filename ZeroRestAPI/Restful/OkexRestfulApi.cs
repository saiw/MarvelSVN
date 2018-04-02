using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroRestAPI
{
    /// <summary>
    /// URL         :https://www.okex.com/
    /// API Refrence:https://github.com/okcoin-okex/API-docs-OKEx.com/blob/master/API-For-Spot-EN/REST%20API%20for%20SPOT.md
    /// </summary>
    public class OkexRestfulApi : AbstractRestful
    {
        protected override int TimeOut => 3 * 1000; // 3秒斷現
        public override string URL => $"{HOSTURL}{ResourcePath}?symbol={TokenCoin}";
        public string SecondURL => $"{HOSTURL}{ResourcePath2}?symbol={TokenCoin}";
        
        public OKexPrice Price { get; private set; }
        public OKexBidAsk BidAsk { get; private set; }

        public OkexRestfulApi() { }

        #region Method

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
            //string newMsg = string.Empty;

            contect = ToJsonFormat(msg);
            if (contect.IndexOf("ticker") != -1 && contect.IndexOf("last") != -1) 
            {
                //parse price status
                //newMsg = contect.Replace("ticker", "OKexticker"); ///customer json class name 
                this.Price = Newtonsoft.Json.JsonConvert.DeserializeObject<OKexPrice>(contect.Replace("ticker", "OKexticker"));
            }
            else if (contect.IndexOf("bids") != -1 && contect.IndexOf("asks") != -1)
            {
                // bid /ask 
                this.BidAsk = Newtonsoft.Json.JsonConvert.DeserializeObject<OKexBidAsk>(contect);
            }
            else if (contect.IndexOf("error_code") != -1)
            {
                isError = true;
                errorCode = contect ;///可以客制 ErroCode
            }
            else
            {

            }
        }
        #endregion 
        
        #region Excel 
        public override decimal GetAsk()
        {
            return this.BidAsk != null ? this.BidAsk.asks[0][0] : 0m;  
        }

        public override decimal GetAskVol()
        {
            return this.BidAsk != null ? this.BidAsk.asks[0][1] : 0m;  //[0]: price 1:vol
        }

        public override decimal GetBid()
        {
            return this.BidAsk != null ? this.BidAsk.bids[0][0] : 0m;  
        }

        public override decimal GetBidVol()
        {
            return this.BidAsk != null ? this.BidAsk.bids[0][1] : 0m;  //[0]: price 1:vol
        }

        public override decimal GetClose()
        {
            decimal close = 0m;
            if (this.Price != null)
                return decimal.TryParse(Price.OKexticker.last, out close) ? close : 0m;
            return close;
        }

        public override string GetErrorCode()
        {
            return this.ErrorCode;
        }
        #endregion

    }
}
