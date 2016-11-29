using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;
using WxAppService.DbContext;
using WxModel;
using WxModel.DtoModel;

namespace WxAppService
{
    public class DeptImgAppService : SqlRepository<DeptImg>
    {

        /// <summary>
        /// 
        /// 新增图片
        /// </summary>
        /// <param name="imgName"></param>
        /// <param name="imgUrl"></param>
        /// <param name="imgType"></param>
        public int AddDeptImg(string  imgName,string imgUrl,int imgType)
        {
            var imgModel = new DeptImg();
            imgModel.ImgName = imgName;
            imgModel.ImgType = imgType;
            imgModel.ImgUrl = imgUrl;
            imgModel.ImgSummary = string.Empty;
            imgModel.InsertDate=DateTime.Now;

            var id=InsertEntity(imgModel);
            return int.Parse(id.ToString());
        }


        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="id"></param>
        public void DeleteImg(int id)
        {
            Delete(s => s.Id == id);
        }


        public DeptShow GetDeptImgUrlByDeptId(int id)
        {
            var dept=new DeptShow();
            var deptSummary = _dbClient.Queryable<Department>().Where(s => s.Id == id).SingleOrDefault();
            if (deptSummary != null)
            {
                dept.DeptSummary = deptSummary.DeptSummary;
            }
            var deptImgs = _dbClient.Queryable<DeptImg>().Where(s =>s.ParentId==id && s.ImgType == 0).Select<string>("ImgUrl").ToList();

            dept.ImgUrl = deptImgs;
            return dept;
        } 
    }
}
