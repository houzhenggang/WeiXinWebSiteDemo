using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebApplication1.Areas.Admin.Models;
using WebApplication1.Models;
using WebApplication1.Utils;

namespace WebApplication1.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        private string weixinAppid = WeiXinAppKey.WeixinAppid;
        private string weixinSecret = WeiXinAppKey.WeixinSecret;
        private string webToken = WeiXinAppKey.WebToken;
        //private string apiTokenKey = "ApiTokenKey";
        // GET: Admin/Admin
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult SideBarMenu()
        {

            return PartialView();
        }


        public ActionResult WxMenu()
        {
            return View();
        }


        public JsonResult CreateMenu()
        {
            var result = new ResultJson();
            try
            {
                Wx_Menu Wx_Menus = new Wx_Menu();
                Wx_Menus.menu.button = new button[3];

                //button sonbutton = new button { name = "子菜单1", key = "menu2", type = "click" };

                button Button1 = new button
                {
                    name = "🌐官网",
                    key = "Home",
                    type = "view",
                    url = "http://www.didizufang.com.cn"
                };
                button Button2 = new button
                {
                    name = "租房app",
                    key = "App",
                    type = "view",
                    url = "http://a.app.qq.com/o/simple.jsp?pkgname=com.example.mydidizufang"
                };
                button Button3 = new button
                {
                    name = "投票",
                    key = "menu2",
                    type = "view",
                    url = "http://cs.didizufang.com.cn:9002/vote/RequestApi"
                };
                //Button3.sub_button = new button[1];
                //Button3.sub_button[0] = sonbutton;


                Wx_Menus.menu.button[0] = Button1;
                Wx_Menus.menu.button[1] = Button2;
                Wx_Menus.menu.button[2] = Button3;
                string PostParam = JsonConvert.SerializeObject(Wx_Menus.menu);

                log4netHelper.Info(PostParam);
                //var apiTokenCache = CacheHelper.GetCacheObj(WeiXinAppKey.ApiTokenCacheKey);
                //if (apiTokenCache == null)
                //{
                //    apiTokenCache = WeiXinHelper.GetApiToken();
                //}
                //HttpClient httpClient=new HttpClient();
                //string url = string.Format("https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}", apiTokenCache);

                //HttpContent content=new ByteArrayContent(Encoding.UTF8.GetBytes(PostParam));
                //Task<HttpResponseMessage> respMsg= httpClient.PostAsync(url, content);
                //if (respMsg.Result.IsSuccessStatusCode)
                //{
                //    string resultStr = respMsg.Result.Content.ReadAsStringAsync().Result;
                //    log4netHelper.Info(resultStr);
                //    var resultObj = JObject.Parse(resultStr);
                //    string errcode = resultObj["errcode"].ToString();
                //    if (errcode == "0")
                //       result.Ret = true;
                //}
            }
            catch (Exception ex)
            {
                log4netHelper.Exception(ex.ToString());
            }

            return Json(result);
        }


        public JsonResult CreateMenuDefault(string menuStr)
        {
            var result = new ResultJson();
            try
            {
                if (!string.IsNullOrEmpty(menuStr))
                {
                    var apiTokenCache = CacheHelper.GetCacheObj(WeiXinAppKey.ApiTokenCacheKey);
                    if (apiTokenCache == null)
                    {
                        apiTokenCache = WeiXinHelper.GetApiToken();
                    }
                    HttpClient httpClient = new HttpClient();
                    string url = string.Format("https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}", apiTokenCache);

                    HttpContent content = new ByteArrayContent(Encoding.UTF8.GetBytes(menuStr));
                    Task<HttpResponseMessage> respMsg = httpClient.PostAsync(url, content);
                    if (respMsg.Result.IsSuccessStatusCode)
                    {
                        string resultStr = respMsg.Result.Content.ReadAsStringAsync().Result;
                        log4netHelper.Info(resultStr);
                        var resultObj = JObject.Parse(resultStr);
                        string errcode = resultObj["errcode"].ToString();
                        if (errcode == "0")
                            result.Ret = true;
                    }

                }
              
            }
            catch (Exception ex)
            {
                log4netHelper.Exception(ex.ToString());
            }

            return Json(result);
        }
    }
}