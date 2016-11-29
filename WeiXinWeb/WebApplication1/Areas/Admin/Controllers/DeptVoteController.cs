using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Controllers.Base;
using WebApplication1.Models;
using WebApplication1.Utils;
using WxAppService;
using WxModel;
using WxModel.DtoModel;

namespace WebApplication1.Areas.Admin.Controllers
{
    public class DeptVoteController : BaseController
    {
        // GET: Admin/Vote
        public ActionResult Index()
        {
            var deptApp = new DeptVoteAppService();

            var list = deptApp.GetDeptDic(0);
            ViewBag.ParentDept = new SelectList(list, "Id", "Name");

            return View();
        }

        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        public JsonResult AddDeptVote(VoteDeptDto model)
        {
           // HttpFileCollection filedata = System.Web.HttpContext.Current.Request.Files;
            //var reqFile = HttpContext.Request.Files;

            var result=new ResultJson();
            try
            {

                var deptApp = new DeptVoteAppService();
                model.DeptType = 2;
                 deptApp.AddDepartment(model);

                //文件保存处理
                //var baseUrl = "/UploadFile/Image/DeptImg/";
                //var dirpath = Server.MapPath(baseUrl);
                //if (!Directory.Exists(dirpath))
                //{
                //    Directory.CreateDirectory(dirpath);
                //}
                //if (!string.IsNullOrEmpty(model.Base64ImgCover))
                //{
                //    string imgCoverName = ImgHelper.Base64ToImg(model.Base64ImgCover, dirpath);
                //    //model.CoverImg = baseUrl + imgCoverName;

                //    var deptImgApp=new DeptImgAppService();
                //    deptImgApp.AddDeptImg(imgCoverName, baseUrl + imgCoverName,1);
                 
                //}


                result.Ret = true;
            }
            catch (Exception ex)
            {
                log4netHelper.Exception(ex);
            }
            return Json(result);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult DeleteDeptData(int id)
        {
            var result=new ResultJson();
            try
            {
                var deptApp=new DeptVoteAppService();
                deptApp.DeleteDeptData(id);
                result.Ret = true;
            }
            catch (Exception ex)
            {
                log4netHelper.Exception(ex);
            }

            return Json(result);
        }

        /// <summary>
        /// 根据id获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetDeptById(int id)
        {
            var result = new ResultJson<VoteDeptDto>();
            try
            {
                var deptApp = new DeptVoteAppService();
                var data = deptApp.GetDeptDataById(id);

                result.Data = data;
                result.Ret = true;
            }
            catch (Exception ex)
            {
                log4netHelper.Exception(ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult DeleteDeptImg(int id,string imgUrl)
        {
            var result=new ResultJson();
            try
            {
                var imgApp = new DeptImgAppService();
                imgApp.DeleteImg(id);

                result.Ret = true;
            }
            catch (Exception ex)
            {
                log4netHelper.Exception(ex);
            }
            finally
            {
                if (!string.IsNullOrEmpty(imgUrl))
                {
                    var serverUrl = Server.MapPath(imgUrl);
                    var file = new FileInfo(serverUrl);//指定文件路径 
                    if (file.Exists)//判断文件是否存在 
                    {
                        file.Attributes = FileAttributes.Normal;//将文件属性设置为普通,比方说只读文件设置为普通 
                        file.Delete();//删除文件 
                    }
                }
            }
            return Json(result);
        }


        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="pId"></param>
        /// <returns></returns>
        public JsonResult GetDeptByPid(int pId)
        {
            var result=new ResultJson<List<Department>>();
            try
            {
                var deptApp=new DeptVoteAppService();
                result.Data = deptApp.GetDeptByPid(pId);
                result.Ret = true;
            }
            catch (Exception ex)
            {
                log4netHelper.Exception(ex);
            }

            return Json(result,JsonRequestBehavior.AllowGet);
        }
    }
}