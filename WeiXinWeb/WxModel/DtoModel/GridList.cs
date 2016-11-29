using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WxModel.DtoModel
{
    public class GridList<T>
    {
        public GridList()
        {
            this.size = 10;
            this.rows = new List<T>();
        }
        /// <summary>
        /// 总页数
        /// </summary>
        public int total { get; set; }
        /// <summary>
        /// 当前页码
        /// </summary>
        public int page { get; set; }
        /// <summary>
        /// 每页记录条数
        /// </summary>
        public int size { get; set; }
        /// <summary>
        /// 总记录的条数
        /// </summary>
        public int records { get; set; }
        /// <summary>
        /// 当前返回的数据行
        /// </summary>
        public List<T> rows { get; set; }

        public bool Ret { get; set; }
    }
}
