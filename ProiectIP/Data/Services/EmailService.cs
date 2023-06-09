﻿/**************************************************************************
 *                                                                        *
 *  Description: Sistem de rezervari bilete cinema                        *
 *  Website:     https://github.com/Momoleanu/ProiectIP                   *
 *  Copyright:   (c) 2023, Holban Mihnea, Dumitru Andrei                  *
 *                                                                        *
 *  This code and information is provided "as is" without warranty of     *
 *  any kind, either expressed or implied, including but not limited      *
 *  to the implied warranties of merchantability or fitness for a         *
 *  particular purpose. You are free to use this source code in your      *
 *  applications as long as the original copyright notice is included.    *
 *                                                                        *                      
 *                                                                        *
 **************************************************************************/


using Microsoft.Extensions.Options;
using ProiectIP.Data.Services;
using ProiectIP.Models;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

/// <summary>
/// Clasa EmailService implementează interfața IEmailService și gestionează trimiterea de e-mailuri.
/// </summary>
public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;

    /// <summary>
    /// Constructorul clasei EmailService.
    /// </summary>
    /// <param name="emailSettings">Setările de e-mail preluate din opțiuni.</param>
    public EmailService(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }

    /// <summary>
    /// Metoda SendEmailAsync trimite un e-mail către o adresă specificată.
    /// </summary>
    /// <param name="toEmail">Adresa de e-mail destinatară.</param>
    /// <param name="subject">Subiectul e-mailului.</param>
    /// <param name="body">Corpul e-mailului.</param>
    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var smtpClient = new SmtpClient
        {
            Host = _emailSettings.SmtpServer,
            Port = _emailSettings.SmtpPort,
            EnableSsl = true,
            Credentials = new NetworkCredential(_emailSettings.SmtpUsername, _emailSettings.SmtpPassword)
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        mailMessage.To.Add(toEmail);

        await smtpClient.SendMailAsync(mailMessage);
    }
}
