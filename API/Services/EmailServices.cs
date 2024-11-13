using System;
using System.Net;
using System.Net.Mail;

namespace API.Services;

public class EmailServices
{
    private readonly IConfiguration _config;

    public EmailServices(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendPasswordResetEmail(string email, string resetUrl)
    {
        var message = new MailMessage();
        message.From = new MailAddress(_config["Smtp:Username"]);
        message.To.Add(email);
        message.Subject = "Password Reset Request";
        message.Body = $"Please Copy the token and paste it to token field in web to reset a password: {resetUrl}";
        message.IsBodyHtml = true;

        using (
            var smtpClient = new SmtpClient(_config["Smtp:Host"], int.Parse(_config["Smtp:Port"]))
        )
        {
            smtpClient.Credentials = new NetworkCredential(
                _config["Smtp:Username"],
                _config["Smtp:Password"]
            );
            smtpClient.EnableSsl = true;
            await smtpClient.SendMailAsync(message);
        }
    }
}
