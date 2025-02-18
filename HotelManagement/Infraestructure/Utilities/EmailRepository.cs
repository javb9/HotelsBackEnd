using System.Net.Mail;
using System.Net;
using HotelManagement.Domain.Interfaces;
using static HotelManagement.Application.Dtos.EmailDto;
using Microsoft.Extensions.Options;

namespace HotelManagement.Infraestructure.Utilities
{
    public class EmailRepository : IEmail
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailRepository(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }

        public void SendEmail(string to, string subject, string body)
        {
            var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(_smtpSettings.User);
            mailMessage.To.Add(to);
            mailMessage.Subject = subject;
            mailMessage.Body = body;

            using (var smtpClient = new SmtpClient(_smtpSettings.Server, _smtpSettings.Port))
            {
                smtpClient.Credentials = new NetworkCredential(_smtpSettings.User, _smtpSettings.Pass);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Send(mailMessage);
            }
        }
    }
}
