using System.Net.Http;

namespace PunkBeer.Models
{
    public class RetrieveDataFromPunkById
    {
        public static string id { get; set; }

        public RetrieveDataFromPunkById(string id)
        {
            RetrieveDataFromPunkById.id = id;
        }
        private static bool reposnse { get; set; }

        public bool GetBeer()
        {
            using (var client = new HttpClient())
            {
                string baseUrl = string.Concat("https://api.punkapi.com/v2/beers/" + id);

                //HTTP GET
                var responseTask = client.GetAsync(baseUrl);

                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

    }
}
