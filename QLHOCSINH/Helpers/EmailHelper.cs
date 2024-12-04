using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace QLHOCSINH.Helpers
{
    public static class EmailHelper
    {
        public static async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("hoanggnk.it@gmail.com", "uflx dsaw zahi tzgj"),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("hoanggnk.it@gmail.com"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }

}