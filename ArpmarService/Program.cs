using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Net;
using System.ServiceProcess;
using ArpmarService.Services;

namespace ArpmarService
{
    internal static class Program
    {
        private static void Main()
        {
            var appSettings = ConfigurationManager.AppSettings;

            using (var logService = new LogService())
            {
                using (var mailService = new MailService(logService))
                {
                    Configure(mailService, appSettings);

                    var mainService = new ArpmarService(logService, mailService)
                    {
                        CheckForChangesInterval = Convert.ToInt32(appSettings["CheckForChangesInterval"])
                    };

                    ServiceBase.Run(mainService);
                }
            }
        }

        private static void Configure(MailService mailService, NameValueCollection appSettings)
        {
            mailService.Client.Host = appSettings["SMTP-Host"];
            mailService.Client.Port = Convert.ToInt32(appSettings["SMTP-Port"]);
            mailService.Client.EnableSsl = Convert.ToBoolean(appSettings["SMTP-EnableSSL"]);
            mailService.Client.Credentials =
                new NetworkCredential(appSettings["SMTP-Login"], appSettings["SMTP-Password"]);

            mailService.From = appSettings["SMTP-From"];
            mailService.To = appSettings["SMTP-To"];
        }
    }
}