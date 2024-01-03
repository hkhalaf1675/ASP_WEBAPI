﻿using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;

namespace Sending_Email.Services
{
    public class MailingService:IMailingService
    {
        private readonly MailSetting mailSetting;

        public MailingService(IOptions<MailSetting> _mailSetting)
        {
            this.mailSetting = _mailSetting.Value;
        }
        public async Task<string> SendEmailAsync(string email, string subject, string body)
        {
            try
            {
                MailMessage message = new()
                {
                    From = new MailAddress(mailSetting.Email),
                    Subject = subject,
                    Body = $"<html><body>{body}</body></html>",
                    IsBodyHtml = true
                };
                message.To.Add(email);

                var smtpClient = new System.Net.Mail.SmtpClient(mailSetting.Host)
                {
                    Port = mailSetting.Port,
                    Credentials = new NetworkCredential(mailSetting.Email, mailSetting.Password),
                    EnableSsl = mailSetting.EnableSsl,
                };

                await smtpClient.SendMailAsync(message);
                return "Email Send Successfully!";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
