using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace PunkBeer
{
    public class CustomActionFilter : ActionFilterAttribute
    {
        // variable for storing JSON received from API request
        string requestBodyRetrieved;

        // created private class for data variable email-id
        private class EmailData{
            public string username { get; set; }
        }

        // Method for persorming custom email validation action before executing action in controller 
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            using (var stream = new MemoryStream())
            {
                var context = (HttpContextBase)actionContext.Request.Properties["MS_HttpContext"];
                context.Request.InputStream.Seek(0, SeekOrigin.Begin);
                context.Request.InputStream.CopyTo(stream);
                requestBodyRetrieved = Encoding.UTF8.GetString(stream.ToArray());
            }

            //Deserializing JSON to data menmber of Class EmailData
            var user = JsonConvert.DeserializeObject<EmailData>(requestBodyRetrieved);

            //Verifying email format using regular expression
            if (!Regex.IsMatch(user.username, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z",RegexOptions.IgnoreCase))
            {
                actionContext.Response = actionContext.Request.CreateResponse(
                HttpStatusCode.OK,
                new { foo = "Invalid Email Format" },
                actionContext.ControllerContext.Configuration.Formatters.JsonFormatter
                );
            }

         base.OnActionExecuting(actionContext);
        }    
    }
}