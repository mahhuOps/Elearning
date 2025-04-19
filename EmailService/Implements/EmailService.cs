using EmailService.Interfaces;
using EmailService.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace EmailService.Implements
{
    public class EmailService : IEmailService
    {
        SmtpClient _smtpClient;

        async Task OpenConnect(MailConfig mailConfig)
        {
            _smtpClient = new SmtpClient();

            _smtpClient.Connect(mailConfig.Host, mailConfig.Port, SecureSocketOptions.Auto);
            await _smtpClient.AuthenticateAsync(mailConfig.Email, mailConfig.Password);
        }

        void Disconnect()
        {
            _smtpClient.Disconnect(true);
        }

        public async Task<bool> SendMail(MailConfig mailConfig, MailModel mail)
        {
            if(mail.To == null || mail.To.Count  == 0) throw new Exception("Gửi mail không có người nhận.");

            try
            {
                await OpenConnect(mailConfig);
                var emailMessage = new MimeMessage();
                var mailSender = new MailboxAddress(mailConfig.DisplayName, mailConfig.Email);

                var toAddresses = mail.To.Select(to => new MailboxAddress("",to));

                emailMessage.Subject = mail.Subject;
                emailMessage.Body = new TextPart("html") { Text = mail.Body };
                emailMessage.From.Add(mailSender);
                emailMessage.To.AddRange(toAddresses);

                var res = await _smtpClient.SendAsync(emailMessage);
                Disconnect();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> ValidateMailConfig(MailConfig mailConfig)
        {
            try
            {
                await OpenConnect(mailConfig);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
