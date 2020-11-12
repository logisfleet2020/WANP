using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WANP.Models;

namespace WANP.Controllers
{
    public class HistoricalController : ApiController
    {
        // GET api/<controller>
        /*
        [Route("api/Historical")]
        public IHttpActionResult Get()
        {
            return Json("hi");
        }
        */

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }


        [Route("api/Historical")]
        public IHttpActionResult Get([FromBody] RequestModel model){
            var test = model.CarPlateNo;
            return Json(test);
        }
        

        // POST api/<controller>
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}