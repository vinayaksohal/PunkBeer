/*This Class reads data of Beers from Database2.Json when there is valid response
 * from RetrieveDataFromPunkByName and converts whole file into format using Linq and send
 * it as response of BeerByName action request*/


using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace PunkBeer.Models
{
    public class ResponseProjectionFromFileUsingLinq
    {
        string filePath = @"C:\Users\user\source\repos\PunkBeer\PunkBeer\Database2.json";
        public JObject convertUsingLinq()
        {
            string jsonData = File.ReadAllText(filePath);

            var root = JsonConvert.DeserializeObject<List<DataClassBeerByNameAction.RootData>>(jsonData);

            JObject response =
                new JObject(
                    new JProperty("Beer Detail",
                          from p in root
                          orderby p.id
                          select new JObject(
                              new JProperty("Beer id", p.id),
                              new JProperty("Beer Name", p.name),
                              new JProperty("Beer_Tagline", p.tagline),
                              new JProperty("Beer_Description", p.description))));
                              //new JProperty("Ingredients",
                              //   new JObject("Malt",
                              //              new JArray(from c in p.ingredients.malt
                              //                         select new JObject(
                              //                             new JProperty("Name", c.name),
                              //                             new JProperty("Amount",
                              //                             new JObject(
                              //                                 new JProperty("unit", c.amount.unit))))))))));




            return response;
        }

    }
}