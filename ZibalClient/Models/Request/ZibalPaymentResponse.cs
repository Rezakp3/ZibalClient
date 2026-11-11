using System.Text.Json.Serialization;
using ZibalClient.Enums;
using ZibalClient.Extensions;

namespace ZibalClient.Models.Request;

/// <summary>
/// پاسخ خام دریافتی از زیبال برای درخواست شروع پرداخت
/// </summary>
public record ZibalPaymentResponse
{
    /// <summary>
    /// شناسه یکتای پیگیری پرداخت در زیبال (TrackId)
    /// </summary>
    [JsonPropertyName("trackId")]
    public long TrackId { get; init; }

    /// <summary>
    /// کد نتیجه عملیات زیبال (100 = موفق)
    /// </summary>
    [JsonPropertyName("result")]
    public int Result { get; init; }

    /// <summary>
    /// پیام دریافتی از زیبال
    /// </summary>
    [JsonPropertyName("message")]
    public string Message { get; init; } = string.Empty;

    /// <summary>
    /// کد نتیجه در قالب Enum
    /// </summary>
    [JsonIgnore]
    public ZibalResultCode ResultCode => Result.ToResultCode();

    /// <summary>
    /// پیام فارسی شفاف متناسب با نتیجه
    /// </summary>
    [JsonIgnore]
    public string PersianMessage => ResultCode.ToPersianMessage();

    /// <summary>
    /// تولید آدرس کامل جهت هدایت کاربر به صفحه پرداخت درگاه زیبال
    /// </summary>
    /// <param name="gatewayBaseUrl">آدرس پایه درگاه (پیش‌فرض زیبال)</param>
    /// <returns>لینک کامل ریدایرکت</returns>
    public string GetPaymentUrl(string gatewayBaseUrl = "https://gateway.zibal.ir") =>
        $"{gatewayBaseUrl.TrimEnd('/')}/start/{TrackId}";
}