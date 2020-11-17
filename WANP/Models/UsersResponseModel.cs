using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WANP.Models
{
    public class UsersResponseModel
    {
        public UsersResponseModel()
        {

        }
        public string name { get; set; }
        public int id { get; set; }
        public List<DevicesModel> devices { get; set; } 
    }
}