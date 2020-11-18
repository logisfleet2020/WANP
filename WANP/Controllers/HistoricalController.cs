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

        public string formatUTCDate(DateTime dateFromDb)
        {
            var localDateTime = dateFromDb.ToLocalTime();
            var formattedStr = String.Format("{0: yyyy-MM-dd HH:mm:ss}",localDateTime);
            return formattedStr;
        }

        
        public void transformToResponse(List<TrackModel> originalTracks, List<ResponseTrackModel> responseTracks,string requestedCarPlate)
        {
            for (int i = 0; i < originalTracks.Count; i++)
            {
                responseTracks.Add(new ResponseTrackModel { CarPlateNo = requestedCarPlate, TimeStamp = formatUTCDate(originalTracks[i].utc), Latitude = originalTracks[i].position.latitude, Longitude = originalTracks[i].position.longitude });
            }
        }


        [Route("api/Historical")]
        public async Task<IHttpActionResult> GetAsync([FromBody] RequestModel model){
            try
            {

                var tracksResult = new List<TrackModel>();
                var formattedResponse = new List<ResponseTrackModel>();
                var firstResult = new List<TrackModel>();
                var secondResult = new List<TrackModel>();
                var formattedResponseDayOne = new List<ResponseTrackModel>();
                var formattedResponseDayTwo = new List<ResponseTrackModel>();
               

                var requestedCarPlate = model.CarPlateNo;
                var client = new RestClient("https://fms.logisfleet.com/comGpsGate/api/v.1/applications");

                var userIdCarPlateRequest = new RestRequest("259/users");
                userIdCarPlateRequest.AddHeader("Authorization", "cwdGQXZ1Zg7S8ixv0JLfVPh0BDizIqCm0Whv0uOwAHiGUEZRvTremuXMfqCsEj6atW2GrgdkyOJCJJmCxGCAiQ%3d%3d");

                var usersResult = await client.GetAsync<List<UsersResponseModel>>(userIdCarPlateRequest);

                var carPlateMatch = usersResult.Where(user => user.name == requestedCarPlate);

                if (carPlateMatch.Count() == 1)
                {

                    var idToRequest = carPlateMatch.ToList()[0].id;

                    if (model.FromDate == model.ToDate)
                    {
                        if (model.FromTime != null && model.ToDate != null && model.ToTime != null)
                        {
                            var requestWithAllParameters = new RestRequest("/259/users/" + idToRequest + "/tracks?Date=" + model.FromDate + "&From=" + model.FromTime + "&Until=" + model.ToTime);
                            requestWithAllParameters.AddHeader("Authorization", "cwdGQXZ1Zg7S8ixv0JLfVPh0BDizIqCm0Whv0uOwAHiGUEZRvTremuXMfqCsEj6atW2GrgdkyOJCJJmCxGCAiQ%3d%3d");

                            tracksResult = await client.GetAsync<List<TrackModel>>(requestWithAllParameters);

                            transformToResponse(tracksResult, formattedResponse, model.CarPlateNo);

                            return Json(formattedResponse);
                        }
                    }
                    else
                    {
                        var firstRequest = new RestRequest("/259/users/" + idToRequest + "/tracks?Date=" + model.FromDate + "&From=" + model.FromTime + "&Until=23:59:59");
                        firstRequest.AddHeader("Authorization", "cwdGQXZ1Zg7S8ixv0JLfVPh0BDizIqCm0Whv0uOwAHiGUEZRvTremuXMfqCsEj6atW2GrgdkyOJCJJmCxGCAiQ%3d%3d");

                        firstResult = await client.GetAsync<List<TrackModel>>(firstRequest);

                        transformToResponse(firstResult, formattedResponseDayOne, model.CarPlateNo);

                        var secondRequest = new RestRequest("/259/users/" + idToRequest + "/tracks?Date=" + model.ToDate + "&From=00:00:00&Until=" + model.ToTime);
                        secondRequest.AddHeader("Authorization", "cwdGQXZ1Zg7S8ixv0JLfVPh0BDizIqCm0Whv0uOwAHiGUEZRvTremuXMfqCsEj6atW2GrgdkyOJCJJmCxGCAiQ%3d%3d");

                        secondResult = await client.GetAsync<List<TrackModel>>(secondRequest);

                        transformToResponse(secondResult, formattedResponseDayTwo, model.CarPlateNo);

                        formattedResponseDayOne.AddRange(formattedResponseDayTwo);

                        return Json(formattedResponseDayOne);
                    }

                   
                    //end try here. 
                }
                else
                {
                    return Json("Not a valid car plate");
                }
            }catch(Exception e)
            {
                Console.Write(e);
            }

            return Json("nothing");
            //var test = model.CarPlateNo;
            //return Json(test);
        }


        [Route("api/HistoricalAlt")]
        public async Task<IHttpActionResult> GetAsyncAlt([FromBody] RequestModelA model)
        {

            var listOfVehiclesToRequest = new List<MatchVehicleModel>(); 
           var trackModelResult = new List<TrackModel>();
           var formattedResponse = new List<ResponseTrackModel>();
            //var allResponses = new List<ResponseTrackModel>();


            try
            {
                var client = new RestClient("https://fms.logisfleet.com/comGpsGate/api/v.1/applications");
                // if inside the JSON request body, there isn't a property call carplate no at all. 
                if (model.CarPlateNo == null)
                {
                    var AllUsersRequest = new RestRequest("259/users");
                    AllUsersRequest.AddHeader("Authorization", "cwdGQXZ1Zg7S8ixv0JLfVPh0BDizIqCm0Whv0uOwAHiGUEZRvTremuXMfqCsEj6atW2GrgdkyOJCJJmCxGCAiQ%3d%3d");
                    var usersResult = await client.GetAsync<List<UsersResponseModel>>(AllUsersRequest);

                    var usersWithDevices = usersResult.Where(user => user.devices.Count != 0).ToList();

                    if (model.FromDate == model.ToDate)
                    {
                        if (model.FromTime != null && model.ToDate != null && model.ToTime != null)
                        {
                            //loop through all the ID to get the tracks
                            for (int j = 0; j < usersWithDevices.Count; j++ ) {

                                var requestWithAllParameters = new RestRequest("/259/users/" + usersWithDevices[j].id + "/tracks?Date=" + model.FromDate + "&From=" + model.FromTime + "&Until=" + model.ToTime);
                                requestWithAllParameters.AddHeader("Authorization", "cwdGQXZ1Zg7S8ixv0JLfVPh0BDizIqCm0Whv0uOwAHiGUEZRvTremuXMfqCsEj6atW2GrgdkyOJCJJmCxGCAiQ%3d%3d");

                                trackModelResult = await client.GetAsync<List<TrackModel>>(requestWithAllParameters);
                                //assume that the list will be poupulated with the data of every vehicle. 
                                transformToResponse(trackModelResult, formattedResponse, usersWithDevices[j].name);
                              
                            }

                        }
                    }

                    return Json(formattedResponse);
                    //if inside the request body there is a property called carplate no.
                }else if(model.CarPlateNo != null)
                {
                    for (int j = 0; j < model.CarPlateNo.Length; j++)
                    {
                        var AllUsersRequest = new RestRequest("259/users");
                        AllUsersRequest.AddHeader("Authorization", "cwdGQXZ1Zg7S8ixv0JLfVPh0BDizIqCm0Whv0uOwAHiGUEZRvTremuXMfqCsEj6atW2GrgdkyOJCJJmCxGCAiQ%3d%3d");
                        var usersResult = await client.GetAsync<List<UsersResponseModel>>(AllUsersRequest);

                        var carPlatesMatch = usersResult.Where(user => user.name == model.CarPlateNo[j]);
                        if(carPlatesMatch.Count() == 1)
                        {
                            listOfVehiclesToRequest.Add(new MatchVehicleModel { id = carPlatesMatch.ToList()[0].id, name = carPlatesMatch.ToList()[0].name });
                        }

                    }
                    if (model.FromTime != null && model.ToDate != null && model.ToTime != null)
                    {

                        for (int k = 0; k < listOfVehiclesToRequest.Count; k++)
                        {
                            var requestWithAllParameters = new RestRequest("/259/users/" + listOfVehiclesToRequest[k].id + "/tracks?Date=" + model.FromDate + "&From=" + model.FromTime + "&Until=" + model.ToTime);
                            requestWithAllParameters.AddHeader("Authorization", "cwdGQXZ1Zg7S8ixv0JLfVPh0BDizIqCm0Whv0uOwAHiGUEZRvTremuXMfqCsEj6atW2GrgdkyOJCJJmCxGCAiQ%3d%3d");
                            trackModelResult = await client.GetAsync<List<TrackModel>>(requestWithAllParameters);
                            transformToResponse(trackModelResult, formattedResponse, listOfVehiclesToRequest[k].name);
                        }
                    }
                    return Json(formattedResponse);
                }
                
            }
            catch(Exception e)
            {

            }
            return Json("nothing");
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