using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication1.Models;
using WebApplication1.Utils;
using WxUtils;
using WxUtils.WxModel;
using WxUtils.WxModel.SendMsg;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private string Token = "yushuowechat";


        public ActionResult Test()
        {
            return View();
        }

        [HttpGet]
        public void Index(WebChatModel model)
        {
            string echoStr = model.echostr;
            //通过验证
            if (CheckSignature(model))
            {
                if (!string.IsNullOrEmpty(echoStr))
                {
                    ////将随机生成的 echostr 参数 原样输出
                    Response.Write(echoStr);
                    ////截止输出流
                    Response.End();

                }
            }
            //return echoStr;
        }


        [HttpPost]
        public ContentResult Index()
        {
            string responeData = string.Empty;
            string _xmlData = string.Empty;
            //将xml数据转换
            using (Stream stream = HttpContext.Request.InputStream)
            {
                byte[] byteData = new byte[stream.Length];
                stream.Read(byteData, 0, (Int32)stream.Length);
                _xmlData = Encoding.UTF8.GetString(byteData);
            }
            //判断传输过来的xml是否有数据
            if (!string.IsNullOrEmpty(_xmlData))
            {
                log4netHelper.Info(_xmlData);
                Dictionary<string, string> model = new Dictionary<string, string>();
                try
                {
                    model = _xmlData.ReadXml();
                    if (model.Count > 0)//回复消息
                    {
                       responeData= ExecuteMsg(model);
                    }
                }
                catch (Exception ex)
                {
                   log4netHelper.Exception(ex);
                    //OperateXml.ResponseEnd("出错了！！！");
                }
               
            }

            return Content(responeData);
        }
        /// <summary>
        /// 验证签名，检验是否是从微信服务器上发出的请求
        /// </summary>
        /// <param name="model">请求参数模型 Model</param>
        /// <returns>是否验证通过</returns>
        private bool CheckSignature(WebChatModel model)
        {
            string signature, timestamp, nonce, tempStr;
            //获取请求来的参数
            signature = model.signature;
            timestamp = model.timestamp;
            nonce = model.nonce;
            //创建数组，将 Token, timestamp, nonce 三个参数加入数组
            string[] array = { Token, timestamp, nonce };
            //进行排序
            Array.Sort(array);
            //拼接为一个字符串
            tempStr = String.Join("", array);
            //对字符串进行 SHA1加密
            tempStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tempStr, "SHA1").ToLower();
            //判断signature 是否正确
            if (tempStr.Equals(signature))
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        public string ExecuteMsg(Dictionary<string, string> model)
        {
            //通过MsgType判断是普通消息还是事件推送
            string msgType = model.ReadKey("MsgType");
            string responseData = string.Empty;
            switch (msgType)
            {

                #region 普通消息

                case "text": //文本消息
                    responseData = HandleTextMsg(model);
                    //responseData=SendMsg.ReplyTexts(model,"测试一下吧");
                    break;
                case "image": //图片
                    break;
                case "voice": //声音
                    break;
                case "video": //视频
                    break;
                case "location": //地理位置
                    break;
                case "link": //链接
                    break;

                #endregion

                #region 事件推送

                case "event":
                    //事件类型
                    msgType = model.ReadKey("Event");
                    switch (msgType)
                    {
                        case "subscribe": //关注
                            /*if (model.ReadKey(PublicField.EventKey).StartsWith("qrscene_"))
                            {
                                //带参数的二维码扫描关注
                            }
                            else
                            {
                                //普通关注
                            }*/
                            SendMsg.ReplaySubscribeText(model);
                            break;
                        case "unsubscribe": //取消关注
                            break;
                        case "SCAN": //已经关注的用户扫描带参数的二维码
                            break;
                        case "LOCATION": //用户上报地理位置
                            break;
                        case "CLICK": //自定义菜单点击
                            break;
                        case "VIEW": //自定义菜单跳转事件
                            break;
                        case "scancode_push": //扫码推事件的事件推送
                            break;
                        case "scancode_waitmsg": //扫码推事件且弹出“消息接收中”提示框的事件推送
                            //  SendMsg.ReplyScanCodeWaitmsg(model);
                            //SendMsg.ReplayTemplete(model);
                            break;
                        case "pic_sysphoto": //弹出系统拍照发图的事件推送
                            break;
                        case "pic_photo_or_album": //弹出拍照或者相册发图的事件推送
                            break;
                        case "pic_weixin": //弹出微信相册发图器的事件推送
                            break;
                        case "location_select": //弹出地理位置选择器的事件推送
                            break;
                    }
                    break;

                    #endregion
            }

            return responseData;
        }

        private string HandleTextMsg(Dictionary<string, string> dic)
        {
            
            string msg = "";
            string url= ConfigurationManager.AppSettings["weixinBackUrl"];
            if (dic.ReadKey(PublicField.Content) == "投票")
            {
                var model = new ArticlesModel();
                model.Title = "嘀嘀租房杯,发现人气团队投票评选";
                model.Description = "嘀嘀租房杯,发现人气团队投票评选";
                model.PicUrl = "http://cs.didizufang.com.cn:9002/WeiXinImg/testWeixin.png";
                model.Url = url;
                var list = new List<ArticlesModel>();
                list.Add(model);
                msg = SendMsg.ReplyNews(dic, list);

            }
            else
            {
                msg =SendMsg.ReplyTexts(dic, "回复[投票]获取活动投票地址");
            }

            return msg;
        }


    }
}