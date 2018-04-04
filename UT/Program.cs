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
            ApiTests test = new ApiTests();

            Console.WriteLine("========Test GetBifinexDetail  =========");
            test.GetBifinexDetail();
            
            //Console.WriteLine("========Test GetHitBtcDetail  =========");
            //test.GetHitBtcDetail();
            //Console.WriteLine("========Test GetOkexCoinDetail=========");
            //test.GetOkexCoinDetail();
            //test.GetBinanceDetail();
            //Console.WriteLine("========Test GeKuCoinRestfulApi  =========");
            //test.GetKuCoinDetail();
            //test.GetMaketDeatil();

            Console.WriteLine("press any key to exit");
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
        [TestMethod()]
        public void GetBifinexDetail()
        {
            BitfinexRestful api = new BitfinexRestful();
            api.Host = "api.bitfinex.com";

            api.TokenCoin = "EOSBTC";
            api.ResourcePath = $"/v2/tickers";
            Console.WriteLine(api.URL);             //https://api.bitfinex.com/v2/tickers?symbols=tEOSBTC          
            api.SendRequest(api.URL);
            Print(api);

            api.Clear(); /// 清空

            api.TokenCoin = "EOSETH";
            api.ResourcePath = $"/v2/tickers";
            Console.WriteLine(api.URL);             //https://api.bitfinex.com/v2/tickers?symbols=tEOSBTC          
            api.SendRequest(api.URL);
            Print(api);
        }
        [TestMethod()]
        public void GetHitBtcDetail()
        {
            HTBRestfulApi api = new HTBRestfulApi();
            api.Host = "api.hitbtc.com";

            api.TokenCoin = "LTCETH";
            api.ResourcePath = $"/api/1/public/{api.TokenCoin}/ticker";
            Console.WriteLine(api.URL);                                //https://www.okex.com/api/v1/ticker.do?symbol=ltc_btc
            api.ResourcePath2 = $"/api/1/public/{api.TokenCoin}/orderbook";
            Console.WriteLine(api.SecondURL);                          //https://www.okex.com/api/v1/depth.do?symbol=ltc_btc
            api.SendCombineRequest(api.URL, api.SecondURL);
            Print(api);

            api.Clear();
            //api.Dispose();

            api.TokenCoin = "LTCBTC";
            api.ResourcePath = $"/api/1/public/{api.TokenCoin}/ticker";
            Console.WriteLine(api.URL);                                //https://www.okex.com/api/v1/ticker.do?symbol=ltc_btc
            api.ResourcePath2 = $"/api/1/public/{api.TokenCoin}/orderbook";
            Console.WriteLine(api.SecondURL);                          //https://www.okex.com/api/v1/depth.do?symbol=ltc_btc
            api.SendCombineRequest(api.URL, api.SecondURL);
            Print(api);


        }
        [TestMethod()]
        public void GetOkexCoinDetail()
        {
            OkexRestfulApi api = new OkexRestfulApi();
            api.Host = "www.okex.com";

            //api.TokenCoin = "ltc_btc";
            //api.ResourcePath = "/api/v1/ticker.do";
            //Console.WriteLine(api.URL);                  //https://www.okex.com/api/v1/ticker.do?symbol=ltc_btc
            //api.ResourcePath2 = "/api/v1/depth.do";
            //Console.WriteLine(api.SecondURL);           //https://www.okex.com/api/v1/depth.do?symbol=ltc_btc
            //api.SendCombineRequest(api.URL, api.SecondURL);
            //Print(api);

            api.Clear();
            api.TokenCoin = "bkx_eth";
            api.ResourcePath = "/api/v1/ticker.do";
            Console.WriteLine(api.URL);                  //https://www.okex.com/api/v1/ticker.do?symbol=ltc_btc
            api.ResourcePath2 = "/api/v1/depth.do";
            Console.WriteLine(api.SecondURL);           //https://www.okex.com/api/v1/depth.do?symbol=ltc_btc
            api.SendCombineRequest(api.URL, api.SecondURL);
            Print(api);

            api.Clear();
            api.TokenCoin = "bch_btc";
            api.ResourcePath = "/api/v1/ticker.do";
            Console.WriteLine(api.URL);                  //https://www.okex.com/api/v1/ticker.do?symbol=ltc_btc
            api.ResourcePath2 = "/api/v1/depth.do";
            Console.WriteLine(api.SecondURL);           //https://www.okex.com/api/v1/depth.do?symbol=ltc_btc
            api.SendCombineRequest(api.URL, api.SecondURL);
            Print(api);

        }
        [TestMethod()]
        public void GetBinanceDetail()
        {
            BinanceRestfulApi api = new BinanceRestfulApi();
            api.Host = "api.binance.com";

            /*LTCBTC */
            api.Clear();
            api.TokenCoin = "EOSBTC";
            api.ResourcePath = $"/api/v1/ticker/24hr"; //https://api.binance.com/api/v1/ticker/24hr?symbol=BNBBTC
            Console.WriteLine(api.URL);
            api.SendRequest(api.URL);
            Print(api);

            /*Error*/
            api.Clear();
            api.TokenCoin = "xxx";
            api.ResourcePath = $"/api/v1/ticker/24hr"; //https://api.binance.com/api/v1/ticker/24hr?symbol=BNBBTC
            Console.WriteLine(api.URL);
            api.SendRequest(api.URL);
            Print(api);

        }
        [TestMethod()]
        public void GetKuCoinDetail()
        {
            //var result = false;

            KuCoinRestfulApi api = new KuCoinRestfulApi();
            api.Host = "api.kucoin.com";

            /*ACAT - BTC */
            //api.Clear();
            api.TokenCoin = "ACAT-BTC";
            api.ResourcePath = $"/v1/{api.TokenCoin}/open/tick";             //https://api.kucoin.com/v1/ETH-BTC/open/tick
            Console.WriteLine(api.URL);                                
            api.ResourcePath2 = $"/v1/{api.TokenCoin}/open/orders";  //https://api.kucoin.com/v1/ETH-BTC/open/orders   
            Console.WriteLine(api.SecondURL);  

            api.SendCombineRequest(api.URL, api.SecondURL);
            Print(api);

            //////////api.ResourcePath = $"/v1/{api.TokenCoin}/open/tick"; 
            //////////Console.WriteLine(api.URL);
            //////////api.SendRequest(api.URL);
            ////////Console.WriteLine(api.OrderBookURL);
            ////////api.SendRequest(api.OrderBookURL);
            //api.Clear();

            ///*ACAT-ETH*/
            //api.TokenCoin = "ACAT-ETH";
            //api.ResourcePath = $"/v1/{api.TokenCoin}/open/tick";

            //Console.WriteLine(api.URL);
            //api.SendRequest(api.URL);
            //Console.WriteLine(api.OrderBookURL);//api.ResourcePath = $"/v1/{api.TokenCoin}/open/orders";
            //api.SendRequest(api.OrderBookURL);  //api.ResourcePath = $"/v1/{api.TokenCoin}/open/orders";
            //Print(api);



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
        public void GetOKCoinDetail()
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
                Console.WriteLine($"Ask  ={api.GetAsk()}  ;Vol=[{api.GetAskVol()}],\n" +
                                  $"Close={api.GetClose()};\n" +
                                  $"Bid  ={api.GetBid()}  ;Vol=[{api.GetBidVol()}],\n" +
                                  $"------{ api.LastRevTime}--{api.TokenCoin}--\n");

            //Console.WriteLine(api.Contect);
            //Console.Write($"{api.LastRevTime}\n");

        }


    }
}
