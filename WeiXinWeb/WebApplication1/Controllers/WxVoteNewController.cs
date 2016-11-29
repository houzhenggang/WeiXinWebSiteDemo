using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using WebApplication1.Models;
using WebApplication1.Utils;
using WxAppService;
using WxModel.DtoModel;

namespace WebApplication1.Controllers
{
    public class WxVoteNewController : Controller
    {
        private string weixinAppid = WeiXinAppKey.WeixinAppid;
        private string weixinSecret = WeiXinAppKey.WeixinSecret;
        private string webToken = WeiXinAppKey.WebToken;
        private string apiTokenKey = WeiXinAppKey.ApiTokenCacheKey;
        private string jsApiTokenKey = WeiXinAppKey.JsApiTokenCacheKey;
        //public VoteController()
        //{
        //    log4netHelper.Info("访问测试");


        //}
        public WxVoteNewController()
        {
            var tspan = WeiXinHelper.ConvertDateTimeToTimeSpan(DateTime.Now);
            var nonceStr = Guid.NewGuid().ToString("N");

            var jsApiToken = CacheHelper.GetCacheObj(jsApiTokenKey);
            if (jsApiToken == null)
            {
                jsApiToken = WeiXinHelper.GetJsApiToken();
            }
            string url = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;

            string signStr = string.Format("jsapi_ticket={0}&noncestr={1}&timestamp={2}&url={3}", jsApiToken, nonceStr,
                tspan, url);
            //var jssignature= FormsAuthentication.HashPasswordForStoringInConfigFile(signStr, "SHA1").ToLower();
            var jssignature = WeiXinHelper.GetSha1(signStr).ToLower();
            //log4netHelper.Info("singStr="+ signStr);
            //log4netHelper.Info("jssignature=" + jssignature);
            ViewBag.AppId = weixinAppid;
            ViewBag.Tspan = tspan;
            ViewBag.NonceStr = nonceStr;
            ViewBag.Jssignature = jssignature;
            //ViewBag.CurrentUrl = url;
        }

        public ActionResult Index()
        {
            var code = System.Web.HttpContext.Current.Request["code"];
            //string openId = string.Empty;
            if (!string.IsNullOrEmpty(code))
            {
                var userAccess = GetOpenId(code);
                if (userAccess != null)
                {
                    bool isFollow = GetUserInfo(userAccess);
                    if (!isFollow)
                    {
                        ViewBag.OpenId = "0";
                    }
                    else
                    {
                        ViewBag.OpenId = userAccess.openid;
                    }

                }

            }
            else
            {
                ViewBag.OpenId = "0";
                //string weixinAppid = System.Configuration.ConfigurationManager.AppSettings["weixinAppid"];
                string weixinBackUrl = System.Configuration.ConfigurationManager.AppSettings["weixinBackUrl"];

                //string encodeBackUrl =HttpUtility.UrlEncode(weixinBackUrl);
                var url = string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_base&state=STATE#wechat_redirect", weixinAppid, weixinBackUrl);
                return Redirect(url);

            }
            return View();
        }

        /// <summary>
        /// 根据code获取用的openid
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public AccessTokenwx GetOpenId(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return null;
            }

            string url = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", weixinAppid, weixinSecret, code); //weixinAppid和weixinSecret为公众号的appID和appsecret

            var client = new System.Net.WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            string data = client.DownloadString(url);
            //log4netHelper.Info("openidData:"+data);
            JavaScriptSerializer Jss = new JavaScriptSerializer();
            AccessTokenwx userInfo = Jss.Deserialize<AccessTokenwx>(data);
            return userInfo;
        }

        /// <summary>
        /// 根据openid获取用户基本信息 判断用否已经关注
        /// </summary>
        /// <param name="wxUser"></param>
        /// <returns></returns>
        public bool GetUserInfo(AccessTokenwx wxUser)
        {
            if (string.IsNullOrEmpty(wxUser.openid))
            {
                return false;
            }

            var apiTokenCache = CacheHelper.GetCacheObj(apiTokenKey);
            if (apiTokenCache == null)
            {
                apiTokenCache = WeiXinHelper.GetApiToken();
            }
            string url =
                string.Format(
                    "https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang=zh_CN", apiTokenCache, wxUser.openid);

            HttpClient httpClient = new HttpClient();
            Task<HttpResponseMessage> respMsg = httpClient.GetAsync(url);

            var respResult = respMsg.Result;
            if (respResult.IsSuccessStatusCode)
            {
                string result = respResult.Content.ReadAsStringAsync().Result;
                //log4netHelper.Info("userinfodata:"+result);
                var resultObj = JObject.Parse(result);
                if (resultObj["errcode"] == null)
                {
                    if (resultObj["subscribe"].ToString() == "1")
                    {
                        return true;
                    }
                }
            }
            return false;

        }


        /// <summary>
        /// 提交投票
        /// </summary>
        /// <returns></returns>
        public JsonResult SubmitVote()
        {
            var result = new ResultJson();
            try
            {
                var deptId = RequestHelper.RequestPost("deptId", "0");
                var openId = RequestHelper.RequestPost("openId", "0");

                if (deptId == "0" || openId == "0")
                {
                    result.Msg = "参数错误，无法投票";

                }
                else
                {
                    string dateLine = ConfigurationManager.AppSettings["DateLine"];

                    var dtdateline = DateTime.Parse(dateLine);
                    if (DateTime.Compare(dtdateline, DateTime.Now) > 0)
                    {
                        int ideptId = int.Parse(deptId);

                        var voteApp = new VoteCountAppService();
                        var respInt = voteApp.IsVotedToday(ideptId, openId);
                        if (respInt == 0)
                        {
                            result.Msg = "今天已经投票,不能重复投票";
                        }
                        else
                        {
                            result.Msg = "投票成功,明天可以继续投票";
                            
                        }
                        result.Ret = true;
                        result.VoteStatus = respInt;
                    }
                    else
                    {
                        result.Msg = "投票活动已经过期";
                    }

                }

            }
            catch (Exception ex)
            {
                log4netHelper.Exception(ex.ToString());
                result.Msg = "服务器异常,请稍后再试";
            }

            return Json(result);
        }


        /// <summary>
        /// 部门投票入口
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public ActionResult VoteHome(string openId)
        {

            var list=new List<VoteDeptDto>();
            try
            {
                //获取门店的部门封面
                var deptApp=new DeptVoteAppService();
                list = deptApp.GetDeptTitleByPid(30);

                //获取大众房产和房网的封面图片
                //var ids = new int[] {28, 29};
                //var fdfxImg = deptApp.GetDeptCover(ids);

            }
            catch (Exception ex)
            {
               log4netHelper.Exception(ex);
            }

            ViewBag.OpenId = openId;
            ViewData.Model = list;
            return View();
        }

        /// <summary>
        /// 投票页面
        /// </summary>
        /// <param name="deptId"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        public ActionResult VoteContent(int deptId,string openId)
        {
            var data = new VoteDetailDto();
            try
            {
                var deptApp = new DeptVoteAppService();
                data = deptApp.GetVoteDetailByPartId(deptId);
            }
            catch (Exception ex)
            {
                log4netHelper.Exception(ex);
            }
            ViewBag.OpenId = openId;
            ViewData.Model = data;
            return View();
        }

        /// <summary>
        /// 部门风采展示
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeptShow(int id)
        {
            var data = new DeptShow();
            try
            {
                var deptApp = new DeptImgAppService();
                data= deptApp.GetDeptImgUrlByDeptId(id);
            }
            catch (Exception ex)
            {
                log4netHelper.Exception(ex);
            }
            ViewData.Model = data;
            return View();
        }

        /// <summary>
        /// 投票排名
        /// </summary>
        /// <returns></returns>
        public ActionResult DeptRank(int deptId)
        {
           var data=new List<VoteCountDto>();
            try
            {
                var voteApp = new VoteCountAppService();

                if (deptId == 30)
                {
                    data = voteApp.GetStoreVoteCountByDeptId(deptId);
                }
                else
                {
                    data = voteApp.GetVoteCountByDeptId(deptId);
                }

                data = data.OrderByDescending(s => s.VCount).ToList();
            }
            catch (Exception ex)
            {
                log4netHelper.Exception(ex);
            }
            ViewData.Model = data;
            return View();
        }


        /// <summary>
        /// 根据部门获取投票排行榜
        /// </summary>
        /// <returns></returns>
        public JsonResult GetVoteCountByDeptId(int id)
        {
            var result = new ResultJson<List<VoteCountDto>>();
            try
            {
                var voteApp = new VoteCountAppService();

                if (id == 30)
                {
                    result.Data = voteApp.GetStoreVoteCountByDeptId(id);
                }
                else
                {
                    result.Data = voteApp.GetVoteCountByDeptId(id);
                }

                result.Ret = true;
            }
            catch (Exception ex)
            {
                log4netHelper.Exception(ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}