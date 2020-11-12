using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WANP.Models
{
    public class Variables
    {
        public bool harshAccelerationDigital { get; set; }
        public bool hashBrakingDigital { get; set; }
        public bool harshCorneringDigital { get; set; }
        public float satelliteCount { get; set; }
        public float eventID { get; set; }
        public bool ignition { get; set; }
        public bool digitalOutput1 { get; set; }
        public float gsmSignalStrength { get; set; }
        public float voltage { get; set; }
        public float cellID { get; set; }
        public float dallasTemperature1 { get; set; }
        public float temperatureSensor { get; set; }
        public float dallasTemperature2 { get; set; }
        public float dallasTemperature3 { get; set; }
        public float batteryVoltage { get; set; }
        public bool iButton_Connected { get; set; }
        public float speed { get; set; }
    }
}