using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;

namespace BacklogTracker.Infrastructure.Email
{
    public sealed class ResendEmailSender : IEmailSender
    {
        private readonly IHttpClientFactory _factory;
        private readonly IConfiguration _config;

        public ResendEmailSender(IHttpClientFactory factory, IConfiguration config)
        {
            _factory = factory;
            _config = config;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var apiKey = _config["RESEND_API_KEY"];
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new InvalidOperationException("RESEND_API_KEY is not configured.");

            var fromEmail = _config["Email:FromEmail"] ?? throw new InvalidOperationException("Email:FromEmail missing.");
            var fromName = _config["Email:FromName"] ?? "App";

            var payload = new
            {
                from = $"{fromName} <{fromEmail}>",
                to = new[] { email },
                subject,
                html = htmlMessage
            };

            var http = _factory.CreateClient();
            http.BaseAddress = new Uri("https://api.resend.com");
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var json = JsonSerializer.Serialize(payload);
            var resp = await http.PostAsync("/emails", new StringContent(json, Encoding.UTF8, "application/json"));
            var body = await resp.Content.ReadAsStringAsync();

            resp.EnsureSuccessStatusCode();
        }
    }
}