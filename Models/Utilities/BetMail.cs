using BetTrackApi.Dtos;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System.Diagnostics;

namespace BetTrackApi.Models.Utilities
{
    public class BetMail
    {
        private readonly string smtpServer = "smtp.gmail.com";
        private readonly int smtpPort = 587; // o el puerto que uses
        private readonly string smtpUser = "webettrack@gmail.com";
        private readonly string smtpPass = "ieeunopcnagwmxid";

        public async void SendEmail([FromBody] DtoBetMail request)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("BetTrack", smtpUser));
            email.To.Add(new MailboxAddress(request.To, request.To));
            email.Subject = request.Subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = request.Body
            };
            email.Body = bodyBuilder.ToMessageBody();

            using var smtp = new SmtpClient();
            try
            {
                await smtp.ConnectAsync(smtpServer, smtpPort, false);
                await smtp.AuthenticateAsync(smtpUser, smtpPass);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
