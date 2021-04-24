// using MailKit.Net.Smtp;
// using MailKit.Security;
// using Microsoft.Extensions.Options;
// using MimeKit;
// using MimeKit.Text;

// namespace backend.Services.Email
// {
//     public interface IEmailService
//     {
//         void Send(string from, string to, string subject, string html);
//     }

//     public class EmailService : IEmailService
//     {
//         private readonly EmailConfiguration _emailConfiguration;

//         public EmailService(EmailConfiguration emailConfiguration)
//         {
//             _emailConfiguration = emailConfiguration;
//         }

//         public void Send(string from, string to, string subject, string html)
//         {
//             // create message
//             var email = new MimeMessage();
//             email.From.Add(MailboxAddress.Parse(from));
//             email.To.Add(MailboxAddress.Parse(to));
//             email.Subject = subject;
//             email.Body = new TextPart(TextFormat.Plain) { Text = html };

//             // send email
//             using var smtp = new SmtpClient();
//             smtp.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.Port, SecureSocketOptions.StartTls);
//             smtp.Authenticate(_emailConfiguration.UserName, _emailConfiguration.Password);
//             smtp.Send(email);
//             smtp.Disconnect(true);
//         }
//     }
// }