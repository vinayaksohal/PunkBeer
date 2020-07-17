using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PunkBeer.Models
{
    public class RetrieveDataFromPunkByName
    {
        public string name { get; set; }

        public RetrieveDataFromPunkByName(string name)
        {
            this.name = name;
        }

        public  async Task<JObject> GetBeerAsync()
        {
            JObject responseByName  = new JObject();
            // HTTP GET.  
            using (var client = new HttpClient())
            {
                string baseURL = "https://api.punkapi.com/v2/";
                string _baseURL = "beers?beer_name=" + name;
                // Setting Base address.  
                client.BaseAddress = new Uri(baseURL);

                // Setting content type.  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Initialization.  
                HttpResponseMessage response = new HttpResponseMessage();

                // HTTP GET  
                response = await client.GetAsync(_baseURL).ConfigureAwait(false);

                // Verification  
                if (response.IsSuccessStatusCode)
                {
                    // Reading Response.  
                    string resp = response.Content.ReadAsStringAsync().Result;
                    //resp=Regex.Unescape(resp);
                    JArray jsonArray = JArray.Parse(resp);
                    //var _resp = JObject.Parse(resp);
                    dynamic data = JObject.Parse(jsonArray[0].ToString());

                    responseByName = data;
                }
            }

            return responseByName;
        }
    }
}