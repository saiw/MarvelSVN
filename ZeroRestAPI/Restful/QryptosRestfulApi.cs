using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroRestAPI
{
    public class QryptosRestfulApi : AbstractRestful
    {
        public string ID = string.Empty;
        public override string URL => $"{HOSTURL}{ResourcePath}/{ID}";
        public string SecondURL => $"{HOSTURL}{ResourcePath}/{ID}/price_levels";

        public override void Clear()
        {
            throw new NotImplementedException();
        }

        public override decimal GetAsk()
        {
            throw new NotImplementedException();
        }

        public override decimal GetAskVol()
        {
            throw new NotImplementedException();
        }

        public override decimal GetBid()
        {
            throw new NotImplementedException();
        }

        public override decimal GetBidVol()
        {
            throw new NotImplementedException();
        }

        public override decimal GetClose()
        {
            throw new NotImplementedException();
        }

        public override string GetErrorCode()
        {
            throw new NotImplementedException();
        }

        public override void Parse(string msg)
        {
            throw new NotImplementedException();
        }
    }
}
