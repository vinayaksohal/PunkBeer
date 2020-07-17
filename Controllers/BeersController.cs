/*Contoller class handles API requests 
 * (A).Request type 1 via method PostBeerById: this action method accepts two parameters 'id' and 
 *     Json object from request body of HTTP request and does the further processing by using
 *     classes 'ValidateJsonRequestBeerByIDAction.cs' and 'WriteValidJsonRequestToFileResponseByIdAction.cs'
 *     Before performing this action method email id parameter validation is done in action filter 'CustomActionFilter'
 *   
 *      (https://localhost:xxxxx/api/Beers/BeerByID/id)
 *      e.g: https://localhost:xxxxx/api/Beers/BeerByID/1
 *      
 *      requestbody:{
                        "username":"vinayak.sohal007@gmail.com",
                        "rating":"5",
                        "comments":"Good"
                    }
 *      
 * (B) Request type 2 via method GetBeerByName: this method accepts one parameter 'name' and does the 
 *     further processing by using classes 'RetrieveDataFromPunkByName' ,'DataClassBeerByNameAction',
 *     'WriteValidJsonResponseToFileByNameAction''ResponseProjectionFromFileUsingLinq'
 * 
 *     (https://localhost:xxxxx/api/Beers/BeerByName?name=[”BeerName"])
 *     e.g: https://localhost:xxxxx/api/Beers/BeerByName?name=Buzz
 */

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PunkBeer.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace PunkBeer.Controllers
{
    public class BeersController : ApiController
    {
        List<DataClassBeerByIdAction> list = new List<DataClassBeerByIdAction>();
        public BeersController() { }

        //Post method for BeerById action request

        [ActionName("BeerById")]
        [CustomActionFilter]
        public  dynamic PostBeerById(string id,[FromBody] JObject d)
        {
            ValidateJsonRequestBeerByIDAction ratingDetails = new ValidateJsonRequestBeerByIDAction();

            //Converting Json object to ValidateJsonRequestBeerByIDAction class type
            ratingDetails = JsonConvert.DeserializeObject<ValidateJsonRequestBeerByIDAction>(d.ToString());
            ratingDetails.id = id;

            //Validation methods of class 'ValidateJsonRequestBeerByIDAction' called
            JObject APIResponseForId = ratingDetails.ValidatebeerID(id);   
            bool APIResposeForRating = ratingDetails.ValidateRating(ratingDetails.rating);

            //Creating response based on the result validation method
            if (APIResponseForId.Count > 0 &&  APIResposeForRating)
            {
                //writing valid responses to file
                WriteValidJsonRequestToFileResponseByIdAction<ValidateJsonRequestBeerByIDAction> writeToJson = 
                new WriteValidJsonRequestToFileResponseByIdAction<ValidateJsonRequestBeerByIDAction>();
              
                writeToJson.AppendToFile(ratingDetails);
                
                return APIResponseForId;

            }else if(APIResponseForId.Count == 0 && !APIResposeForRating)
            {
                return "Beer Id does not exist in punk and rating was out of range";
            
            }else if(APIResponseForId.Count == 0)
            {
                return "Beer id does not exists in Punk";
            }else
                return "Rating provided was out of range";
        }

        // Get method for BeerByName action request

        [ActionName("BeerByName")]
        public dynamic GetBeerByName(string name)
        {
            //using class 'RetrieveDataFromPunkByName' to get response from Punk API
            RetrieveDataFromPunkByName getByNameAPI = new RetrieveDataFromPunkByName(name);
            var task= getByNameAPI.GetBeerAsync();
            task.Wait();

            JObject result=task.Result;
            if (result.Count > 0)
            {   
                //Creating response in case of success
                DataClassBeerByNameAction.RootData root = new DataClassBeerByNameAction.RootData();

                root = JsonConvert.DeserializeObject<DataClassBeerByNameAction.RootData>(result.ToString());

                WriteValidJsonResponseToFileByNameAction<DataClassBeerByNameAction.RootData> w = new WriteValidJsonResponseToFileByNameAction<DataClassBeerByNameAction.RootData>();
                w.AppendToFile(root);

                ResponseProjectionFromFileUsingLinq usingLinq = new ResponseProjectionFromFileUsingLinq();
                return usingLinq.convertUsingLinq();
            }
            else
            {
                //creating response if unsuccessful
                return new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent("Invalid Beer name", System.Text.Encoding.UTF8, "application/json") };
                
            }
            
        }
    }
}

