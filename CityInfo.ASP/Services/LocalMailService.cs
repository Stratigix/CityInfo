using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.ASP.Services
{
    public class LocalMailService : IMailService
    {
        private string _mailTo = Startup.Configuration["mailsettings:mailToAddress"];
        private string _mailFrom = Startup.Configuration["mailsettings:mailFromAddress"];

        public void Send(string subject, string message)
        {
            Debug.WriteLine($"Mail from {_mailFrom} to {_mailTo} using our {GetType().Name}.");
            Debug.WriteLine($"Subject: {subject}");
            Debug.WriteLine($"Message: {message}");
        }
    }
}
