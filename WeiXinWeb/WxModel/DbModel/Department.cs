using System;
using System.Linq;
using System.Text;

namespace WxModel
{
    public class Department
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
        public string Name {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:((0)) 
        ///事业部:0, 片区：1，门店:2
        /// Nullable:False 
        /// </summary>
        public int DeptType {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string DeptSummary {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:((0)) 
        /// Nullable:False 
        /// </summary>
        public int ParentId {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public DateTime InsertDate {get;set;}

    }
}
