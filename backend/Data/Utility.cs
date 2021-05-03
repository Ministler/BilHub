using System;
using System.Collections.Generic;
using System.IO;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
            string ret = "";
            Random random = new Random();
            int len = random.Next() % 8 + 9;
            for (int i = 0; i < len; i++)
            {
                char tmp = (char)('a' + (random.Next() % 26));
                char tmp2 = (char)('1' + (random.Next() % 9));
                char tmp4 = 'a';
                int tmp5 = random.Next() % 4;
                if (tmp5 == 0)
                    tmp4 = tmp;
                if (tmp5 == 1)
                    tmp4 = tmp2;
                if (tmp5 == 2)
                    tmp4 = tmp;
                if (tmp5 == 3)
                    tmp4 = (char)('A' + tmp - 'a');
                ret = ret + tmp4.ToString();
            }
            return ret;
        }
        public static string GenerateRandomCode()
        {
            string ret = "";
            Random random = new Random();
            int len = 12;
            for (int i = 0; i < len; i++)
            {
                char tmp = (char)('1' + (random.Next() % 9));
                ret = ret + tmp.ToString();
            }
            return ret;
        }
        public static void SendMail(string mailaddress, string content, bool recovery)
        {
            string subject = recovery ? "BilHub Password Recovery" : "BilHub Email Verification";
            content = recovery ? "Your recovery password for BilHub is: " + content : "Your verification code for BilHub is: " + content;

            var email = new MimeMessage();
            //^^^^^^^^^^^^^^^^ sadece productionda bu emaile gecicez ^^^^^^^^^^^^^^^^^^^^^^^^^^^^
            email.From.Add(MailboxAddress.Parse("bilhub@bilkent.edu.tr"));
            email.To.Add(MailboxAddress.Parse(mailaddress));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Plain) { Text = content };

            // send email
            SmtpClient smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("bilhubapp@gmail.com", "BilHubApp-Ministler");
            smtp.Send(email);
            smtp.Disconnect(true);

            // ///^^^^^^^^^^^ testing icin etheral mail ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
            // email.From.Add(MailboxAddress.Parse("felix.kihn39@ethereal.email"));
            // email.To.Add(MailboxAddress.Parse(mailaddress));
            // email.Subject = subject;
            // email.Body = new TextPart(TextFormat.Plain) { Text = content };

            // // send email
            // SmtpClient smtp = new SmtpClient();
            // smtp.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
            // smtp.Authenticate("felix.kihn39@ethereal.email", "v2pPMTNgnQhdB3muC1");
            // smtp.Send(email);
            // smtp.Disconnect(true);
        }

        public static bool CheckIfInstructorEmail(string email)
        {
            var array = Instructors.instrs;
            if (binarySearch(array, email, array.Length))
                return true;
            return false;
        }
        private static bool binarySearch(string[] arr, string x, int n)
        {
            int l = 0;
            int r = n - 1;
            while (l <= r)
            {
                int m = l + (r - l) / 2;
                if (x.Equals(arr[m]))
                    return true;
                if (string.Compare(x, arr[m]) > 1)
                    l = m + 1;
                else
                    r = m - 1;
            }
            return false;
        }
    }
}