using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZeroRestAPI;

namespace APITest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Console.WriteLine("Test GetMaketDeatil ");

            HuobiApiTests test = new HuobiApiTests();
            test.GetMaketDeatil();

            Console.ReadKey();
        }
    }
    [TestClass()]
    public class HuobiApiTests
    {
        /* 1.api = new HuobiApi(網域)
         *   vba 使用  api = new HuobiApi()
                       api.HoubitHOST = "api.huobipro.com";
                       api.Init();
         * 2.api.SendRequest(path, param) ex:( /market/detail/merged , ekobtc)
         * 3.GetClose / GetBid(0) ->Bid price ; 
         *              GetBid(1) ->Bid Vol   ; ask 同理
         */

        //HuobiRestfulApi api = new HuobiRestfulApi("api.huobipro.com"); 
        HuobiRestfulApi api = new HuobiRestfulApi();

        [TestMethod()]
        public void GetMaketDeatil()
        {
            api.HoubitHOST = "api.huobipro.com";
            api.Init();
            string path = "/market/detail/merged";

            //cointoken
            string param = "ethbtc";
            var result = api.SendRequest(path, param);
            Print();

            ///
            param = "quneth";
            result = api.SendRequest(path, param);
            Print();

            param = "qunbtc";
            result = api.SendRequest(path, param);
            Print();

            param = "ekoeth";
            result = api.SendRequest(path, param);
            Print();

            param = "ekobtc";
            result = api.SendRequest(path, param);
            Print();

            param = "qusheth";
            result = api.SendRequest(path, param);
            Print();

            param = "qushbtc";
            result = api.SendRequest(path, param);
            Print();

            param = "thetaeth";
            result = api.SendRequest(path, param);
            Print();

            param = "thetabtc";
            result = api.SendRequest(path, param);
            Print();

            param = "wicceth";
            result = api.SendRequest(path, param);
            Print();

            param = "wiccbtc";
            result = api.SendRequest(path, param);
            Print();
        }

        public void Print()
        {
            Console.WriteLine(api.URL);
            if (api.IsError)
                Console.WriteLine(api.ErrorCode);
            else 
            Console.WriteLine($"Close ={api.GetClose()} ," +
                              $"Bid ={api.GetBid(0)},vol={api.GetBid(1)}" +
                              $"Ask ={api.GetAsk(0)},vol={api.GetAsk(1)}" +
                              $"\n { api.LastRevTime} ; {api.TokenCoin}");

            //Console.WriteLine(api.Contect);
            Console.WriteLine(api.LastRevTime);

        }

        [TestMethod()]
        public void GetAllAccountTest()
        {
            //var result = api.GetAllAccount();
            //Console.WriteLine(result.GetEnumerator());
            //Assert.IsNull(result);
        }

        [TestMethod()]
        public void OrderPlaceTest()
        {
            //var accounts = api.GetAllAccount();
            //var spotAccountId = accounts.FirstOrDefault(a => a.Type == "spot" && a.State == "working")?.Id;
            //if (spotAccountId <= 0)
            //    throw new ArgumentException("spot account unavailable");
            //OrderPlaceRequest req = new OrderPlaceRequest();
            //req.account_id = spotAccountId.ToString();
            //req.amount = "0.1";
            //req.price = "800";
            //req.source = "api";
            //req.symbol = "ethusdt";
            //req.type = "buy-limit";
            //var result = api.OrderPlace(req);
            //Assert.AreEqual(result.Status, "ok");
        }
    }
}
