using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Utils;
using WxAppService;
using WxModel.DtoModel;

namespace WebApplication1.Areas.Admin.Controllers
{
    public class VoteCountController : Controller
    {
        // GET: Admin/VoteCount
        public ActionResult Index()
        {
            var deptApp = new DeptVoteAppService();

            var list = deptApp.GetDeptDic(0);
            ViewBag.ParentDept = new SelectList(list, "Id", "Name");
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
                var voteApp=new VoteCountAppService();

                if (id == 30)
                {
                    result.Data = voteApp.GetStoreVoteCountByDeptId(id);
                }
                else
                {
                    result.Data= voteApp.GetVoteCountByDeptId(id);
                }

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