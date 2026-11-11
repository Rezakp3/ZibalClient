using ZibalClient.Enums;
using ZibalClient.Extensions;

namespace ZibalClient.Models.Callback;

/// <summary>
/// مدل اطلاعات ارسال‌شده توسط زیبال به آدرس بازگشت (CallbackUrl) پس از اتمام پرداخت
/// </summary>
public record ZibalCallback
{
    /// <summary>
    /// شناسه پیگیری یکتای جلسه پرداخت
    /// </summary>
    public long TrackId { get; init; }

    /// <summary>
    /// آیا پرداخت در درگاه با موفقیت اولیه همراه بوده است؟ (معادل 1)
    /// </summary>
    public bool Success { get; init; }

    /// <summary>
    /// کد وضعیت پرداختی شاپرک (مثلاً 2 به معنی پرداخت موفق در انتظار تایید)
    /// </summary>
    public int Status { get; init; }

    /// <summary>
    /// شناسه سفارش اختصاصی ارسال‌شده در زمان درخواست اولیه
    /// </summary>
    public string? OrderId { get; init; }

    /// <summary>
    /// وضعیت شاپرکی در قالب Enum
    /// </summary>
    public ZibalPaymentStatus PaymentStatus => Status.ToPaymentStatus();

    /// <summary>
    /// پیام فارسی شفاف وضعیت پرداخت
    /// </summary>
    public string PaymentStatusMessage => PaymentStatus.ToPersianMessage();

    /// <summary>
    /// آیا تراکنش آماده ارسال به متد وریفای (Verify) است؟
    /// (تراکنش زمانی آماده تایید است که در درگاه موفق بوده و در وضعیت انتظار تایید (Status = 2) باشد)
    /// </summary>
    public bool CanBeVerified => Success && Status == (int)ZibalPaymentStatus.PaidNotVerified;

    // ==================== متدهای پارسر هوشمند (Helper Parsers) ====================

    /// <summary>
    /// پارس کردن خودکار اطلاعات کال‌بک از دیکشنری مقادیر Query String
    /// </summary>
    public static ZibalCallback FromDictionary(IDictionary<string, string> queryParameters)
    {
        queryParameters.TryGetValue("trackId", out var trackIdStr);
        queryParameters.TryGetValue("success", out var successStr);
        queryParameters.TryGetValue("status", out var statusStr);
        queryParameters.TryGetValue("orderId", out var orderId);

        _ = long.TryParse(trackIdStr, out var trackId);
        _ = int.TryParse(statusStr, out var status);
        var success = successStr == "1";

        return new ZibalCallback
        {
            TrackId = trackId,
            Success = success,
            Status = status,
            OrderId = orderId
        };
    }

    /// <summary>
    /// پارس کردن خودکار اطلاعات کال‌بک مستقیماً از رشته QueryString
    /// </summary>
    /// <param name="queryString">رشته کوئری کامل (مثال: "?trackId=123&amp;success=1&amp;status=2")</param>
    public static ZibalCallback FromQueryString(string queryString)
    {
        var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        if (string.IsNullOrWhiteSpace(queryString))
            return FromDictionary(dict);

        var query = queryString.TrimStart('?');
        var pairs = query.Split('&', StringSplitOptions.RemoveEmptyEntries);

        foreach (var pair in pairs)
        {
            var parts = pair.Split('=', 2);
            if (parts.Length == 2)
            {
                dict[Uri.UnescapeDataString(parts[0])] = Uri.UnescapeDataString(parts[1]);
            }
        }

        return FromDictionary(dict);
    }
}