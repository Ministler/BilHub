using System;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace BilHub.Data
{
    public static class Utility
    {
        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        public static void CreatePasswordWithSalt(string password, byte[] salt, out byte[] passwordHash)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(salt))
            {
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        public static string GenerateRandomPassword()
        {
            string ret = "";
            Random random = new Random();
            int len = random.Next() % 8 + 9;
            for (int i = 0; i < len; i++)
            {
                char tmp = (char)('a' + (random.Next() % 26));
                char tmp2 = (char)('1' + (random.Next() % 9));
                char tmp3 = '_';
                char tmp4 = 'a';
                int tmp5 = random.Next() % 4;
                if (tmp5 == 0)
                    tmp4 = tmp;
                if (tmp5 == 1)
                    tmp4 = tmp2;
                if (tmp5 == 2)
                    tmp4 = tmp3;
                if (tmp5 == 3)
                    tmp4 = (char)('A' + tmp - 'a');
                ret = ret + tmp4.ToString();
            }
            return ret;
        }
        public static void SendMail(string mailaddress, string content)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("bilhubapp@gmail.com"));
            email.To.Add(MailboxAddress.Parse(mailaddress));
            email.Subject = "BilHub Password Recovery";
            email.Body = new TextPart(TextFormat.Plain) { Text = content };

            // send email
            SmtpClient smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("bilhubapp@gmail.com", "[PASSWORD]");
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}