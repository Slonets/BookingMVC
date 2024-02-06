using BusinessLogic.BookingServices;
using BusinessLogic.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public static class ServiceExtensions
    {
        public static void AddCustomService(this IServiceCollection servicesCollection)
        {
            servicesCollection.AddScoped<IAccountService, AccountService>();
            servicesCollection.AddScoped<IMailService, MailService>();

        }

        public static void CustomMapper(this IServiceCollection servicesCollection)
        {
            servicesCollection.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }        
    }
}
