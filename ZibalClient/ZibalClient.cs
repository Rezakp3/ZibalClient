using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using ZibalClient.Common;
using ZibalClient.Enums;
using ZibalClient.Extensions;
using ZibalClient.Models.Inquiry;
using ZibalClient.Models.Request;
using ZibalClient.Models.Verify;
using ZibalClient.Options;

namespace ZibalClient;

/// <summary>
/// پیاده‌سازی کلاینت استاندارد زیبال با استفاده از Typed HttpClient
/// </summary>
public class ZibalClient : IZibalClient
{
    private readonly HttpClient _httpClient;
    private readonly ZibalOptions _options;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public ZibalClient(HttpClient httpClient, IOptions<ZibalOptions> options)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }

    /// <inheritdoc />
    public async Task<ZibalResult<ZibalPaymentResponse>> RequestPaymentAsync(
        ZibalPaymentRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        // پر کردن خودکار مرچنت در صورت خالی بودن
        if (string.IsNullOrWhiteSpace(request.Merchant))
        {
            request.Merchant = _options.GetEffectiveMerchant();
        }

        try
        {
            using var response = await _httpClient.PostAsJsonAsync(
                "v1/request",
                request,
                JsonOptions,
                cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return ZibalResult<ZibalPaymentResponse>.Failure(
                    (int)response.StatusCode,
                    $"خطای ارتباط با سرور زیبال (HTTP {(int)response.StatusCode}).");
            }

            var result = await response.Content.ReadFromJsonAsync<ZibalPaymentResponse>(
                JsonOptions,
                cancellationToken);

            if (result is null)
            {
                return ZibalResult<ZibalPaymentResponse>.Failure(
                    (int)ZibalResultCode.Unknown,
                    "پاسخ دریافتی از درگاه خالی یا نامعتبر است.");
            }

            // کد ۱۰۰ به معنی درخواست موفق و صدور توکن است
            return result.Result == (int)ZibalResultCode.Success
                ? ZibalResult<ZibalPaymentResponse>.Success(result, result.Result, result.PersianMessage)
                : ZibalResult<ZibalPaymentResponse>.Failure(result.Result, result.PersianMessage, result.Message);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw; // به درخواست فراخواننده لغو شده است
        }
        catch (Exception ex)
        {
            return ZibalResult<ZibalPaymentResponse>.Failure(
                -1,
                "خطا در برقراری ارتباط اینترنتی با درگاه پرداخت زیبال.",
                ex.Message);
        }
    }

    /// <inheritdoc />
    public async Task<ZibalResult<ZibalVerifyResponse>> VerifyPaymentAsync(
        ZibalVerifyRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (string.IsNullOrWhiteSpace(request.Merchant))
        {
            request.Merchant = _options.GetEffectiveMerchant();
        }

        try
        {
            using var response = await _httpClient.PostAsJsonAsync(
                "v1/verify",
                request,
                JsonOptions,
                cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return ZibalResult<ZibalVerifyResponse>.Failure(
                    (int)response.StatusCode,
                    $"خطای ارتباط با سرور زیبال در تایید تراکنش (HTTP {(int)response.StatusCode}).");
            }

            var result = await response.Content.ReadFromJsonAsync<ZibalVerifyResponse>(
                JsonOptions,
                cancellationToken);

            if (result is null)
            {
                return ZibalResult<ZibalVerifyResponse>.Failure(
                    (int)ZibalResultCode.Unknown,
                    "پاسخ تایید دریافتی از درگاه زیبال نامعتبر است.");
            }

            // در زیبال هم کد ۱۰۰ (موفق) و هم کد ۲۰۱ (قبلاً تایید شده) تایید موفق بانکی به شمار می‌روند
            var isSuccess = result.Result is (int)ZibalResultCode.Success or (int)ZibalResultCode.AlreadyVerified;
            var message = result.ResultCode.ToPersianMessage();

            return isSuccess
                ? ZibalResult<ZibalVerifyResponse>.Success(result, result.Result, message)
                : ZibalResult<ZibalVerifyResponse>.Failure(result.Result, message, result.Message);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            return ZibalResult<ZibalVerifyResponse>.Failure(
                -1,
                "خطا در ارسال درخواست تایید تراکنش به درگاه زیبال.",
                ex.Message);
        }
    }

    /// <inheritdoc />
    public Task<ZibalResult<ZibalVerifyResponse>> VerifyPaymentAsync(
        long trackId,
        CancellationToken cancellationToken = default) =>
        VerifyPaymentAsync(new ZibalVerifyRequest { TrackId = trackId }, cancellationToken);

    /// <inheritdoc />
    public async Task<ZibalResult<ZibalInquiryResponse>> InquiryPaymentAsync(
        long trackId,
        CancellationToken cancellationToken = default)
    {
        var request = new
        {
            merchant = _options.GetEffectiveMerchant(),
            trackId
        };

        try
        {
            using var response = await _httpClient.PostAsJsonAsync(
                "v1/inquiry",
                request,
                JsonOptions,
                cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return ZibalResult<ZibalInquiryResponse>.Failure(
                    (int)response.StatusCode,
                    $"خطای ارتباط در استعلام تراکنش (HTTP {(int)response.StatusCode}).");
            }

            var result = await response.Content.ReadFromJsonAsync<ZibalInquiryResponse>(
                JsonOptions,
                cancellationToken);

            if (result is null)
            {
                return ZibalResult<ZibalInquiryResponse>.Failure(
                    (int)ZibalResultCode.Unknown,
                    "پاسخ استعلام دریافتی نامعتبر است.");
            }

            var isSuccess = result.Result == (int)ZibalResultCode.Success;
            var message = result.ResultCode.ToPersianMessage();

            return isSuccess
                ? ZibalResult<ZibalInquiryResponse>.Success(result, result.Result, message)
                : ZibalResult<ZibalInquiryResponse>.Failure(result.Result, message, result.Message);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            return ZibalResult<ZibalInquiryResponse>.Failure(
                -1,
                "خطا در استعلام وضعیت تراکنش از زیبال.",
                ex.Message);
        }
    }

    /// <inheritdoc />
    public string GetPaymentUrl(long trackId) =>
        $"{_options.BaseUrl.TrimEnd('/')}/start/{trackId}";
}