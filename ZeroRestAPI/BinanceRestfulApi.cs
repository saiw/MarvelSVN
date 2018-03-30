using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroRestAPI
{
    public class BinanceRestfulApi : AbstractRequest ,IPrint
    {



        #region Method

        public override void Init()
        {
            throw new NotImplementedException();
        }

        public override bool SendRequest(string resourcePath, string parameters = "")
        {
            throw new NotImplementedException();
        }

        #endregion

        bool IPrint.IsError => throw new NotImplementedException();

        string IPrint.URL => throw new NotImplementedException();

        string IPrint.ErrorCode => throw new NotImplementedException();

        string IPrint.LastRevTime => throw new NotImplementedException();

        string IPrint.TokenCoin => throw new NotImplementedException();

        #region IPrint  

        public decimal GetAsk(int idx)
        {
            throw new NotImplementedException();
        }

        public decimal GetBid(int idx)
        {
            throw new NotImplementedException();
        }

        public decimal GetClose()
        {
            throw new NotImplementedException();
        }

        public string GetErrorCode()
        {
            throw new NotImplementedException();
        }

        decimal IPrint.GetClose()
        {
            throw new NotImplementedException();
        }

        decimal IPrint.GetBid(int idx)
        {
            throw new NotImplementedException();
        }

        decimal IPrint.GetAsk(int idx)
        {
            throw new NotImplementedException();
        }

        string IPrint.GetErrorCode()
        {
            throw new NotImplementedException();
        }


        #endregion
    }
}
