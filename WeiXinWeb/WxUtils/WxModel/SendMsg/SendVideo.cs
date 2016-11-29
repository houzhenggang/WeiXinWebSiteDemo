
using System;

namespace WxUtils.WxModel.SendMsg
{
    public class SendVideo : BaseModel
    {

        /// <summary>
        ///  通过上传多媒体文件，得到的id
        /// </summary>
        /// Author  : 123
        /// Company : 123
        /// Created : 2014-10-10 10:50:27
        public int MediaId { get; set; }

        /// <summary>
        ///  视频消息的标题
        /// </summary>
        /// Author  : 123
        /// Company : 123
        /// Created : 2014-10-10 10:50:27
        public string Title { get; set; }

        /// <summary>
        ///  视频消息的描述
        /// </summary>
        /// Author  : 123
        /// Company : 123
        /// Created : 2014-10-10 10:50:27
        public string Description { get; set; }

    }
}
