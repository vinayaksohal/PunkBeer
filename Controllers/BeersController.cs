
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PunkBeer.Models;
using System.Collections.Generic;
using System.Web.Http;


namespace PunkBeer.Controllers
{
    public class BeersController : ApiController
    {
        [ActionName("BeerById")]
        [CustomActionFilter]
        public  string GetBeerById(string id,[FromBody] JObject d)
        {
            ValidateJsonRequestBeerByIDAction ratingDetails= new ValidateJsonRequestBeerByIDAction();

            //Converting Json object to ValidateJsonRequestBeerByIDAction class type
            ratingDetails = JsonConvert.DeserializeObject<ValidateJsonRequestBeerByIDAction>(d.ToString());
            ratingDetails.id = id;

            bool APIResponseForId = ratingDetails.ValidatebeerID(id);
            bool APIResposeForRating = ratingDetails.ValidateRating(ratingDetails.rating);
            if (APIResponseForId && APIResposeForRating)
            {
                WriteValidJsonResponseToFileByNameAction<ValidateJsonRequestBeerByIDAction> writeToJson = 
                new WriteValidJsonResponseToFileByNameAction<ValidateJsonRequestBeerByIDAction>();
              
                writeToJson.AppendToFile(ratingDetails);
                
                return "Operation Successful";
            }
            return "Operation was unsuccessful";
        }

        [ActionName("BeerByName")]
        public JObject GetBeerByName(string name)
        {
            RetrieveDataFromPunkByName getByNameAPI = new RetrieveDataFromPunkByName(name);
            var task= getByNameAPI.GetBeerAsync();
            task.Wait();

            JObject result=task.Result;
            
            DataClassBeerByNameAction.RootData root = new DataClassBeerByNameAction.RootData();
            
            root = JsonConvert.DeserializeObject<DataClassBeerByNameAction.RootData>(result.ToString());
            
            WriteValidJsonResponseToFileByNameAction<DataClassBeerByNameAction.RootData> w = new WriteValidJsonResponseToFileByNameAction<DataClassBeerByNameAction.RootData>();
            w.AppendToFile(root);

            ResponseProjectionFromFileUsingLinq usingLinq = new ResponseProjectionFromFileUsingLinq();
            return usingLinq.convertUsingLinq();
        }
    }
}