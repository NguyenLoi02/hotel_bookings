using System;
using System.IO;
using System.Threading;
using System.Web.Services.Description;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;
using Google.Apis.Services;
using Google.Apis.Gmail.v1;
using System.Text;
using System.Threading.Tasks;




namespace hotel_bookings.Common
{
    public class Common
    {
        private readonly string _clientId;
        private readonly string _clientSecret;

        public Common(string clientId, string clientSecret)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
        }

        public async Task SendEmailAsync(string senderEmail, string senderName, string recipientEmail, string recipientName, string subject, string body)
        {
            try
            {
                string[] scopes = { Google.Apis.Gmail.v1.GmailService.Scope.GmailSend };

                // Authenticate the user
                UserCredential credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    new ClientSecrets { ClientId = _clientId, ClientSecret = _clientSecret },
                    scopes,
                    "user",
                    CancellationToken.None
                );

                // Create Gmail API service
                var service = new Google.Apis.Gmail.v1.GmailService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "YourAppName"
                });

                // Create email message
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(senderName, senderEmail));
                message.To.Add(new MailboxAddress(recipientName, recipientEmail));
                message.Subject = subject;
                message.Body = new TextPart("plain")
                {
                    Text = body
                };

                // Send email
                var msgStr = new StringWriter();
                message.WriteTo(msgStr);
                var rawMsg = Convert.ToBase64String(Encoding.ASCII.GetBytes(msgStr.ToString()));
                var gmailMessage = new Message { Raw = rawMsg };
                await service.Users.Messages.Send(gmailMessage, "me").ExecuteAsync();
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately (log, throw, etc.)
                throw new Exception("Failed to send email.", ex);
            }
        }

    }
}

//private static string password = ConfigurationManager.AppSettings["PasswordEmail"];
//public static string Email = ConfigurationManager.AppSettings["Email"];
//public static bool SendMail(string subject, string name, string content, string toMail)
//{
//    bool rs = false;
//    try
//    {
//        MailMessage message = new MailMessage();
//        var smtp = new System.Net.Mail.SmtpClient();
//        {
//            smtp.Host = "smtp.gmail.com";
//            smtp.Port = 587;
//            smtp.EnableSsl = true;
//            smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
//            smtp.UseDefaultCredentials = false;
//            smtp.Credentials = new NetworkCredential()
//            {
//                UserName = Email,
//                Password = password,
//            };
//        }
//        MailAddress fromAddress = new MailAddress(Email, name);
//        message.From = fromAddress;
//        message.To.Add(toMail);
//        message.Subject = subject;
//        message.IsBodyHtml = true;
//        message.Body = content;
//        smtp.Send(message);
//        rs = true;
//    }
//    catch (Exception ex)
//    {
//        rs = false;
//    }
//    return rs;
//}