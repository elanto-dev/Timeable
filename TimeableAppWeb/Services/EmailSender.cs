using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace TimeableAppWeb.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Sends the email to the specified address.
        /// </summary>
        /// <param name="email">Email where to send the message</param>
        /// <param name="subject">Message subject</param>
        /// <param name="message">Message body</param>
        /// <returns></returns>
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailSettings = _configuration.GetSection("EmailSettings");

            if (!string.IsNullOrWhiteSpace(emailSettings["SenderEmail"]) &&
                !string.IsNullOrWhiteSpace(emailSettings["Password"]) && 
                !string.IsNullOrWhiteSpace(emailSettings["Host"]) && 
                int.TryParse(emailSettings["Port"], out var portNumber) && bool.TryParse(emailSettings["EnableSsl"], out var enableSsl))
            {
                try
                {
                    var smtpClient = new SmtpClient
                    {
                        Port = portNumber,
                        EnableSsl = enableSsl,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        Credentials = new NetworkCredential(emailSettings["SenderEmail"], emailSettings["Password"]),
                        Host = emailSettings["Host"]
                    };

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(emailSettings["SenderEmail"], string.IsNullOrWhiteSpace(emailSettings["SenderName"]) ? emailSettings["SenderName"] : "Timeable"),
                        IsBodyHtml = true,
                        Subject = subject,
                        Body = message
                    };
                    mailMessage.To.Add(new MailAddress(email));
                    await smtpClient.SendMailAsync(mailMessage);
                }
                catch
                {
                    // ignored
                }
            }
        }
    }
}
