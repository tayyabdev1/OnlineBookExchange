using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;



namespace OnlineBookExchange.Helpers
{
    public static class EmailHelper
    {
        // Method to send email
        public static void SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                // Configure the email settings
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("tmirza217@gmail.com", "ypccdqjavsnkdcpv"),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("tmirza217@gmail.com", "Online Book Exchange"),
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
