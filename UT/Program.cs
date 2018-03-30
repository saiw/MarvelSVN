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

            ApiTests test = new ApiTests();

            test.GeKuCoinDetail();
            //test.GetMaketDeatil();

            Console.ReadKey();
        }
    }
    [TestClass()]
    public class ApiTests
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

        [TestMethod()]
        public void GeKuCoinDetail()
        {
            //var result = false;

            KuCoinRestfulApi api = new KuCoinRestfulApi();
            api.Host = "api.kucoin.com";

            /*ACAT - BTC */
            api.Clear();
            api.TokenCoin = "ACAT-BTC";
            api.ResourcePath = $"/v1/{api.TokenCoin}/open/tick"; //https://api.kucoin.com/v1/ETH-BTC/open/tick
            Console.WriteLine(api.URL);
            api.SendRequest(api.URL);

            Console.WriteLine(api.OrderBookURL);//https://api.kucoin.com/v1/ETH-BTC/open/orders                                         
            api.SendRequest(api.OrderBookURL);
       

            Print(api);

            /*ACAT-ETH*/
            api.Clear();
            api.TokenCoin = "ACAT-ETH";
            api.ResourcePath = $"/v1/{api.TokenCoin}/open/tick";

            Console.WriteLine(api.URL);
            api.SendRequest(api.URL);
            Console.WriteLine(api.OrderBookURL);//api.ResourcePath = $"/v1/{api.TokenCoin}/open/orders";
            api.SendRequest(api.OrderBookURL);  //api.ResourcePath = $"/v1/{api.TokenCoin}/open/orders";


            Print(api);

            //Console.WriteLine(api.Contect);



        }
        [TestMethod()]
        public void GetMaketDeatil()
        {
            HuobiRestfulApi api = new HuobiRestfulApi();
            api.Host = "api.huobipro.com";
            api.ResourcePath = "/market/detail/merged";

            api.Clear();
            api.TokenCoin = "ethbtc";
            Console.WriteLine(api.URL);
            var result = api.SendRequest(api.URL);
            Print( api);

            /////

            api.Clear();
            api.TokenCoin = "quneth";
            Console.WriteLine(api.URL);
            result = api.SendRequest(api.URL);
            Print(api);

            api.Clear();
            api.TokenCoin = "qunbtc";
            Console.WriteLine(api.URL);
            result = api.SendRequest(api.URL);
            Print(api);

            //param = "ekoeth";
            //api.Clear();
            //result = api.SendRequest(path, param);
            //Print(api);

            //param = "ekobtc";
            //api.Clear();
            //result = api.SendRequest(path, param);
            //Print(api);

            //param = "qusheth";
            //api.Clear();
            //result = api.SendRequest(path, param);
            //Print(api);

            //param = "qushbtc";
            //api.Clear();
            //result = api.SendRequest(path, param);
            //Print(api);

            //param = "thetaeth";
            //api.Clear();
            //result = api.SendRequest(path, param);
            //Print(api);

            //param = "thetabtc";
            //api.Clear();
            //result = api.SendRequest(path, param);
            //Print(api);

            //param = "wicceth";
            //api.Clear();
            //result = api.SendRequest(path, param);
            //Print(api);

            //param = "wiccbtc";
            //api.Clear();
            //result = api.SendRequest(path, param);
            //Print(api);
        }


        [TestMethod()]
        public void GeOKCoinDetail()
        {
            //////https://www.okcoin.com/api/v1/ticker.do?symbol=ltc_usd
            ////HuobiRestfulApi api = new HuobiRestfulApi();
            ////api.host = "www.okcoin.com";
            ////api.Init();

            ////string path = "/api/v1/ticker.do";
            ////string param = "bkx_etc";
            ////var result = api.SendRequest(path, param);

            ////Console.WriteLine(api.Contect);
        }
        
        public void Print(AbstractRestful api)
        {
            if (api.IsError)
                Console.WriteLine(api.ErrorCode);
            else
                Console.WriteLine($"Close ={api.GetClose()};\n" +
                                  $"Bid ={api.GetBid()};Vol=[{api.GetBidVol()}],\n" +
                                  $"Ask ={api.GetAsk()};Vol=[{api.GetAskVol()}],\n" +
                                  $"--{ api.LastRevTime}--{api.TokenCoin}--\n");

            //Console.WriteLine(api.Contect);
            //Console.Write($"{api.LastRevTime}\n");

        }


    }
}
