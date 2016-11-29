using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebApplication1.Utils
{

    //在使用log4net的工程文件下的AssemblyInfo上添加下面一段
    //[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Config/log4net.config", Watch = true)]

    /// <summary>
    /// 日志
    /// 自带日志时间
    /// </summary>
    public class log4netHelper
    {
        /// <summary>
        /// 异常日志信息
        /// </summary>
        /// <param name="strMessage">异常日志信息</param>
        public static void Exception(string strMessage)
        {
            log4net.ILog log = log4net.LogManager.GetLogger("Exception");
            if (log.IsErrorEnabled)
            {
                log.Error(strMessage);
            }
            log = null;
        }

        /// <summary>
        /// 普通的日志信息
        /// </summary>
        /// <param name="message">日志内容</param>
        public static void Info(string message)
        {
            log4net.ILog log = log4net.LogManager.GetLogger("InfoLog");
            if (log.IsInfoEnabled)
            {
                log.Info(message);
            }
            log = null;
        }

        public static void Exception(Exception ex)
        {
            log4net.ILog log = log4net.LogManager.GetLogger("Exception");
            if (log.IsErrorEnabled)
            {
                log.Error("error",ex);
            }
            log = null;
        }

        /// <summary>
        /// 请求日志信息
        /// </summary>
        /// <param name="id">请求页面</param>
        /// <param name="ip">请求者  </param>
        /// /// <param name="ip">请求IP </param>
        /// <param name="strParam">请求时传入的参数</param>
        public static void RequestLog(string strPage, string strName, string strIp, string strParam)
        {
            log4net.ILog log = log4net.LogManager.GetLogger("Request");
            if (log.IsInfoEnabled)
            {
                string strMessage = string.Format("请求页面:{0}\r\n请求者:{1}\r\n请求IP:{2}\r\n参数:{3}", strPage, strName, strIp, strParam);
                log.Info(strMessage);
            }
            log = null;
        }

        /// <summary>
        /// 处理结果日志
        /// </summary>
        /// <param name="strPage">请求页面</param>
        /// <param name="strName">请求者</param>
        /// /// <param name="strIp">请求IP</param>
        /// <param name="strResult">处理结果</param>
        public static void ResultLog(string strPage, string strName, string strIp, string strResult)
        {
            log4net.ILog log = log4net.LogManager.GetLogger("Result");
            if (log.IsInfoEnabled)
            {
                string strMessage = string.Format("请求页面:{0}\r\n请求者:{1}\r\n请求IP:{2}\r\n处理结果:{3}", strPage, strName, strIp, strResult);
                log.Info(strMessage);
            }
            log = null;
        }

        /// <summary>
        /// Post请求的时候出错
        /// </summary>
        /// <param name="message"></param>
        public static void PostLog(string message)
        {
            log4net.ILog log = log4net.LogManager.GetLogger("POST");
            if (log.IsInfoEnabled)
            {
                log.Info(message);
            }
            log = null;
        }

    }
}
