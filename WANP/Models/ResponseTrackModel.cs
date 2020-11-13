using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WANP.Models
{
    public class ResponseTrackModel
    {
        public string CarPlateNo { get; set; }
        public string TimeStamp { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

    }
}