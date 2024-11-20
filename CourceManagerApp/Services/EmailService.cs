using System.Net.Mail;
using System.Net;

namespace CourceManagerApp.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendConfirmationEmail(string toEmail, string studentName, int courseId)
        {
            // Load email settings from appsettings.json
            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
            var senderEmail = _configuration["EmailSettings:SenderEmail"];
            var senderPassword = _configuration["EmailSettings:SenderPassword"];

            var subject = "Course Enrollment Confirmation";
            var body = $"Hello {studentName},<br/><br/>" +
                       "Please confirm your enrollment by clicking the following link:<br/>" +
                       $"<a href='https://localhost:7102/Enrollment/Confirm?courseId={courseId}&studentEmail={toEmail}'>Confirm Enrollment</a>";

            var mailMessage = new MailMessage
            {
                From = new MailAddress(senderEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);

            using (var smtpClient = new SmtpClient(smtpServer, smtpPort))
            {
                smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
                smtpClient.EnableSsl = true;
                try
                {
                    await smtpClient.SendMailAsync(mailMessage);
                }
                catch (Exception ex)
                {
                    // Log the error or show a message
                    Console.WriteLine($"Email sending failed: {ex.Message}");
                    throw; // Re-throw if needed
                }

            }

        }
    }
}
