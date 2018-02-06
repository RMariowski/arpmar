using System;
using System.Net.Mail;

namespace ArpmarService.Services
{
    public class MailService : IDisposable
    {
        private readonly LogService _logService;

        public SmtpClient Client { get; }
        public string From { private get; set; }
        public string To { private get; set; }

        public MailService(LogService logService)
        {
            _logService = logService;

            Client = new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false
            };
        }

        public void SendMail(string bodyOfMail)
        {
            using (var mail = new MailMessage(From, To)
            {
                Subject = "arpMAR - ARP table changed",
                Body = bodyOfMail
            })
            {
                try
                {
                    Client?.Send(mail);
                }
                catch (Exception e)
                {
                    _logService.WriteWarning(e.Message);
                }
            }
        }

        public void Dispose() 
            => Client?.Dispose();
    }
}