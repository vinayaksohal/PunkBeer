using System;

namespace PunkBeer.Models
{
    public class ValidateJsonRequestBeerByIDAction:DataClassBeerByIdAction
    {
        // validate Id coming from API request
        public bool ValidatebeerID(string id)
        {
            RetrieveDataFromPunkById getID = new RetrieveDataFromPunkById(id);

            //setting static Id variable of CallPunk class
            return getID.GetBeer();
        }

        
        // Validate userRating between 1 and 5
        public bool ValidateRating(string rating)
        {
            var rated = int.Parse(rating);
            if (rated > 0 && rated < 6)
            {
                Console.WriteLine("Correct raing");
                return true;
            }
            else return false;
        }
    }
}