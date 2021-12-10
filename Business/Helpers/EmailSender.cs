using System;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Business.Interfaces;
using DAL.Models.Entities;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Business.Helpers
{
    public sealed class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;

        public EmailSender(IConfiguration configuration, UserManager<User> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task SendConfirmationEmailAsync(User user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var tokenBytes = Encoding.UTF8.GetBytes(token);
            var tokenEncoded = WebEncoders.Base64UrlEncode(tokenBytes);

            var scheme = _configuration.GetValue<string>("AuthURI:Scheme");
            var host = _configuration.GetValue<string>("AuthURI:Host");
            var authPath = _configuration.GetValue<string>("AuthURI:Path");
            var port = _configuration.GetValue<int>("AuthURI:Port");

            var confirmationUri = new UriBuilder
            {
                Scheme = scheme,
                Host = host,
                Path = authPath,
                Port = port
            };
            var query = HttpUtility.ParseQueryString(confirmationUri.Query);
            query["id"] = user.Id.ToString();
            query["token"] = tokenEncoded;
            confirmationUri.Query = query.ToString()!;

            await SendEmailAsync(user.Email, "Confirm your account",
                $"Verify your account by clicking the <a href='{confirmationUri}'>link</a>");
        }

        private static async Task SendEmailAsync(string email, string subject, string message)
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
