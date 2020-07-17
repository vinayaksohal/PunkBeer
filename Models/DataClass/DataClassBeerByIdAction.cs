/* This class defines variable which gets populated with the URI parameter and 
 * JSON request body parameters when 'BeerById' action is performed in BeersController Class" */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PunkBeer.Models
{
    public class DataClassBeerByIdAction
    {
        public string id { get; set; }
        public string username{get;set;}
        public string rating{get;set;}
        public string comments{get;set;}
    }
}