using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using crds_angular.Models.Sendgrid;
using RestSharp;


namespace crds_angular.Controllers.Webhooks
{
    public class SendgridWebhookController : ApiController
    {
        [HttpPost]
        [Route("sendgrid/events")]
        public IHttpActionResult AcceptEvent(List<SendgridEvent> events )
        {
            string emails = events.Select(e => e.Email).Aggregate((e1, e2) => e1 + ", " + e2);
            string messageText = "Here is the list of users who just recieved an email: " + emails + ". Please review and add the appropriate email addresses to the Global Unsubscribe list if neccesary. ";

            //var message = new SendGridMessage();
            //message.From = new MailAddress("updates@crossroads.net");
            //var to = ConfigurationManager.AppSettings["PeopleToNotify"];

            //var recipients = to.Split(';').ToList();

            //message.AddTo(recipients);
            //message.Subject = "HEADS UP! Someones getting emails!";

            //message.Text = messageText;
            //// notify someone that this list of email addresses has been sent emails

            //// Create a Web transport, using API Key
            //var transportWeb = new Web(ConfigurationManager.AppSettings["SendgridApiKey"]);

            //// Send the email.
            //transportWeb.DeliverAsync(message);

            RestClient client = new RestClient("https://hooks.slack.com");
            RestRequest request = new RestRequest("/services/T02C3F91X/B13TG71SP/RTloIjjUXgmW7MzgYHykhox4", Method.POST);
            request.AddParameter("payload", "{ \"channel\": \"#production-support\", \"username\": \"SendgridBot\", \"text\": \"" + message.Text + "\", \"icon_emoji\": \":mailbox_with_mail:\" }" );
            var response = client.Execute(request);

            return Ok();
        }
    }
}