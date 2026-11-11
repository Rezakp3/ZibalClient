using System.Text.Json.Serialization;
using ZibalClient.Enums;
using ZibalClient.Extensions;
using ZibalClient.Models.Common;

namespace ZibalClient.Models.Inquiry;

/// <summary>
/// گزارش وضعیت کامل تراکنش دریافتی از متد استعلام
/// </summary>
public record ZibalInquiryResponse
{
    /// <summary>زمان ایجاد جلسه پرداخت</summary>
    [JsonPropertyName("createdAt")]
    public DateTimeOffset? CreatedAt { get; init; }

    /// <summary>زمان انجام پرداخت توسط کاربر</summary>
    [JsonPropertyName("paidAt")]
    public DateTimeOffset? PaidAt { get; init; }

    /// <summary>زمان تایید نهایی تراکنش</summary>
    [JsonPropertyName("verifiedAt")]
    public DateTimeOffset? VerifiedAt { get; init; }

    /// <summary>شماره کارت ماسک شده خریدار</summary>
    [JsonPropertyName("cardNumber")]
    public string? CardNumber { get; init; }

    /// <summary>کد وضعیت تراکنش شاپرک</summary>
    [JsonPropertyName("status")]
    public int Status { get; init; }

    /// <summary>مبلغ سفارش به ریال</summary>
    [JsonPropertyName("amount")]
    public long Amount { get; init; }

    /// <summary>شناسه مرجع تراکنش شاپرک</summary>
    [JsonPropertyName("refNumber")]
    public long? RefNumber { get; init; }

    /// <summary>توضیحات تراکنش</summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }

    /// <summary>شناسه سفارش اختصاصی سایت</summary>
    [JsonPropertyName("orderId")]
    public string? OrderId { get; init; }

    /// <summary>مبلغ کارمزد کسر شده به ریال</summary>
    [JsonPropertyName("wage")]
    public long Wage { get; init; }

    /// <summary>کد نتیجه استعلام</summary>
    [JsonPropertyName("result")]
    public int Result { get; init; }

    /// <summary>پیام دریافتی از وب‌سرویس</summary>
    [JsonPropertyName("message")]
    public string Message { get; init; } = string.Empty;

    /// <summary>اطلاعات تسهیم</summary>
    [JsonPropertyName("multiplexingInfos")]
    public IReadOnlyList<MultiplexingInfo>? MultiplexingInfos { get; init; }

    [JsonIgnore]
    public ZibalResultCode ResultCode => Result.ToResultCode();

    [JsonIgnore]
    public ZibalPaymentStatus PaymentStatus => Status.ToPaymentStatus();

    [JsonIgnore]
    public string PaymentStatusMessage => PaymentStatus.ToPersianMessage();
}