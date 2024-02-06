using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IMailService
    {
        //надсилання листа до юсера з його замовленням       
        Task SendMailAsync(string toEmail, string subject, string body);
    }
}
