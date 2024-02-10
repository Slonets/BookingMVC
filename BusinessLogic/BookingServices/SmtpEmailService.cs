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
    public class SmtpEmailService : ISmtpEmailService
    {
        // Конфігураційні параметри електронної пошти
        private readonly EmailConfiguration _configuration;

        // Конструктор класу, ініціалізує об'єкт конфігурації
        public SmtpEmailService()
        {
            _configuration = new EmailConfiguration();
        }

        // Метод для відправлення електронного листа
        public void Send(Message message)
        {
            // Створення тіла листа
            var body = new TextPart("html")
            {
                Text = message.Body             
            };

            // Шлях до вкладення (зображення)
            //string path = @"D:\New\New\wwwroot\images\register.png";

            //// Створення об'єкта для вкладення (зображення)
            //var attachment = new MimePart("image", "jpeg")
            //{
            //    FileName = "Регістрація",
            //    Content = new MimeContent(File.OpenRead(path))
            //};

            // Створення багаточастинового повідомлення
            var multipart = new Multipart("mixed");
            multipart.Add(body);
            //multipart.Add(attachment);

            // Створення об'єкта електронного листа
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_configuration.From));
            emailMessage.To.Add(new MailboxAddress(message.To));
            emailMessage.Subject = message.Subject;
            emailMessage.Body = multipart;

            // Використання SmtpClient для відправлення листа
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

        // Метод для завантаження повідомлень з поштового сервера
        public void DownloadMessages()
        {
            // Використання ImapClient для роботи з IMAP-протоколом
            using (var client = new ImapClient())
            {
                // Підключення до поштового сервера
                client.Connect("imap.ukr.net", 993, MailKit.Security.SecureSocketOptions.SslOnConnect);
                client.Authenticate(_configuration.UserName, _configuration.Password);

                // Відкриття скриньки вхідних повідомлень
                client.Inbox.Open(MailKit.FolderAccess.ReadOnly);

                // Пошук унікальних ідентифікаторів повідомлень
                var uids = client.Inbox.Search(SearchQuery.All);

                // Цикл для обробки кожного повідомлення
                foreach (var uid in uids)
                {
                    // Отримання повідомлення за його унікальним ідентифікатором
                    var message = client.Inbox.GetMessage(uid);

                    // Виведення інформації про повідомлення на консоль
                    Console.WriteLine("------------------");
                    Console.WriteLine("From: {0}", message.From);
                    Console.WriteLine("Subject: {0}", message.Subject);

                    // Запис повідомлення у файл з розширенням .eml
                    message.WriteTo(string.Format("{0}.eml", uid));
                }

                // Відключення від поштового сервера
                client.Disconnect(true);
            }
        }

        // Метод для відправлення успішного листа після реєстрації
        public void SuccessfulLogin(string emailUser, string password, string email)
        {
            // Налаштування консолі на використання Unicode
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.InputEncoding = System.Text.Encoding.Unicode;

            // Створення нового екземпляру SmtpEmailService
            SmtpEmailService emailService = new SmtpEmailService();

            // Створення об'єкта Message для успішної реєстрації
            Message info = new Message()
            {
                Subject = "Успішна реєстрація",
                Body="",
                To = email
            };

            // Зчитування HTML-коду з файлу та встановлення його як тіла повідомлення
            string html = File.ReadAllText(@"D:\New\New\wwwroot\html\index.html");

            string newHtml = html.Replace("Email2", emailUser);
            newHtml = newHtml.Replace("Password2", password);

            info.Body = newHtml;

            // Відправлення повідомлення
            emailService.Send(info);
        }
    }

}
