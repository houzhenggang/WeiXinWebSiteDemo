using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Newtonsoft.Json.Linq;
using WebApplication1.Models;
using WxUtils;

namespace WebApplication1.Utils
{
    public class WeiXinHelper
    {
        /// <summary>
        /// 获取接口访问必须的access_token
        /// </summary>
        /// <returns></returns>
        public static string GetApiToken()
        {
            string url =
                string.Format(
                    "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", WeiXinAppKey.WeixinAppid, WeiXinAppKey.WeixinSecret);

            var client = new System.Net.WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            string data = client.DownloadString(url);

            var resultObj = JObject.Parse(data);
            if (resultObj["errcode"] == null)
            {
                string accessToken = resultObj["access_token"].ToString();
                int expires_in = int.Parse(resultObj["expires_in"].ToString());

                CacheHelper.AddCacheObj(WeiXinAppKey.ApiTokenCacheKey, accessToken, expires_in);
                return accessToken;
            }

            return string.Empty;
        }

        /// <summary>
        /// 获取jsapi接口的token
        /// </summary>
        /// <returns></returns>
        public static string GetJsApiToken()
        {
           var apiTokenCache = CacheHelper.GetCacheObj(WeiXinAppKey.ApiTokenCacheKey);
            if (apiTokenCache == null)
            {
                apiTokenCache = WeiXinHelper.GetApiToken();
            }
            string url =
                string.Format("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi",apiTokenCache);

            var client = new System.Net.WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            string data = client.DownloadString(url);

            var resultObj = JObject.Parse(data);
            if (resultObj["errcode"].ToString() == "0")
            {
                string jsApiToken = resultObj["ticket"].ToString();
                int expires_in = int.Parse(resultObj["expires_in"].ToString());

                CacheHelper.AddCacheObj(WeiXinAppKey.JsApiTokenCacheKey, jsApiToken, expires_in);
                return jsApiToken;
            }
            return string.Empty;
        }

        /// <summary>
        /// DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time"> DateTime时间格式</param>
        /// <returns>Unix时间戳格式</returns>
        public static int ConvertDateTimeToTimeSpan(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }

        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name="timeStamp">Unix时间戳格式</param>
        /// <returns>C#格式时间</returns>
        public static DateTime GetTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        /// <summary>
        /// 签名算法
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetSha1(string str)
        {
            //建立SHA1对象
            SHA1 sha = new SHA1CryptoServiceProvider();
            //将mystr转换成byte[] 
            ASCIIEncoding enc = new ASCIIEncoding();
            byte[] dataToHash = enc.GetBytes(str);
            //Hash运算
            byte[] dataHashed = sha.ComputeHash(dataToHash);
            //将运算结果转换成string
            string hash = BitConverter.ToString(dataHashed).Replace("-", "");
            return hash;
        }
    }
}