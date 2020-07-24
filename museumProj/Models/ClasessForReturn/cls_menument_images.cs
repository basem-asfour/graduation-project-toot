using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace museumProj.Models.ClasessForReturn
{
    public class cls_menument_images
    {
        public int id { get; set; }
        public Nullable<int> menument_id { get; set; }
        public string image { get; set; }
        public string altr { get; set; }
    }
}