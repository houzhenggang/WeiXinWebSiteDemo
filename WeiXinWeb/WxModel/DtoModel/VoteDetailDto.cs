using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxModel.DtoModel
{
   public class VoteDetailDto
    {
       public VoteDetailDto()
       {
           this.Detial=new List<VoteDetailSonDto>();
       }
        public string PartName { get; set; }
        public List<VoteDetailSonDto> Detial { get; set; }
    }

    public class VoteDetailSonDto
    {
        public int DeptId { get; set; }
        public string DeptName { get; set; }
        public int VoteCount { get; set; }
        public string CoverImg { get; set; }
    }

    public class VoteCountDto
    {
        public int DeptId { get; set; }
        public string DeptName { get; set; }
        public string PartName { get; set; } = "";
        public int VCount { get; set; }
    }

    public class DeptCoverDto
    {
        public int DeptId { get; set; }
        public string CoverImg { get; set; }
    }

    public class VoteRankDto
    {
        public string DeptName { get; set; }
        public string VCount { get; set; }
    }

    public class DeptShow
    {
        public DeptShow()
        {
            this.ImgUrl=new List<string>();
        }
        public string DeptSummary { get; set; } = "";
        public List<string> ImgUrl { get; set; } 
    }
}
