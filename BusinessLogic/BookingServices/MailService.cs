using BusinessLogic.Interfaces;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;
using Org.BouncyCastle.Asn1.Pkcs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using BusinessLogic.Helpers;

namespace BusinessLogic.BookingServices
{
    public class MailService : IMailService
    {
        //Інтерфейс підключення
        public readonly IConfiguration _configuration;

        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendMailAsync(string toEmail, string subject, string body)
        {
            string FormEmail = _configuration["EmailSettings:User"];
            string password= _configuration["EmailSettings:Password"];
            string smtp= _configuration["EmailSettings:SMTP"];
            int port = Int32.Parse(_configuration["EmailSettings:PORT"]);

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(FormEmail));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;

            var bodybuilder = new BodyBuilder();
            bodybuilder.HtmlBody = body;
            email.Body= bodybuilder.ToMessageBody();

            using (var smtpCl = new MailKit.Net.Smtp.SmtpClient())
            {
                smtpCl.Connect(smtp, port, MailKit.Security.SecureSocketOptions.SslOnConnect);
                smtpCl.Authenticate(FormEmail, password);
                await smtpCl.SendAsync(email);
                smtpCl.Disconnect(true);
            }       

        }
    }
}
