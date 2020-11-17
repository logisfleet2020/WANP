using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WANP.Models
{
    public class RequestModelA
    {
        public string[] CarPlateNo { get; set; }
        public string FromDate { get; set; }
        public string FromTime { get; set; }
        public string ToDate { get; set; }
        public string ToTime { get; set; }
    }
}