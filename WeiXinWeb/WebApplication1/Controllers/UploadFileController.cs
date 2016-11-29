using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Utils;
using WxAppService;

namespace WebApplication1.Controllers
{
    public class UploadFileController : Controller
    {
        // GET: UploadFile
        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public JsonResult RentExcelImport(string parentId)
        {
            string message = "";
            try
            {
                HttpFileCollection filedata = System.Web.HttpContext.Current.Request.Files;
                if (filedata.Count > 0 && filedata[0].ContentLength > 0)
                {
                    //string path = AppDomain.CurrentDomain.BaseDirectory + "uploads/excel/";
                    // savePath = Path.Combine(path, FileName);
                    //创建图片路径

                    var urlpath = "/UploadFile/Import/temp/";

                    var dirpath = Server.MapPath(urlpath);
                    if (!Directory.Exists(dirpath))
                    {
                        Directory.CreateDirectory(dirpath);
                    }
                    string fileName = filedata[0].FileName;
                    int index = fileName.LastIndexOf('.');
                    string extName = fileName.Substring(index);

                    string serverPath = dirpath + DateTime.Now.ToString("yyyyMMddhhMMss") + extName;
                    //serverPath = FileHelper.GetNewPathForDupes(serverPath);
                    //urlpath = urlpath + Path.GetFileName(serverPath);
                    //_clothImgApp.SvaeClothImgData(id,urlpath,filedata.ContentLength);
                    filedata[0].SaveAs(serverPath);
                    //上传成功后保存数据

                    // return Json(new { ret = "0", message }, JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception ex)
            {
                log4netHelper.Exception(ex);
            }

            return Json(true);
        }

        public JsonResult UploadDeptImg(int imgType)
        {
            
            try
            {
                 HttpFileCollection filedata = System.Web.HttpContext.Current.Request.Files;
                if (filedata.Count > 0 && filedata[0].ContentLength > 0)
                {
                    var dtStr = DateTime.Now.ToString("yyyyMMdd");
                    var baseUrl = "/UploadFile/Image/DeptImg/" + dtStr + "/";
                    var dirpath = Server.MapPath(baseUrl);
                    if (!Directory.Exists(dirpath))
                    {
                        Directory.CreateDirectory(dirpath);
                    }

                    string fileName = filedata[0].FileName;
                    int index = fileName.LastIndexOf('.');
                    string extName = fileName.Substring(index);
                    string guidFileName = Guid.NewGuid().ToString("N")+extName;
                    string serverPath = dirpath + guidFileName;

                    filedata[0].SaveAs(serverPath);
                     var deptImgApp=new DeptImgAppService();
                     int imgid = deptImgApp.AddDeptImg(guidFileName, baseUrl + guidFileName, imgType);
                     return Json(new { Ret = true, imgId = imgid, imgUrl = baseUrl + guidFileName });
                }

            }
            catch (Exception ex)
            {
               log4netHelper.Exception(ex);
               return Json(new { Ret = false });
            }
            return Json(new { Ret = false });
        }

    }
}