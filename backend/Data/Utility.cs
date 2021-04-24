using System;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace backend.Data
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
            //todo
            //b=alfanumerik olsun sadece obur turlu iki tikla kopyalanmio
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
        public static string GenerateRandomCode()
        {
            string ret = "431274";
            //Todo
            //sadece rakam olsun
            return ret;
        }
        public static void SendMail(string mailaddress, string content, bool recovery)
        {
            string subject = recovery ? "BilHub Password Recovery" : "BilHub Email Verification";
            //Etheral.mail diye bi site, 1 2saat dayaniyo patlarsa yenisini alin
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("joel.koelpin66@ethereal.email"));
            email.To.Add(MailboxAddress.Parse(mailaddress));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Plain) { Text = content };

            // send email
            SmtpClient smtp = new SmtpClient();
            smtp.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("joel.koelpin66@ethereal.email", "TfywjSmqBrrFJ8ae5e");
            smtp.Send(email);
            smtp.Disconnect(true);
        }

        public static bool CheckIfInstructorEmail(string email)
        {
            //TODO
            //bunu sorted json file yapcaz binaray searchle aricak 
            if (email.Equals("instructor@bilkent.edu.tr"))
                return true;
            return false;
        }
    }
}