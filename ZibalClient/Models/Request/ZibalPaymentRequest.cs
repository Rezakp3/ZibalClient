using System.Text.Json.Serialization;
using ZibalClient.Enums;
using ZibalClient.Models.Common;

namespace ZibalClient.Models.Request;

/// <summary>
/// پارامترهای درخواست ایجاد تراکنش و دریافت توکن پرداخت
/// </summary>
public record ZibalPaymentRequest
{
    /// <summary>
    /// مرچنت کد پذیرنده (در صورت خالی بودن، مقدار پیش‌فرض پیکربندی استفاده می‌شود)
    /// </summary>
    [JsonPropertyName("merchant")]
    public string? Merchant { get; set; }

    /// <summary>
    /// مبلغ کل سفارش به «ریال» (حداقل ۱,۰۰۰ ریال)
    /// </summary>
    [JsonPropertyName("amount")]
    public required long Amount { get; init; }

    /// <summary>
    /// آدرس بازگشت به سایت پذیرنده پس از پرداخت
    /// </summary>
    [JsonPropertyName("callbackUrl")]
    public required string CallbackUrl { get; init; }

    /// <summary>
    /// توضیحات مربوط به سفارش و خرید
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }

    /// <summary>
    /// شماره سفارش اختصاصی در سیستم شما (جهت گزارش‌گیری و رهگیری)
    /// </summary>
    [JsonPropertyName("orderId")]
    public string? OrderId { get; init; }

    /// <summary>
    /// شماره موبایل مشتری (جهت نمایش کارت‌های ذخیره‌شده و سهولت پرداخت در درگاه)
    /// </summary>
    [JsonPropertyName("mobile")]
    public string? Mobile { get; init; }

    /// <summary>
    /// لیست شماره کارت‌های ۱۶ رقمی مجاز (کاربر فقط با این کارت‌ها امکان پرداخت خواهد داشت)
    /// </summary>
    [JsonPropertyName("allowedCards")]
    public IReadOnlyList<string>? AllowedCards { get; init; }

    /// <summary>
    /// کد ملی ۱۰ رقمی خریدار (در صورت ارسال، باید با کد ملی صاحب کارت بانکی تطابق داشته باشد)
    /// </summary>
    [JsonPropertyName("nationalCode")]
    public string? NationalCode { get; init; }

    /// <summary>
    /// بررسی و تطبیق الزامی شماره موبایل با صاحب کارت بانکی
    /// </summary>
    [JsonPropertyName("checkMobileWithCard")]
    public bool? CheckMobileWithCard { get; init; }

    /// <summary>
    /// آیا نحوه تسهیم به صورت درصدی است؟ (پیش‌فرض: خیر - مبلغی)
    /// </summary>
    [JsonPropertyName("percentMode")]
    public int? PercentMode { get; init; }

    /// <summary>
    /// نحوه کسر کارمزد درگاه (از مبلغ تراکنش یا کیف پول)
    /// </summary>
    [JsonPropertyName("feeMode")]
    public ZibalFeeMode? FeeMode { get; init; }

    /// <summary>
    /// لیست اطلاعات ذی‌نفعان جهت تسهیم وجوه
    /// </summary>
    [JsonPropertyName("multiplexingInfos")]
    public IReadOnlyList<MultiplexingInfo>? MultiplexingInfos { get; init; }
}