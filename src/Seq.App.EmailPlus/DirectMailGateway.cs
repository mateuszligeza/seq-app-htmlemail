﻿using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;

namespace Seq.App.EmailPlus
{
    class DirectMailGateway : IMailGateway
    {
        public async Task SendAsync(SmtpOptions options, MimeMessage message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            var client = new SmtpClient();

            if (options.SkipServerCertificateValidation)
                client.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

            await client.ConnectAsync(options.Host, options.Port, options.SocketOptions);
            if (options.RequiresAuthentication)
                await client.AuthenticateAsync(options.Username, options.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}