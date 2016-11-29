﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Utils
{
    public class RequestHelper
    {
        public static string RequestGet(string key, string defVal)
        {
            //string val = string.Empty;
            if (HttpContext.Current.Request.QueryString[key] != null)
            {
                defVal = HttpContext.Current.Request.QueryString[key];
            }

            return defVal;
        }

        public static string RequestPost(string key, string defVal)
        {
            if (HttpContext.Current.Request.Form[key] != null)
            {
                defVal = HttpContext.Current.Request.Form[key];
            }

            return defVal;
        }
    }
}