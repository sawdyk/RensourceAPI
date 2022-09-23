using MimeKit;
using RensourceDomain.Helpers.EmailClient;
using RensourceDomain.Models.Request;
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
        void SendContactUsEmailAsync(ContactUsRequest message, string? content);
        MailMessage ContactUSMail(ContactUsRequest message, string? content);
        Task SendAsync(MailMessage mailMessage);
    }
}
