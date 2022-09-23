using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using Newtonsoft.Json;
using RensourceDomain.Configurations;
using RensourceDomain.Helpers.EmailClient;
using RensourceDomain.Interfaces.Helpers;
using RensourceDomain.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace RensourceDomain.Interfaces.Repos.Helpers
{
    public class EmailClientRepo : IEmailClientRepo
    {
        private readonly EmailConfig _emailConfig;
        private readonly ILogger<EmailClientRepo> _logger;

        public EmailClientRepo(IOptions<EmailConfig> emailConfig, ILogger<EmailClientRepo> logger)
        {
            _emailConfig = emailConfig.Value;
            _logger = logger;
        }
        public MailMessage CreateEmailMessage(EmailMessage message)
        {
            try
            {
                //get the template
                var emailMessage = new MailMessage();
                emailMessage.From = new MailAddress(_emailConfig.From, _emailConfig.DisplayName);
                emailMessage.To.Add(message.To);
                //emailMessage.CC.Add("");
                emailMessage.Subject = _emailConfig.Subject;
                emailMessage.Body = message.Content;
                emailMessage.IsBodyHtml = true;
                //emailMessage.BodyEncoding = Encoding.UTF8;
                emailMessage.Sender = new MailAddress(_emailConfig.From);

                _logger.LogInformation($"Mail Message:=> [{JsonConvert.SerializeObject(emailMessage)}] DateTime: {DateTime.Now}");
                return emailMessage;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
                return default;
            }
        }

        public MailMessage ContactUSMail(ContactUsRequest? message, string? content)
        {
            try
            {
                //get the template
                var emailMessage = new MailMessage();
                emailMessage.From = new MailAddress(message?.EmailAddress, message.SendersName);
                emailMessage.To.Add(_emailConfig.RensourceEmailAddress);
                //emailMessage.CC.Add("");
                emailMessage.Subject = message.MessageSubject;
                emailMessage.Body = content;
                emailMessage.IsBodyHtml = true;
                //emailMessage.BodyEncoding = Encoding.UTF8;
                emailMessage.Sender = new MailAddress(message.EmailAddress);

                _logger.LogInformation($"Mail Message:=> [{JsonConvert.SerializeObject(emailMessage)}] DateTime: {DateTime.Now}");
                return emailMessage;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
                return default;
            }
        }

        public async Task SendAsync(MailMessage mailMessage)
        {
            try
            {
                try
                {
                    using (var client = new SmtpClient())
                    {
                        client.Host = _emailConfig?.SmtpServer;
                        client.Port = _emailConfig.Port;
                        client.EnableSsl = true;
                        client.UseDefaultCredentials = false;
                        client.Credentials = new NetworkCredential(_emailConfig.UserName, _emailConfig.Password);
                        client.Send(mailMessage);

                        _logger.LogInformation($"SMTP Config:=> [Host => {client.Host}]--[Port => {client.Port}]");
                        _logger.LogInformation($"[Username => {_emailConfig.UserName}]--[Password => {_emailConfig.Password}] DateTime: {DateTime.Now}");
                        client.Dispose();
                    }
                }
                catch (SmtpFailedRecipientException ex)
                {
                    _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; InnerException: {ex.InnerException}; Source: {ex.Source} DateTime: {DateTime.Now}");
                    throw new Exception($"SMTP Failed to send Mail {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; InnerException: {ex.InnerException}; Source: {ex.Source} DateTime: {DateTime.Now}");
                throw new Exception($"SMTP Failed to send Mail {ex.Message}");
            }
        }

        public void SendEmailAsync(EmailMessage message)
        {
            try
            {
                var emailMessage = CreateEmailMessage(message);
                SendAsync(emailMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
            }
        }

        public void SendContactUsEmailAsync(ContactUsRequest message, string? content)
        {
            try
            {
                var emailMessage = ContactUSMail(message, content);
                SendAsync(emailMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Mesage: {ex.Message}; StackTrace: {ex.StackTrace}; DateTime: {DateTime.Now}");
            }
        }
    }
}
