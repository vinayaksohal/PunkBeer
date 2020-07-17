using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace PunkBeer.Models
{
    public class WriteValidJsonResponseToFileByNameAction<DataClassBeerByNameAction>
    {
        public void AppendToFile(DataClassBeerByNameAction data)
        {
            var filePath = @"C:\Users\user\source\repos\PunkBeer\PunkBeer\Database2.json";

            // Read existing json data
            string jsonData = File.ReadAllText(filePath);

            // De-serialize to object or create new list
            var list = JsonConvert.DeserializeObject<List<DataClassBeerByNameAction>>(jsonData) ?? new List<DataClassBeerByNameAction>();

            // Add to List

            list.Add(data);

            // Update json data string2
            jsonData = JsonConvert.SerializeObject(list);
            System.IO.File.WriteAllText(filePath, jsonData);
        }
    }
}