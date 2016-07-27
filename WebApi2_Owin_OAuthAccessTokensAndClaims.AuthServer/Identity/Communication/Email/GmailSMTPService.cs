using Microsoft.AspNet.Identity;
using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace WebApi2_Owin_OAuthAccessTokensAndClaims.AuthServer.Identity.Services.Communication.Email
{
    public class GmailSMTPService : IIdentityMessageService
    {
        #region MAIL SETTINGS PROPERTIES
        /// <summary>
        /// Specifies the smtp service name
        /// </summary>
        public string HostServer { get; set; } = "smtp.gmail.com";

        /// <summary>
        /// Specifies the service port number
        /// </summary>
        public int Port { get; set; } = 587;

        /// <summary>
        /// Specifies whether or not to connect using a secure connection.
        /// </summary>
        public bool EnableSsl { get; set; } = true;

        private string secret;
        /// <summary>
        /// Specifies the password for the gmail account
        /// </summary>
        public string Secret
        {
            get
            {
                if (String.IsNullOrEmpty(secret))
                {
                    secret = ConfigurationManager.AppSettings.Get(GmailAccountSecretAppSettingKey).ToString();
                }
                return secret;
            }
            set
            {
                secret = value;
            }
        }

        /// <summary>
        /// Gets or sets a Boolean value that controls whether the DefaultCredentials are sent with requests
        /// </summary>
        public bool UseDefaultCredentials { get; set; } = false;

        /// <summary>
        /// Specifies the application setting key within the App.Config file for the name of the Gmail account (<see cref="SentFrom"/>).
        /// </summary>
        public string GmailAccountNameAppSettingKey { get; set; } = "GmailSmtpAccount";

        /// <summary>
        /// Specifies the application setting keyy within the App.Config file for the secret of the Gmail account (<see cref="Secret"/>).
        /// </summary>
        public string GmailAccountSecretAppSettingKey { get; set; } = "GmailSmtpSecret";

        /// <summary>
        /// Specifies how email messages are delivered
        /// </summary>
        public SmtpDeliveryMethod DeliveryMethod { get; set; } = System.Net.Mail.SmtpDeliveryMethod.Network;


        private NetworkCredential credentials;
        /// <summary>
        /// Network credentials required for authenticating with the external service
        /// </summary>
        public NetworkCredential Credentials
        {
            get
            {
                if (credentials == null)
                {
                    credentials = new NetworkCredential(SentFrom, Secret);
                }
                return credentials;
            }
            set { credentials = value; }
        }

        #endregion

        #region MAIL CONTENT PROPERTIES
        private string sentForm;
        /// <summary>
        /// Specifies the sender's email address
        /// </summary>
        public string SentFrom
        {
            get
            {
                if (String.IsNullOrEmpty(sentForm))
                {
                    sentForm = ConfigurationManager.AppSettings.Get(GmailAccountNameAppSettingKey).ToString();
                }
                return sentForm;
            }
            set
            {
                sentForm = value;
            }
        }

        /// <summary>
        /// Specifies the recipient(s) email address
        /// </summary>
        public string SendTo { get; set; }

        /// <summary>
        /// Specifies the subject of the email
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Friendly name for sender
        /// </summary>
        public string CredentialName { get; set; } = "Admin";

        /// <summary>
        /// Specifies the priority of a MailMessage
        /// </summary>
        public MailPriority MailPriority { get; set; } = System.Net.Mail.MailPriority.Normal;

        /// <summary>
        /// The message body
        /// </summary>
        public string Body { get; set; }
        #endregion

        #region METHODS

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

        /// <summary>
        /// Performs the actual sending
        /// </summary>
        /// <returns></returns>
        private async Task SendAsync()
        {
            // Configure The Client and send
            using (SmtpClient client = new SmtpClient(HostServer))
            {
                client.Port = Port;
                client.DeliveryMethod = DeliveryMethod;
                client.UseDefaultCredentials = false;
                client.EnableSsl = EnableSsl;
                client.Credentials = Credentials;
                await client.SendMailAsync(CreateMessage());
            };
        }

        /// <summary>
        /// Creates the message to be sent over SMTP
        /// </summary>
        /// <returns></returns>
        private MailMessage CreateMessage()
        {
            return new MailMessage(SentFrom, SendTo)
            {
                Priority = MailPriority,
                Subject = Subject,
                Body = Body
            };
        }

        #endregion
    }
}





