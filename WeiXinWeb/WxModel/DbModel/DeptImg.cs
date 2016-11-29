using System;
using System.Linq;
using System.Text;

namespace WxModel
{
    public class DeptImg
    {
        
        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public int Id {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string ImgName {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string ImgSummary {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string ImgUrl {get;set;}

        /// <summary>
        /// 0：风采照片,1:封面照片
        /// </summary>
        public int ImgType { get; set; }
        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public DateTime InsertDate {get;set;}

        public int ParentId { get; set; }

    }
}
