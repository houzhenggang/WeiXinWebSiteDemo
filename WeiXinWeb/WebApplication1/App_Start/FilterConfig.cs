using System.Web;
using System.Web.Mvc;
using WebApplication1.Utils;

namespace WebApplication1
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
            //filters.Add(new MyExceptionFilterAttribute());
        }
    }

    public class MyExceptionFilterAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);

            //处理错误消息。跳转到一个错误也页面
            ///using(Sql)
            log4netHelper.Exception(filterContext.Exception.ToString());

            //页面跳转到错误页面
           filterContext.HttpContext.Response.Redirect("/Views/Error/Error.chtml");

        }
    }
}
