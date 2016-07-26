using Newtonsoft.Json.Serialization;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace WebApi2_Owin_OAuthAccessTokensAndClaims.Core.Config
{
    /// <summary>
    /// Configures JSON message format
    /// </summary>
    public class JSONMediaTypeConfiguration
    {
        public static void Register(HttpConfiguration config)
        {
            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}