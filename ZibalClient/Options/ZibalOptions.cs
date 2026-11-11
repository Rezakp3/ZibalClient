namespace ZibalClient.Options;

/// <summary>
/// تنظیمات اختصاصی اتصال به درگاه پرداخت اینترنتی زیبال
/// </summary>
public class ZibalOptions
{
    /// <summary>
    /// نام بخش تنظیمات در فایل appsettings.json
    /// </summary>
    public const string SectionName = "Zibal";

    /// <summary>
    /// کد پذیرنده درگاه (مرچنت).
    /// در صورت فعال بودن UseSandbox، مقدار تستی 'zibal' جایگزین خواهد شد.
    /// </summary>
    public string Merchant { get; set; } = string.Empty;

    /// <summary>
    /// آدرس سرور اصلی زیبال
    /// (پیش‌فرض: https://gateway.zibal.ir)
    /// </summary>
    public string BaseUrl { get; set; } = "https://gateway.zibal.ir";

    /// <summary>
    /// فعال‌سازی حالت سندباکس و آزمایشی
    /// (در این حالت مرچنت به صورت خودکار برابر 'zibal' قرار می‌گیرد)
    /// </summary>
    public bool UseSandbox { get; set; }

    /// <summary>
    /// مهلت زمانی درخواست شبکه به ثانیه (پیش‌فرض: ۳۰ ثانیه)
    /// </summary>
    public int TimeoutSeconds { get; set; } = 30;

    /// <summary>
    /// دریافت مرچنت معتبر با در نظر گرفتن وضعیت سندباکس
    /// </summary>
    public string GetEffectiveMerchant() => UseSandbox ? "zibal" : Merchant;
}