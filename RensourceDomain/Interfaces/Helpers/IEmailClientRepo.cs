using MimeKit;
using RensourceDomain.Helpers.EmailClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace RensourceDomain.Interfaces.Helpers
{
    public interface IEmailClientRepo
    {
        void SendEmailAsync(EmailMessage message);
        MailMessage CreateEmailMessage(EmailMessage message);
        Task SendAsync(MailMessage mailMessage);
    }
}
