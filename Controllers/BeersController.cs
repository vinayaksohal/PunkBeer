
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PunkBeer.Models;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace PunkBeer.Controllers
{
    public class BeersController : ApiController
    {
        [ActionName("BeerById")]
        [CustomActionFilter]
        public  string GetBeerById(string id,[FromBody] JObject d)
        {
            ValidateJsonRequestBeerByIDAction ratingDetails = new ValidateJsonRequestBeerByIDAction();
            //Converting Json object to ValidateJsonRequestBeerByIDAction class type
            ratingDetails = JsonConvert.DeserializeObject<ValidateJsonRequestBeerByIDAction>(d.ToString());
            ratingDetails.id = id;
           
            JObject APIResponseForId = ratingDetails.ValidatebeerID(id);

            bool APIResposeForRating = ratingDetails.ValidateRating(ratingDetails.rating);
            if (APIResponseForId.Count > 0 &&  APIResposeForRating)
            {
                WriteValidJsonRequestToFileResponseByIdAction<ValidateJsonRequestBeerByIDAction> writeToJson = 
                new WriteValidJsonRequestToFileResponseByIdAction<ValidateJsonRequestBeerByIDAction>();
              
                writeToJson.AppendToFile(ratingDetails);
                
                return "Rated Successfully";
            }else if(APIResponseForId.Count == 0 && !APIResposeForRating)
            {
                return "Beer Id does not exist in punk and rating was out of range";
            
            }else if(APIResponseForId.Count == 0)
            {
                return "Beer id does not exists in Punk";
            }else
                return "Rating provided was out of range";
        }


        [ActionName("BeerByName")]
        public dynamic GetBeerByName(string name)
        {
            RetrieveDataFromPunkByName getByNameAPI = new RetrieveDataFromPunkByName(name);
            var task= getByNameAPI.GetBeerAsync();
            task.Wait();

            JObject result=task.Result;
            if (result.Count > 0)
            {
                DataClassBeerByNameAction.RootData root = new DataClassBeerByNameAction.RootData();

                root = JsonConvert.DeserializeObject<DataClassBeerByNameAction.RootData>(result.ToString());

                WriteValidJsonResponseToFileByNameAction<DataClassBeerByNameAction.RootData> w = new WriteValidJsonResponseToFileByNameAction<DataClassBeerByNameAction.RootData>();
                w.AppendToFile(root);

                ResponseProjectionFromFileUsingLinq usingLinq = new ResponseProjectionFromFileUsingLinq();
                return usingLinq.convertUsingLinq();
            }
            else
            {
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest, "Beer does not exists on PUNK");
                return response;
            }
            
        }
    }
}