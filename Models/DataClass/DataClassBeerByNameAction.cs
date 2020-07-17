/* This class defines variable which gets populated with API response from Punk API 
 * when Punk API is requested with beer name parameter via BeerByName action */



using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PunkBeer.Models
{
    public class DataClassBeerByNameAction
    {
        public class RootData {
            public string id { get; set; }
            public string name { get; set; }
            public string description { get; set; }
            public string tagline { get; set;}
            public BoilVolume volume { get; set; }
            public Normalvolume boil_volume { get; set; }
            public Ingredients ingredients { get; set; }
 
        }

        public class Ingredients
        {
            public List<Malt> malt { get; set; }
            

        }

        public class Malt
        {
            public string name { get; set; }
            public Amount amount { get; set; }
        }

        public class Amount
        {
            public string value { get; set; }
            public string unit { get; set; }
        }

      
        public class Normalvolume
        {
            public string value { get; set; }
            public string unit { get; set; }

        }

        public class BoilVolume
        {
            public string value { get; set; }
            public string unit { get; set; }

        }
   
    }
}