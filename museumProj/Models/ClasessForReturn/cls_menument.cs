using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace museumProj.Models.ClasessForReturn
{
    public class cls_menument
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string voice_note { get; set; }
        public string QR_image { get; set; }
        public Nullable<int> no_of_scans { get; set; }
        public Nullable<int> place_id { get; set; }
        public string place_name { get; set; }

        public virtual ICollection<cls_menument_images> menument_images { get; set; }
    }
}