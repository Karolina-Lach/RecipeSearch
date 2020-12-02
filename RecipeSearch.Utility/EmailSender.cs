using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RecipeSearch.Utility
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailOptions _emailOptions;
        
        public EmailSender(IOptions<EmailOptions> options)
        {
            _emailOptions = options.Value;       
        }
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(_emailOptions.SendGridKey, subject, htmlMessage, email);
        }

        private Task Execute(string sendGridKey, string subject, string message, string email)
        {
            var client = new SendGridClient(sendGridKey);
            var from = new EmailAddress("carolina.lach@gmail.com", "Recipes Search");
            var to = new EmailAddress(email, "End User");
            //var plainTextContent = "and easy to do anywhere, even with C#";
            //var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, message, message);
            return client.SendEmailAsync(msg);

        }
    }
}
