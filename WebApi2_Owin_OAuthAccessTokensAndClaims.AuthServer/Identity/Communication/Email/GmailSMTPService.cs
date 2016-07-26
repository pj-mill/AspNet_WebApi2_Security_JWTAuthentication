using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace WebApi2_Owin_OAuthAccessTokensAndClaims.AuthServer.Identity.Services.Communication.Email
{
    public class GmailSMTPService : SMTPServiceBase, IIdentityMessageService
    {
        #region SEND METHODS

        /// <summary>
        /// Sends an email asynchronously to a recipient.
        /// <para>
        /// You will typically call this for account confirmation.
        /// </para>
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendAsync(IdentityMessage message)
        {
            Subject = message.Subject;
            Body = message.Body;
            SendTo = message.Destination;

            await SendAsync();
        }

        #endregion
    }
}