using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;


namespace PunkBeer.Models
{
    public class WriteValidJsonRequestToFileResponseByIdAction<ValidateJsonRequestBeerByIDAction>
    {
        public void AppendToFile(ValidateJsonRequestBeerByIDAction jsonRatingData )
        {
            var filePath = @"C:\Users\user\source\repos\PunkBeer\PunkBeer\Database1.json";

            // Read existing json data
            string  jsonData = File.ReadAllText(filePath);
    
            // De-serialize to object or create new list
            var list = JsonConvert.DeserializeObject<List<ValidateJsonRequestBeerByIDAction>>(jsonData) ?? new List<ValidateJsonRequestBeerByIDAction>();

            // Add to List

             list.Add(jsonRatingData);
            
            // Update json data string
            jsonData = JsonConvert.SerializeObject(list);
            System.IO.File.WriteAllText(filePath,jsonData);
        }
    }
}