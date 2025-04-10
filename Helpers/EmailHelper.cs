using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.Configuration;



namespace OnlineBookExchange.Helpers
{
    public static class EmailHelper
    {
        // Method to send email
        public static void SendEmail(string toEmail, string subject, string body)
        {
            string email = ConfigurationManager.AppSettings["EmailAddress"];
            string password = ConfigurationManager.AppSettings["EmailPassword"];
            try
            {
                // Configure the email settings
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(email, password),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(email, "Online Book Exchange"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(toEmail);
                smtpClient.Send(mailMessage);
            }
            catch (SmtpException ex)
            {
                Console.WriteLine("SMTP error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending email: " + ex.Message);
            }
        }
    }
}
