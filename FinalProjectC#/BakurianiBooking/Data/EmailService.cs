using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Threading.Tasks;


namespace BakurianiBooking.Data
{
    public class EmailService
    {
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("*****", "*****"));
            message.To.Add(new MailboxAddress("Recipient Name", to));
            message.Subject = subject;

            message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = body
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync("*****", "*****");
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}
