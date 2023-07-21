
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Net;
using WorkWave.Services.Abstracts;


namespace WorkWave.Services
{
    public class EmailService : IEmailService
    { 

        public void SendEmailAsync(string emeail, string subject, string m)
        {
            
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("florida.klein15@ethereal.email"));
            email.To.Add(MailboxAddress.Parse("florida.klein15@ethereal.email"));
            email.Subject = "test";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = "hi" };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("florida.klein15@ethereal.email", "Qc4yv3jC3zmC8ZqmAg");
            smtp.Send(email);
            smtp.Disconnect(true);
           
        }
    }
}
