using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers.Base
{
    public class BaseController : Controller
    {
        // GET: Base
        #region json时间格式转化
        protected virtual JsonResult JsonExForDateTime(object data, string dateTimeFormatStr = "yyyy-MM-dd HH:mm:ss", JsonRequestBehavior behavior = JsonRequestBehavior.AllowGet)
        {
            return new JsonResultDateTimeFormat()
            {
                Data = data,
                JsonRequestBehavior = behavior,
                DateTimeFormateStr = dateTimeFormatStr,
                ContentEncoding = Encoding.UTF8,
                ContentType = "text/plain",
            };
        }

        //protected virtual JsonResult JsonExForDateTime(object data, string contentType, string dateTimeFormatStr = "yyyy-MM-dd HH:mm:ss", JsonRequestBehavior behavior = JsonRequestBehavior.AllowGet)
        //{
        //    return new JsonResultDateTimeFormat()
        //    {
        //        Data = data,
        //        JsonRequestBehavior = behavior,
        //        DateTimeFormateStr = dateTimeFormatStr,
        //        ContentEncoding = Encoding.UTF8,
        //        ContentType = contentType,
        //    };
        //}

        #endregion
    }
}