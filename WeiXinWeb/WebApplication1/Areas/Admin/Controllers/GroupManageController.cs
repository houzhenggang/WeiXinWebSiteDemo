using System;
using System.Collections.Generic;
using System.Linq;
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
    public class GroupManageController : BaseController
    {
        // GET: Admin/GroupManage
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// x新增片区
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult AddGroup(VoteDeptDto model)
        {
            var result=new ResultJson();
            try
            {
                var deptApp = new DeptVoteAppService();
                if (model.Id == 0)
                {
                    deptApp.AddDepartment(model);

                }
                else
                {
                    deptApp.UpdateDeptDataById(model);
                }

                result.Ret = true;
            }
            catch (Exception ex)
            {
             log4netHelper.Exception(ex);
            }
            return Json(result);
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetGroupList(int pIndex,int pSize,int deptType)
        {
            var result=new GridList<Department>();
            try
            {
                var  deptApp=new DeptVoteAppService();

                result = deptApp.GetDeptList(pIndex, pSize, deptType);
                result.Ret= true;
            }
            catch (Exception ex)
            {
                log4netHelper.Exception(ex);
            }
            return JsonExForDateTime(result,"yyyy-MM-dd");
        }

        /// <summary>
        /// 片区管理
        /// </summary>
        /// <returns></returns>
        public ActionResult PartArea()
        {

            var deptApp=new DeptVoteAppService();

            var list=deptApp.GetDeptDic(0);
            ViewBag.ParentDept=new SelectList(list,"Id","Name");
            return View();
        }

    }
}