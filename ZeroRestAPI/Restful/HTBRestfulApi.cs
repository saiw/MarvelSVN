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
    /// URL         :https://hitbtc.com/
    /// API Refrence:https://github.com/hitbtc-com/hitbtc-api/blob/master/APIv1.md
    /// </summary>
    /// 
    public class HTBRestfulApi : AbstractRestful
    {
        public override string URL => $"{HOSTURL}{ResourcePath}";
        public string SecondURL => $"{HOSTURL}{ResourcePath2}";

        public HTBPrice Price { get; private set; }
        public HTBBidAsk BidAsk { get; private set; }

        public HTBRestfulApi() { }

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
            if (contect.IndexOf("last") != -1 && contect.IndexOf("volume") != -1)
            {
                //parse price status
                this.Price = Newtonsoft.Json.JsonConvert.DeserializeObject<HTBPrice>(contect);
            }
            else if (contect.IndexOf("bids") != -1 && contect.IndexOf("asks") != -1)
            {
                // bid /ask 
                this.BidAsk = Newtonsoft.Json.JsonConvert.DeserializeObject<HTBBidAsk>(contect);
            }
            else if (contect.IndexOf("error_code") != -1)
            {
                isError = true;
                errorCode = contect;///可以客制 ErroCode
            }
            else
            {

            }
        }

        #region Excel 

        public override decimal GetAsk()
        {
            return this.BidAsk != null ? BidAsk.asks[0][0] : 0m;

            //decimal ask = 0m;
            //return decimal.TryParse(BidAsk.asks[0][0], out ask) ? ask : 0m; ;
        }

        public override decimal GetAskVol()
        {
            return this.BidAsk != null ? BidAsk.asks[0][1] : 0m;

            //decimal ask = 0m;
            //return decimal.TryParse(BidAsk.asks[0][1], out ask) ? ask : 0m; ;
        }

        public override decimal GetBid()
        {
            return this.BidAsk != null ? BidAsk.bids[0][0] : 0m;


            //decimal bid = 0m;
            //return decimal.TryParse(BidAsk.bids[0][0], out bid) ? bid : 0m; ;
        }

        public override decimal GetBidVol()
        {
            return this.BidAsk != null ? BidAsk.bids[0][1] : 0m;

            //decimal bid = 0m;
            //return decimal.TryParse(BidAsk.bids[0][1], out bid) ? bid : 0m; ;
        }

        public override decimal GetClose()
        {
            return this.Price != null ? Price.last : 0m;
            ////decimal close = 0m;
            ////return decimal.TryParse(Price.last, out close) ? close : 0m;
            
        }

        public override string GetErrorCode()
        {
            return this.errorCode;
        }

        #endregion


    }
}
