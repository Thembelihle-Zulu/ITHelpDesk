using Microsoft.AspNetCore.Identity;
using SharedLibrary.Model;
using System.Net.Mail;
using System.Net;
using SharedLibrary.Model;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
namespace SharedLibrary.Services
{

    public class FakeEmailSender : IEmailSender<ApplicationUser>, ICustomEmailSender
    {
        private readonly IConfiguration _config;
        public FakeEmailSender(IConfiguration configuration)
        {
            _config = configuration;
        }

        public Task SendConfirmationLinkAsync(IdentityUser user, string email, string confirmationLink)
        {
            throw new NotImplementedException();
        }

        public Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink)
        {
            throw new NotImplementedException();
        }

        public async Task SendEmailAsync(string receptor, string subject, string body)
        {
            var email = _config.GetValue<string>("EMAIL_CONFIGURATION:EMAIL");
            var password = _config.GetValue<string>("EMAIL_CONFIGURATION:PASSWORD");
            var host = _config.GetValue<string>("EMAIL_CONFIGURATION:HOST");
            var port = _config.GetValue<int>("EMAIL_CONFIGURATION:PORT");

            var smtpClient = new SmtpClient(host, port);
            smtpClient.EnableSsl= true;
            smtpClient.UseDefaultCredentials = false;

            smtpClient.Credentials = new NetworkCredential(email, password);

            var message = new MailMessage(email!, receptor, subject, body);
            message.IsBodyHtml= true;
            await smtpClient.SendMailAsync(message);
        }

        public Task SendPasswordResetCodeAsync(IdentityUser user, string email, string resetCode)
        {
            throw new NotImplementedException();
        }

        public Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode)
        {
            throw new NotImplementedException();
        }

        public Task SendPasswordResetLinkAsync(IdentityUser user, string email, string resetLink)
        {
            throw new NotImplementedException();
        }

        public Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink)
        {
            throw new NotImplementedException();
        }
    }

}
