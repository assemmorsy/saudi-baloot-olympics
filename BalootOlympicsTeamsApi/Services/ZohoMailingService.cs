using System.Text.Json;

namespace BalootOlympicsTeamsApi.Services;

public class ZohoMailingService(IOptions<EmailSettings> mailSettings, IHttpClientFactory clientFactory, ILogger<ZohoMailingService> logger)
{
    private readonly EmailSettings _mailSettings = mailSettings.Value;
    private readonly ILogger<ZohoMailingService> _logger = logger;
    private readonly IHttpClientFactory _clientFactory = clientFactory;
    private static async Task<string> GenerateConfirmEmailBody(string otp)
    {
        string fileName = "confirmEmailCode.html";
        string path = Path.Combine(Environment.CurrentDirectory, @"Templates", fileName);
        StreamReader streamReader = new(path);
        string mailText = await streamReader.ReadToEndAsync();
        streamReader.Close();
        return mailText.Replace("[code]", otp);
    }

    public async Task<Result> SendOtpToEmailAsync(string email, string otp)
    {
        var emailSubject = "تأكيد الانضمام الى فريق بدورة الالعاب السعودية";
        var emailBody = await GenerateConfirmEmailBody(otp);
        _ = SendByApi(email, emailSubject, emailBody);
        return Result.Ok();
    }

    private record EmailAddress(string Address, string Name);
    private record ToEmailAddress(EmailAddress Email_Address);
    private record ZohoApiRequestDto(EmailAddress From, object[] To, string Subject, string HtmlBody);
    private async Task<Result<string>> SendByApi(string mailTo, string subject, string body)
    {
        try
        {
            using var httpClient = _clientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization =
                            new System.Net.Http.Headers.AuthenticationHeaderValue("Zoho-enczapikey", _mailSettings.Token);

            var from = new EmailAddress(_mailSettings.SenderEmail, _mailSettings.SenderName);
            var to = new EmailAddress(mailTo, "noreply");
            ZohoApiRequestDto reqBody = new(from, [new { email_address = to }], subject, body);

            HttpResponseMessage response =
                await httpClient.PostAsJsonAsync(new Uri($"{_mailSettings.ApiUrl}"), reqBody, options: new JsonSerializerOptions()
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
}
