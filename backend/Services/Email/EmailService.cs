// using Microsoft.Extensions.Options;
// using MimeKit;
// using System.Net.Mail;

// namespace backend.Services.Email
// {
//     public class EmailService : IEmailService
//     {
//         private readonly EmailConfiguration _emailConfiguration;

//         public EmailService(EmailConfiguration emailConfiguration)
//         {
//             _emailConfiguration = emailConfiguration;
//         }
//         public void Send(Message message)
//         {
//             var emailMessage = CreateEmailMessage(message);
//         }

//         private MimeMessage CreateEmailMessage(Message message)
//         {
//             var emailMessage = new MimeMessage();
//             emailMessage.From.Add(new MailboxAddress(_emailConfiguration.From));
//             emailMessage.To.Add(message.To);
//             emailMessage.Subject = message.Subject;
//             emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content};
//             return emailMessage;

//         }
//         private void SendMail(MimeMessage mimeMessage) {
//             using (var client = new SmtpClient() )
//             {
//                 try {
//                     client.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.Port, true);
//                     client.Send(mimeMessage);
//                 }
//                 catch
//                 {
//                     throw;
//                 }
//             }
//         }
//     }
// }