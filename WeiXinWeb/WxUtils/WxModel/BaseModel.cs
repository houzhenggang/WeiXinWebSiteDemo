﻿
namespace WxUtils.WxModel
{
    public class BaseModel
    {

        /// <summary>
        ///  开发者微信号
        /// </summary>
        /// Author  : 123
        /// Company : 123
        /// Created : 2014-10-09 21:03:38
        public string ToUserName { get; set; }

        /// <summary>
        ///  发送方帐号（一个OpenID）
        /// </summary>
        /// Author  : 123
        /// Company : 123
        /// Created : 2014-10-09 21:03:38
        public string FromUserName { get; set; }

        /// <summary>
        ///  消息创建时间 （整型）
        /// </summary>
        /// Author  : 123
        /// Company : 123
        /// Created : 2014-10-09 21:03:38
        public int CreateTime { get; set; }

        /// <summary>
        ///  消息类型(event/text/image/video/voice/location/link)
        /// </summary>
        /// Author  : 123
        /// Company : 123
        /// Created : 2014-10-09 21:03:38
        public string MsgType { get; set; }

        

    }
}
