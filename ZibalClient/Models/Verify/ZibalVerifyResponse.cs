using System.Text.Json.Serialization;
using ZibalClient.Enums;
using ZibalClient.Extensions;
using ZibalClient.Models.Common;

namespace ZibalClient.Models.Verify;

/// <summary>
/// پاسخ نهایی تایید تراکنش دریافتی از درگاه زیبال
/// </summary>
public record ZibalVerifyResponse
{
    /// <summary>
    /// زمان دقیق پرداخت سفارش
    /// </summary>
    [JsonPropertyName("paidAt")]
    public DateTimeOffset? PaidAt { get; init; }

    /// <summary>
    /// شماره کارت ماسک‌شده پرداخت‌کننده (مثال: 62741****44)
    /// </summary>
    [JsonPropertyName("cardNumber")]
    public string? CardNumber { get; init; }

    /// <summary>
    /// وضعیت نهایی تراکنش در شبکه شاپرک
    /// </summary>
    [JsonPropertyName("status")]
    public int Status { get; init; }

    /// <summary>
    /// مبلغ پرداختی سفارش به «ریال»
    /// </summary>
    [JsonPropertyName("amount")]
    public long Amount { get; init; }

    /// <summary>
    /// شماره پیگیری شاپرکی / شناسه مرجع بانکی (RefNumber)
    /// </summary>
    [JsonPropertyName("refNumber")]
    public long? RefNumber { get; init; }

    /// <summary>
    /// توضیحات ثبت شده برای تراکنش
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }

    /// <summary>
    /// شماره سفارش اختصاصی سایت شما
    /// </summary>
    [JsonPropertyName("orderId")]
    public string? OrderId { get; init; }

    /// <summary>
    /// کد نتیجه عملیات تایید زیبال (100 = تایید موفق، 201 = قبلاً تایید شده)
    /// </summary>
    [JsonPropertyName("result")]
    public int Result { get; init; }

    /// <summary>
    /// پیام دریافتی از زیبال
    /// </summary>
    [JsonPropertyName("message")]
    public string Message { get; init; } = string.Empty;

    /// <summary>
    /// جزئیات تسهیم تراکنش (در صورت وجود)
    /// </summary>
    [JsonPropertyName("multiplexingInfos")]
    public IReadOnlyList<MultiplexingInfo>? MultiplexingInfos { get; init; }

    // ==================== پراپرتی‌های کمکی هوشمند ====================

    /// <summary>
    /// نتیجه عملیات تایید در قالب Enum
    /// </summary>
    [JsonIgnore]
    public ZibalResultCode ResultCode => Result.ToResultCode();

    /// <summary>
    /// وضعیت شاپرکی تراکنش در قالب Enum
    /// </summary>
    [JsonIgnore]
    public ZibalPaymentStatus PaymentStatus => Status.ToPaymentStatus();

    /// <summary>
    /// پیام فارسی شفاف وضعیت تراکنش شاپرک
    /// </summary>
    [JsonIgnore]
    public string PaymentStatusMessage => PaymentStatus.ToPersianMessage();

    /// <summary>
    /// آیا تراکنش قبلاً با موفقیت تایید شده بود؟ (کد ۲۰۱)
    /// </summary>
    [JsonIgnore]
    public bool IsAlreadyVerified => Result == (int)ZibalResultCode.AlreadyVerified;
}