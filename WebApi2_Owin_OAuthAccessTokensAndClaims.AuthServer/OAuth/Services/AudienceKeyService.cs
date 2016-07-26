using Microsoft.Owin.Security.DataHandler.Encoder;
using System;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace WebApi2_Owin_OAuthAccessTokensAndClaims.AuthServer.OAuth.Services
{
    /// <summary>
    /// Responsible for creating audience id's & secure keys (for resource servers)
    /// </summary>
    public class AudienceKeyService
    {
        // N.B. FOR THIS EXERCISE WE ARE JUST TAKING THE KEY FROM APP SETTINGS
        //      THIS MODULE WAS USED TO GENERATE THAT KEY, AND WOULD BE USED IN A CASE WHERE MULTIPLE RESOURCES
        //      SERVERS REQUIRED A UNIQUE KEY.

        // In Memory list of audience keys
        private static ConcurrentDictionary<string, Audience> AudiencesList = new ConcurrentDictionary<string, Audience>();

        public static Audience AddAudience(string name)
        {
            var clientId = Guid.NewGuid().ToString("N");

            var key = new byte[32];
            RNGCryptoServiceProvider.Create().GetBytes(key);
            var base64Secret = TextEncodings.Base64Url.Encode(key);

            Audience newAudience = new Audience { ClientId = clientId, Base64Secret = base64Secret, Name = name };
            AudiencesList.TryAdd(clientId, newAudience);
            return newAudience;
        }

        public static Audience FindAudience(string clientId)
        {
            Audience audience = null;
            if (AudiencesList.TryGetValue(clientId, out audience))
            {
                return audience;
            }
            return null;
        }
    }

    public class Audience
    {
        [Key]
        [MaxLength(32)]
        public string ClientId { get; set; }

        [MaxLength(80)]
        [Required]
        public string Base64Secret { get; set; }

        [MaxLength(100)]
        [Required]
        public string Name { get; set; }
    }
}
