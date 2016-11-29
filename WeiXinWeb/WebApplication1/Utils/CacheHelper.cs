using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace WebApplication1.Utils
{
    public class CacheHelper
    {
        /// <summary>
        /// 设置缓存    
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expires_in">过期时间秒数</param>
        public static void AddCacheObj(string key, object value, int expires_in)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Add(key, value, null, DateTime.Now.AddSeconds(expires_in), TimeSpan.Zero,
                   CacheItemPriority.Normal, null);
            //Cache.Add("ApiToken", accessToken, null, DateTime.Now.AddSeconds(expires_in), TimeSpan.Zero,
            //    CacheItemPriority.Normal);
        }

        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object GetCacheObj(string key)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            return objCache[key];
        }
    }
}