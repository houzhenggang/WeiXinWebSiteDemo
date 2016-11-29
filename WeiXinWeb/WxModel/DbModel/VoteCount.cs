using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxModel
{
    public class VoteCount
    {
        public int Id { get; set; }

        public int DeptId { get; set; }
        public string OpenId { get; set; }

        public DateTime InsertDate { get; set; }
    }
}
