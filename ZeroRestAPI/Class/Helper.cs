using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Xml;
using Newtonsoft.Json;

namespace ZeroRestAPI
{
    public static class JsonHelper
    {
        /// <summary>
        /// 将对象序列化为JSON格式
        /// </summary>
        /// <param name="o">对象</param>
        /// <returns>json字符串</returns>
        public static string SerializeObject(object o)
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(o);
            return json;
        }


        [Obsolete("Non Test")]
        public static System.Data.DataTable DeserializeToDataTable(string json)
        {
            //TO DO DataTable 

            System.Data.DataTable dt = JsonConvert.DeserializeObject<System.Data.DataTable>(json);
            return dt;
        }

        public static string DeserializeToString(string str)
        {
            //格式化json字符串
            JsonSerializer serializer = new JsonSerializer();
            TextReader tr = new StringReader(str);
            JsonTextReader jtr = new JsonTextReader(tr);
            object obj = serializer.Deserialize(jtr);
            if (obj != null)
            {
                StringWriter textWriter = new StringWriter();
                JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
                {
                    Formatting = Formatting.Indented,
                    Indentation = 4,
                    IndentChar = ' '
                };
                serializer.Serialize(jsonWriter, obj);
                return textWriter.ToString();
            }
            else
            {
                return str;
            }
        }

        public static string[] DeserializeToStringArray(string str)
        {

            return str.Split(',');
        }

        public static string JosonWriter(Dictionary<string, string> elment)
        {
            //無成功
            //https://www.newtonsoft.com/json/help/html/WriteJsonWithJsonTextWriter.htm

            if (elment.Count <= 0)
                new Exception("Dictionary Property No Zero!");

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Newtonsoft.Json.Formatting.Indented;


                writer.WriteStartObject();
                foreach (var key in elment.Keys)
                {
                    writer.WritePropertyName(key);
                    writer.WriteValue(elment[key]);
                }

                writer.WriteEnd();

                writer.WriteEndObject();

            }
            return sb.ToString();
        }


    }

    public static class GZipHelper
    {
        /// <summary>
        /// 将传入字符串以GZip算法压缩后，返回Base64编码字符
        /// </summary>
        /// <param name="rawString">需要压缩的字符串</param>
        /// <returns>压缩后的Base64编码的字符串</returns>
        public static string GZipCompressString(string rawString)
        {
            if (string.IsNullOrEmpty(rawString) || rawString.Length == 0)
            {
                return "";
            }
            else
            {
                byte[] rawData = System.Text.Encoding.UTF8.GetBytes(rawString.ToString());
                byte[] zippedData = Compress(rawData);
                return (string)(Convert.ToBase64String(zippedData));
            }

        }
        /// <summary>
        /// 将传入的二进制字符串资料以GZip算法解压缩
        /// </summary>
        /// <param name="zippedString">经GZip压缩后的二进制字符串</param>
        /// <returns>原始未压缩字符串</returns>
        public static string GZipDecompressString(byte[] zippedByte)
        {

            return (string)(System.Text.Encoding.UTF8.GetString(Decompress(zippedByte)));
        }


        /// <summary>
        /// GZip压缩
        /// </summary>
        /// <param name="rawData"></param>
        /// <returns></returns>
        public static byte[] Compress(byte[] rawData)
        {
            MemoryStream ms = new MemoryStream();
            GZipStream compressedzipStream = new GZipStream(ms, CompressionMode.Compress, true);
            compressedzipStream.Write(rawData, 0, rawData.Length);
            compressedzipStream.Close();
            return ms.ToArray();
        }
        /// <summary>
        /// 解压
        /// </summary>
        /// <param name="zippedData"></param>
        /// <returns></returns>
        public static byte[] Decompress(byte[] zippedData)
        {
            MemoryStream ms = new MemoryStream(zippedData);
            GZipStream compressedzipStream = new GZipStream(ms, CompressionMode.Decompress);
            MemoryStream outBuffer = new MemoryStream();
            byte[] block = new byte[1024];
            while (true)
            {
                int bytesRead = compressedzipStream.Read(block, 0, block.Length);
                if (bytesRead <= 0)
                    break;
                else
                    outBuffer.Write(block, 0, bytesRead);
            }
            compressedzipStream.Close();
            return outBuffer.ToArray();
        }
    }
    #region Abstract WebSock 位完成
    //public interface AbstractWebSockApi
    //{
    //    public string WebSocketAddress;
    //    WebSocket4Net.WebSocket websocket;
    //    public abstract Dictionary<string, string> topicDic { private set; get; }

    //    public bool isOpened { private set; get; }


    //    #region 訂閱
    //    /// <summary>
    //    /// 訂閱
    //    /// </summary>
    //    /// <param name="topic"></param>
    //    /// <param name="id"></param>
    //    public void Subscribe(string topic)
    //    {
    //        if (topicDic.ContainsKey(topic))
    //            return;

    //        var data = new HuobiSubscribe(topic, Pid); ///request Class   //var msg = $"{{\"sub\":\"{topic}\",\"id\":\"{id}\"}}";  //.net 7.0 
    //        var msg = JsonHelper.SerializeObject(data);


    //        _message = string.Format("Subscribe Msg: {0} ", msg);
    //        //ConsoleMessage(string.Format("Subscribe Msg: {0} ", msg));

    //        topicDic.Add(topic, msg);
    //        if (isOpened)
    //        {
    //            SendSubscribeTopic(msg);
    //        }
    //    }

    //    /// <summary>
    //    /// 取消订阅
    //    /// </summary>
    //    /// <param name="topic"></param>
    //    /// <param name="id"></param>
    //    public void UnSubscribe(string topic)
    //    {
    //        /*{
    //       *    "unsub": "market.btcusdt.trade.detail",
    //       *    "id": "id4"}
    //       */
    //        if (!topicDic.ContainsKey(topic) || isOpened == false)
    //            return;

    //        var data = new HuobiUnSubscribe(topic, Pid);
    //        var msg = JsonHelper.SerializeObject(data); //var msg = $"{{\"unsub\":\"{topic}\",\"id\":\"{id}\"}}";

    //        topicDic.Remove(topic);
    //        SendSubscribeTopic(msg);

    //        ////ConsoleMessage(msg);////NotifyMessage(msg);
    //    }

    //    public void UnSubscribe(string topic, string id)
    //    {
    //        /*{
    //       *    "unsub": "market.btcusdt.trade.detail",
    //       *    "id": "id4"}
    //       */
    //        if (!topicDic.ContainsKey(topic) || isOpened == false)
    //            return;

    //        var data = new HuobiUnSubscribe(topic, id);
    //        var msg = JsonHelper.SerializeObject(data); //var msg = $"{{\"unsub\":\"{topic}\",\"id\":\"{id}\"}}";

    //        topicDic.Remove(topic);
    //        SendSubscribeTopic(msg);

    //        ////ConsoleMessage(msg);////NotifyMessage(msg);
    //    }

    //    private void SendSubscribeTopic(string msg)
    //    {
    //        websocket.Send(msg);
    //        //ConsoleMessage(msg);
    //    }

    //    #endregion


    //}
        #endregion
}
