﻿using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;

namespace Business.Services
{
    public static class EmailService
    {
        public static async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Account Confirmation", "svistunss.py@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using var client = new SmtpClient();
            await client.ConnectAsync("smtp.gmail.com", 465, true);
            await client.AuthenticateAsync("rom63760@gmail.com", "789173max");
            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
        }
    }
}
