using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using MimeKit;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
namespace hotel_bookings.Common
{
    public class GoogleAuthentication
    {
        public static string CLIENT_ID = "33807516485-lbljlqhr2qu12li17a5trpn50pieho9c.apps.googleusercontent.com";
        public static string CLIENT_SECRET = "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";
        public static string REDIRECT_URI = "https://developers.google.com/oauthplayground";
        public static string REFRESH_TOKEN = "1//04Uf_zlsoHDUqCgYIARAAGAQSNwF-L9Ir2JIFZYuvoYKXFxbzAM3ClThJZkMFSYQzzFK34d887pIun3oncCnrfDtClroSKG_zehU";

        // Khai báo thông tin OAuth2 client
        public static GoogleAuthorizationCodeFlow.Initializer initializer = new GoogleAuthorizationCodeFlow.Initializer
        {
            ClientSecrets = new ClientSecrets
            {
                ClientId = CLIENT_ID,
                ClientSecret = CLIENT_SECRET
            },
            Scopes = new[] { GmailService.Scope.GmailSend },
            DataStore = new FileDataStore("TokenStore")
        };

        public static GoogleAuthorizationCodeFlow oAuth2client = new GoogleAuthorizationCodeFlow(initializer);

        public static async Task<string> RefreshAccessToken()
        {
            GoogleAuthorizationCodeFlow flow = new GoogleAuthorizationCodeFlow(initializer);

            // Tạo một đối tượng TokenResponse từ REFRESH_TOKEN
            TokenResponse tokenResponse = new TokenResponse
            {
                RefreshToken = REFRESH_TOKEN
            };

            // Tạo một UserCredential mới từ TokenResponse
            UserCredential credential = new UserCredential(flow, "user", tokenResponse);
            return credential.Token.AccessToken;
            // Làm mới token
        }

        public static void SendEmail(string subject, string body, string to)
        {
            string accessToken = RefreshAccessToken().Result;

            var service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = new UserCredential(oAuth2client, "user", new TokenResponse
                {
                    AccessToken = accessToken,
                    RefreshToken = REFRESH_TOKEN
                }),
                ApplicationName = "hotel_booking"
            });

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("loi", "thanbaj9k@gmail.com"));
            message.To.Add(new MailboxAddress("", to));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = body;

            message.Body = bodyBuilder.ToMessageBody();

            var rawMessage = CreateRawMessage(message);
            service.Users.Messages.Send(rawMessage, "me").Execute();
        }

        private static Message CreateRawMessage(MimeMessage mimeMessage)
        {
            using (var memory = new MemoryStream())
            {
                mimeMessage.WriteTo(memory);
                var bytes = memory.ToArray();
                var base64 = Convert.ToBase64String(bytes);
                return new Message { Raw = base64 };
            }
        }
    }

}