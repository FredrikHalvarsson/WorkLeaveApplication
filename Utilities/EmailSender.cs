using Microsoft.AspNetCore.Identity.UI.Services;

namespace LeaveApplicationApp.Utilities
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            //For future EmailService logic
            return Task.CompletedTask;
        }
    }
}
