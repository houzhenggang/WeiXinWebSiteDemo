using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WxModel.DtoModel
{
    public class VoteDeptDto
    {
        public VoteDeptDto()
        {
            this.DeptImgs=new List<DeptImg>();
        }
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string DelptName { get; set; }
        public string CoverImg { get; set; }

        /// <summary>
        /// Default:((0)) 
        /// 事业部:0, 片区：1，门店:2
        /// </summary>
        public int DeptType { get; set; }
        public string DeptSummary { get; set; }
        public string ImgIds { get; set; }
        public string TeamImg { get; set; }


        public List<DeptImg> DeptImgs { get; set; } 
    }
}
