using Microsoft.Extensions.Configuration;
using MyLib.Application.Services;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace MyLib.Infrastructure.Services;

public class SendGridEmailService(IConfiguration configuration) : IEmailService
{
    private readonly string _apiKey = configuration["SendGrid:ApiKey"] ?? throw new ArgumentNullException("SendGrid:ApiKey");
    private readonly string _fromEmail = configuration["SendGrid:FromEmail"] ?? throw new ArgumentNullException("SendGrid:FromEmail");
    private readonly string _fromName = configuration["SendGrid:FromName"] ?? "Library Catalog";

    public async Task SendEmailAsync(string toEmail, string subject, string htmlContent)
    {
        var client = new SendGridClient(_apiKey);
        var from = new EmailAddress(_fromEmail, _fromName);
        var to = new EmailAddress(toEmail);
        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent: null, htmlContent: htmlContent);
        var response = await client.SendEmailAsync(msg);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"SendGrid failed: {response.StatusCode}");
        }
    }
}