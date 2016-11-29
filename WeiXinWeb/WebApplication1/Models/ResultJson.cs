using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class ResultJson
    {
        public ResultJson()
        {
            this.Ret = false;
        }

        public bool Ret { get; set; }

        public int VoteStatus { get; set; }
        public string Msg { get; set; }

        
    }

    public class ResultJson<T> where T : class
    {
        public ResultJson()
        {
            this.Ret = false;
        }

        public bool Ret { get; set; }

        public string Msg { get; set; }

        public T Data { get; set; }
    }
}