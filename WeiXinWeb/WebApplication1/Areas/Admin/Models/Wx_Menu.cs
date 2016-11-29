using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebApplication1.Areas.Admin.Models
{
    public class Wx_Menu
    {
        public menu menu = new menu();

    }
    public class menu {
        public button[] button;
    
    }

    public class button
    {
        public string name { get; set; }
        public string type { get; set; }
        public string key { get; set; }
        public string url { get; set; }
        public string media_id { get; set; }
        public button[] sub_button { get; set; }
    }
}
