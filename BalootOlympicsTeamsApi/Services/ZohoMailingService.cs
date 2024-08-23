using System.Text.Json;

namespace BalootOlympicsTeamsApi.Services;

public class ZohoMailingService(IOptions<EmailSettings> mailSettings, IHttpClientFactory clientFactory, ILogger<ZohoMailingService> logger)
{
    private readonly EmailSettings _mailSettings = mailSettings.Value;
    private readonly ILogger<ZohoMailingService> _logger = logger;
    private readonly IHttpClientFactory _clientFactory = clientFactory;
    public async Task<Result<string>> SendOtpToEmailAsync(string email, string otp)
    {
        var emailSubject = "تأكيد الانضمام الى فريق بدورة الالعاب السعودية";
        return await SendByApi(email, emailSubject, otp);
    }

    private record EmailAddress(string Address, string Name);
    private record ToEmailAddress(EmailAddress Email_address);

    private async Task<Result<string>> SendByApi(string mailTo, string subject, string otp)
    {
        try
        {
            using var httpClient = _clientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization =
                            new System.Net.Http.Headers.AuthenticationHeaderValue("Zoho-enczapikey", _mailSettings.Token);
            HttpResponseMessage response =
                await httpClient.PostAsJsonAsync(new Uri($"{_mailSettings.ApiUrl}"),
                    CreateTemplateMessage(mailTo, otp, subject),
                    options: new JsonSerializerOptions()
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });

            if (!response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                _logger.LogCritical("Zoho Api Mailing Service has Failure Status Code {statusCode} and response body : {response}", response.StatusCode, jsonResponse);
                return Result.Fail(new OtpEmailSendingError("ZohoApi"));
            }
            return Result.Ok($"ZohoApi:Mailing:{_mailSettings.SenderEmail}");
        }
        catch (Exception ex)
        {
            _logger.LogCritical("Zoho Api Mailing Service has Exception {exp}", ex);
            return Result.Fail(new OtpEmailSendingError("ZohoApi").CausedBy(ex));
        }
    }
    private object CreateTemplateMessage(string mailTo, string otp, string subject)
    {
        return new
        {
            Mail_template_key = _mailSettings.TemplateKey,
            From = new EmailAddress(_mailSettings.SenderEmail, _mailSettings.SenderName),
            To = new List<ToEmailAddress>() { new(new(mailTo, "noreply")) },
            Merge_info = new
            {
                Otp = otp
            },
            Subject = subject
        };
    }
}
