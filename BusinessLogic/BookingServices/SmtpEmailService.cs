using BusinessLogic.Helpers;
using BusinessLogic.Interfaces;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.BookingServices
{
    public class SmtpEmailService: ISmtpEmailService
    {
        private readonly EmailConfiguration _configuration;
        public SmtpEmailService()
        {
            _configuration = new EmailConfiguration();
        }

        public void Send(Message message)
        {
            var body = new TextPart("html")
            {
                Text = message.Body
            };
            string path = @"~/images/register.png";

            var attachment = new MimePart("image", "jpeg")
            {
                FileName = "Регістрація",
                Content = new MimeContent(File.OpenRead(path))
            };

            var multipart = new Multipart("mixed");
            multipart.Add(body);
            multipart.Add(attachment);

            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_configuration.From));
            emailMessage.To.Add(new MailboxAddress(message.To));
            emailMessage.Subject = message.Subject;
            emailMessage.Body = multipart;

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    client.Connect(_configuration.SmtpServer, _configuration.Port, true);
                    client.Authenticate(_configuration.UserName, _configuration.Password);
                    client.Send(emailMessage);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }
        public void DownloadMessages()
        {
            using (var client = new ImapClient())
            {
                client.Connect("imap.ukr.net", 993, MailKit.Security.SecureSocketOptions.SslOnConnect);
                client.Authenticate(_configuration.UserName, _configuration.Password);
                client.Inbox.Open(MailKit.FolderAccess.ReadOnly);
                var uids = client.Inbox.Search(SearchQuery.All);
                foreach (var uid in uids)
                {
                    var message = client.Inbox.GetMessage(uid);
                    Console.WriteLine("------------------");
                    Console.WriteLine("From: {0}", message.From);
                    Console.WriteLine("Subject: {0}", message.Subject);
                    message.WriteTo(string.Format("{0}.eml", uid));
                }
                client.Disconnect(true);
            }
        }

        public void SuccessfulLogin(string body, string email)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.InputEncoding = System.Text.Encoding.Unicode;
            SmtpEmailService emailService = new SmtpEmailService();
            Message info = new Message()
            {
                Subject = "Успішна регістрація",
                Body = body,
                To = email
            };
            //string html = File.ReadAllText(@"~/html/index.html");
            //html = html.Replace("{$Title}", "Добре істи");
            //info.Body = html;
            emailService.Send(info);
            //emailService.DownloadMessages();
        }        
    }
    
}
