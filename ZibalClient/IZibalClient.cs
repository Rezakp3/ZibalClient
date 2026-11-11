using ZibalClient.Common;
using ZibalClient.Models.Inquiry;
using ZibalClient.Models.Request;
using ZibalClient.Models.Verify;

namespace ZibalClient;

/// <summary>
/// کلاینت اصلی ارتباط با وب‌سرویس‌های درگاه پرداخت زیبال
/// </summary>
public interface IZibalClient
{
    /// <summary>
    /// ایجاد درخواست تراکنش جدید و دریافت شناسه پیگیری (TrackId)
    /// </summary>
    /// <param name="request">پارامترهای درخواست پرداخت</param>
    /// <param name="cancellationToken">توکن لغو عملیات</param>
    /// <returns>نتیجه عملیات شامل TrackId و لینک هدایت به درگاه</returns>
    Task<ZibalResult<ZibalPaymentResponse>> RequestPaymentAsync(
        ZibalPaymentRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// تایید نهایی تراکنش پس از بازگشت کاربر از صفحه بانک (Verify)
    /// </summary>
    /// <param name="request">پارامترهای تایید تراکنش</param>
    /// <param name="cancellationToken">توکن لغو عملیات</param>
    /// <returns>نتیجه تایید و اطلاعات شماره کارت و مرجع شاپرک</returns>
    Task<ZibalResult<ZibalVerifyResponse>> VerifyPaymentAsync(
        ZibalVerifyRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// متد کمکی تایید تراکنش تنها با ارسال شناسه پیگیری
    /// </summary>
    /// <param name="trackId">شناسه پیگیری تراکنش صادر شده توسط زیبال</param>
    /// <param name="cancellationToken">توکن لغو عملیات</param>
    Task<ZibalResult<ZibalVerifyResponse>> VerifyPaymentAsync(
        long trackId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// استعلام وضعیت کامل یک جلسه پرداخت (Inquiry)
    /// </summary>
    /// <param name="trackId">شناسه پیگیری تراکنش</param>
    /// <param name="cancellationToken">توکن لغو عملیات</param>
    /// <returns>گزارش کامل وضعیت، زمان‌های تراکنش، کارمزد و اطلاعات مالی</returns>
    Task<ZibalResult<ZibalInquiryResponse>> InquiryPaymentAsync(
        long trackId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// ساخت آدرس کامل هدایت کاربر به درگاه بانکی
    /// </summary>
    /// <param name="trackId">شناسه پیگیری تراکنش</param>
    /// <returns>آدرس URL شروع پرداخت</returns>
    string GetPaymentUrl(long trackId);
}