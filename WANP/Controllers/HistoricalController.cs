using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WANP.Models;
using RestSharp;
using RestSharp.Authenticators;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Web.Configuration;

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
        public async Task<IHttpActionResult> GetAsync([FromBody] RequestModel model){

            try
            {

                var tracksResult = new List<TrackModel>();
                var firstResult = new List<TrackModel>();
                var secondResult = new List<TrackModel>();

                var requestedCarPlate = model.CarPlateNo;
                var client = new RestClient("https://fms.logisfleet.com/comGpsGate/api/v.1/applications");

                var userIdCarPlateRequest = new RestRequest("252/users");
                userIdCarPlateRequest.AddHeader("Authorization", "YhH6C5FlWp0EYlcKXCcQdzCBmaEUxoXf8AFLfEI%2fh2zu7DE%2biS%2f42L4j25Gc43H%2b");

                var usersResult = await client.GetAsync<List<UsersResponseModel>>(userIdCarPlateRequest);

                var carPlateMatch = usersResult.Where(user => user.name == requestedCarPlate);

                var idToRequest = carPlateMatch.ToList()[0].id;

                if (model.FromDate == model.ToDate) {
                    if (model.FromTime != null && model.ToDate != null && model.ToTime != null)
                    {
                        var requestWithAllParameters = new RestRequest("/252/users/" + idToRequest + "/tracks?Date="+model.FromDate+"&From=" + model.FromTime + "&Until=" + model.ToTime);
                        requestWithAllParameters.AddHeader("Authorization", "YhH6C5FlWp0EYlcKXCcQdzCBmaEUxoXf8AFLfEI%2fh2zu7DE%2biS%2f42L4j25Gc43H%2b");

                         tracksResult = await client.GetAsync<List<TrackModel>>(requestWithAllParameters);

                    }
                }
                else
                {
                    var firstRequest = new RestRequest("/252/users/" + idToRequest + "/tracks?Date=" + model.FromDate + "&From=" + model.FromTime + "&Until=23:59:59");
                    firstRequest.AddHeader("Authorization", "YhH6C5FlWp0EYlcKXCcQdzCBmaEUxoXf8AFLfEI%2fh2zu7DE%2biS%2f42L4j25Gc43H%2b");

                     firstResult = await client.GetAsync<List<TrackModel>>(firstRequest);



                    var secondRequest = new RestRequest("/252/users/" + idToRequest + "/tracks?Date=" + model.ToDate + "&From=00:00:00&Until=" + model.ToTime);
                    secondRequest.AddHeader("Authorization", "YhH6C5FlWp0EYlcKXCcQdzCBmaEUxoXf8AFLfEI%2fh2zu7DE%2biS%2f42L4j25Gc43H%2b");

                     secondResult = await client.GetAsync<List<TrackModel>>(secondRequest);

                }

                /*
                var request = new RestRequest("/252/users/"+idToRequest+"/tracks?Date=2020-10-20");
                request.AddHeader("Authorization", "YhH6C5FlWp0EYlcKXCcQdzCBmaEUxoXf8AFLfEI%2fh2zu7DE%2biS%2f42L4j25Gc43H%2b");

                var result = await client.GetAsync<List<TrackModel>>(request);
                */
            }catch(Exception e)
            {
                Console.Write(e);
            }

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