﻿using System.Net.Http.Json;
namespace BalootOlympicsTeamsApi.Services;

public class WhatsAppService(IOptions<WhatsAppSettings> whatsSettings, IHttpClientFactory clientFactory, ILogger<WhatsAppService> logger)
{
    private readonly WhatsAppSettings _whatsSettings = whatsSettings.Value;
    private readonly IHttpClientFactory _clientFactory = clientFactory;
    private readonly ILogger<WhatsAppService> _logger = logger;

    public async Task<Result<string>> SendOtpAsync(string phoneNum, string username, string otp)
    {
        if (!phoneNum.StartsWith('+'))
            phoneNum = $"+{phoneNum}";

        var body = new
        {
            messaging_product = "whatsapp",
            to = phoneNum,
            type = "template",
            template = new
            {
                name = "otp",
                language = new
                {
                    code = "ar"
                },
                components = new object[]
                {
                    new
                    {
                        type = "body",
                        parameters = new object[]
                        {
                            new
                            {
                                type = "text",
                                text = otp
                            }
                        }
                    },
                    new
                    {
                        type = "button",
                        sub_type = "url",
                        index = "0",
                        parameters = new object[]
                        {
                            new
                            {
                                type = "payload",
                                payload = otp
                            }
                        }
                    }
                }
            }
        };
        try
        {
            using var httpClient = _clientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _whatsSettings.Token);
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(new Uri($"{_whatsSettings.ApiUrl}/{_whatsSettings.InstanceId}/messages"), body);
            if (!response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                _logger.LogCritical("WhatsApp has Failure Status Code {statusCode} and response body : {response}", response.StatusCode, jsonResponse);
                return Result.Fail(new OtpPhoneSendingError("Official_WhatApp"));
            }
            return Result.Ok($"WhatsApp:Official:{_whatsSettings.InstanceId}");
        }
        catch (Exception ex)
        {
            _logger.LogCritical("WhatsApp Service has Exception {exp}", ex);
            return Result.Fail(new OtpPhoneSendingError("Official_WhatApp").CausedBy(ex));
        }
    }

    public async Task<Result<string>> SendMessageFromTemplateAsync(string phoneNum, string templateName)
    {
        if (!phoneNum.StartsWith('+'))
            phoneNum = "+" + phoneNum;

        var body = new
        {
            messaging_product = "whatsapp",
            to = phoneNum,
            type = "template",
            template = new
            {
                name = templateName,
                language = new
                {
                    code = "ar"
                },
            }
        };
        try
        {
            using var httpClient = _clientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _whatsSettings.Token);
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(new Uri($"{_whatsSettings.ApiUrl}/{_whatsSettings.InstanceId}/messages"), body);
            if (!response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                _logger.LogCritical("WhatsApp has Failure Status Code {statusCode} and response body : {response}", response.StatusCode, jsonResponse);
                return Result.Fail(new OtpPhoneSendingError("Official_WhatApp"));
            }
            return Result.Ok($"WhatsApp:Official:{_whatsSettings.InstanceId}");
        }
        catch (Exception ex)
        {
            _logger.LogCritical("WhatsApp Service has Exception {exp}", ex);
            return Result.Fail(new OtpPhoneSendingError("Official_WhatApp").CausedBy(ex));
        }
    }
}
