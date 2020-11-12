using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WANP.Models
{
    public class TrackModel
    {
        public Position position { get; set; }
        public int trackInfoId { get; set; }
        public DateTime utc { get; set; }
        public bool valid { get; set; }
        public Velocity velocity { get; set; }
        public Variables variables { get; set; }
        public DateTime serverUtc { get; set; }
    }
}