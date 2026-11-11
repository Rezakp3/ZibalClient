
---

```markdown
# ZibalClient 💳

[![.NET](https://img.shields.io/badge/.NET-8.0%20%7C%209.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg)](https://github.com/your-username/ZibalClient/pulls)

کلاینت مدرن، سبک، غیرهمگام (Async) و با کارایی بالا برای اتصال به درگاه پرداخت اینترنتی **زیبال (Zibal IPG)** در اکوسیستم دات‌نت (.NET 8 و بالاتر).

---

## 🚀 ویژگی‌های کلیدی

- **بدون وابستگی اضافه (Zero Third-Party Dependencies):** توسعه‌یافته صرفاً با ابزارهای استاندارد مایکروسافت و سریالایزر پرسرعت `System.Text.Json`.
- **مبتنی بر استانداردهای مدرن دات‌نت:** استفاده از `IHttpClientFactory` جهت جلوگیری از مشکل Socket Exhaustion و مصرف بهینه حافظه با `record`.
- **الگوی نتیجه استاندارد (Result Pattern):** عدم پرتاب Exception برای خطاهای بیزینسی، خطای شبکه یا پاسخ‌های ناموفق بانکی.
- **تایپ ایمن و پیام‌های فارسی:** تبدیل خودکار تمام کدهای وضعیت و نتیجه زیبال به پیام‌های فارسی خوانا و کاربرپسند.
- **پشتیبانی کامل از امکانات پیشرفته:** تطبیق شماره موبایل و کد ملی (`nationalCode`)، محدودسازی شماره کارت (`allowedCards`) و تسهیم وجوه (`multiplexingInfos`).
- **تست آسان با سندباکس:** قابلیت سوییچ به محیط تستی زیبال تنها با یک فلگ بولی (`UseSandbox: true`).

---

## 📦 نصب و راه‌اندازی

از طریق .NET CLI:

```bash
dotnet add package ZibalClient
```

یا از طریق Package Manager Console در ویژوال استودیو:

```powershell
Install-Package ZibalClient
```

---

## ⚙️ پیکربندی (Configuration)

### ۱. تنظیم در `appsettings.json`

```json
{
  "Zibal": {
    "Merchant": "YOUR_ZIBAL_MERCHANT_KEY",
    "UseSandbox": false,
    "TimeoutSeconds": 30
  }
}
```

> **نکته:** برای تست رایگان درگاه بدون نیاز به قرارداد، کافی است مقدار `UseSandbox` را `true` بگذارید. در این حالت مرچنت به صورت خودکار برابر `"zibal"` قرار می‌گیرد.

### ۲. ثبت در کانتینر DI (`Program.cs`)

روش اول: خواندن مستقیم از تنظیمات:
```csharp
builder.Services.AddZibalClient(builder.Configuration);
```

روش دوم: تنظیم دستی با Action:
```csharp
builder.Services.AddZibalClient(options =>
{
    options.Merchant = "YOUR_MERCHANT_KEY";
    options.UseSandbox = builder.Environment.IsDevelopment();
    options.TimeoutSeconds = 30;
});
```

---

## 💻 راهنمای استفاده (کد نمونه)

### ۱. درخواست پرداخت (Request Payment)

```csharp
public class PaymentController(IZibalClient zibalClient) : ControllerBase
{
    [HttpPost("pay")]
    public async Task<IActionResult> Pay(long orderId, CancellationToken ct)
    {
        var request = new ZibalPaymentRequest
        {
            Amount = 100000, // مبلغ به ریال (۱۰,۰۰۰ تومان)
            CallbackUrl = "https://yoursite.com/api/payment/callback",
            Description = $"پرداخت سفارش شماره {orderId}",
            OrderId = orderId.ToString(),
            Mobile = "09123456789" // اختیاری: جهت سهولت انتخاب کارت مشتری
        };

        var result = await zibalClient.RequestPaymentAsync(request, ct);

        if (!result.IsSuccess)
        {
            // پیام خطای فارسی در دسترس است
            return BadRequest(new { error = result.Message });
        }

        // دریافت آدرس درگاه و هدایت کاربر به بانک
        var paymentUrl = result.Data!.GetPaymentUrl();
        return Redirect(paymentUrl);
    }
}
```

---

### ۲. پردازش بازگشت از بانک و تایید پرداخت (Callback & Verify)

پس از پرداخت، زیبال کاربر را به `CallbackUrl` مشخص‌شده هدایت می‌کند.

```csharp
[HttpGet("callback")]
public async Task<IActionResult> Callback(CancellationToken ct)
{
    // پارس کردن هوشمند اطلاعات Query String با مدل آماده لایبرری
    var callback = ZibalCallback.FromQueryString(Request.QueryString.Value!);

    // بررسی وضعیت اولیه درگاه
    if (!callback.CanBeVerified)
    {
        return BadRequest($"پرداخت ناموفق بود: {callback.PaymentStatusMessage}");
    }

    // تایید نهایی تراکنش (Verify)
    var verifyResult = await zibalClient.VerifyPaymentAsync(callback.TrackId, ct);

    if (verifyResult.IsSuccess)
    {
        var data = verifyResult.Data!;
        
        // توجه: کد ۲۰۱ یعنی قبلاً تایید شده است (برای جلوگیری از دوبار شارژ شدن)
        if (data.IsAlreadyVerified)
        {
            // لاگ یا گزارش تراکنش تکراری
        }

        return Ok(new
        {
            Message = "پرداخت با موفقیت انجام و تایید شد.",
            RefNumber = data.RefNumber,   // شماره پیگیری شاپرک
            CardNumber = data.CardNumber, // شماره کارت ماسک‌شده خریدار
            Amount = data.Amount          // مبلغ واریزی به ریال
        });
    }

    return BadRequest(new { error = verifyResult.Message });
}
```

---

### ۳. استعلام تراکنش (Inquiry)

در صورتی که وضعیت تراکنش به هر دلیلی نامشخص باشد (مانند قطعی اینترنت کاربر):

```csharp
var inquiryResult = await zibalClient.InquiryPaymentAsync(trackId, cancellationToken);

if (inquiryResult.IsSuccess)
{
    var report = inquiryResult.Data!;
    Console.WriteLine($"وضعیت: {report.PaymentStatusMessage}");
    Console.WriteLine($"تاریخ پرداخت: {report.PaidAt}");
    Console.WriteLine($"کارمزد: {report.Wage} ریال");
}
```

---

## 🔒 ویژگی‌های امنیتی و پیشرفته

### محدودسازی کارت و اعتبارسنجی کد ملی (شاهکار):
```csharp
var request = new ZibalPaymentRequest
{
    Amount = 5000000,
    CallbackUrl = "https://yoursite.com/callback",
    NationalCode = "0012345678",        // تطبیق کد ملی با کارت پرداخت‌کننده
    CheckMobileWithCard = true,         // تطبیق شماره موبایل با صاحب کارت
    AllowedCards = ["603799******1234"] // تنها این کارت مجاز به پرداخت است
};
```

---

## 🤝 مشارکت (Contributing)

از گزارش باگ‌ها، پیشنهادات و Pull Requestها استقبال می‌شود. اگر قابلیتی مدنظر دارید:
1. پروژه را Fork کنید.
2. یک Branch جدید بسازید (`git checkout -b feature/AmazingFeature`).
3. تغییرات خود را Commit کنید (`git commit -m 'Add some AmazingFeature'`).
4. به Branch اصلی Push کنید (`git push origin feature/AmazingFeature`).
5. یک Pull Request باز کنید.

---

## 📄 مجوز (License)

این پروژه تحت مجوز [MIT](LICENSE) منتشر شده است و استفاده از آن در پروژه‌های شخصی و تجاری کاملاً آزاد و رایگان است.
```

---

این فایل تمام سناریوها، نمونه کدهای تمیز و نکات پیشرفته را پوشش داده است. 

حالا می‌خواهید به پروژه اصلی خودتان برگردیم و کلاس آداپتور (`ZibalPaymentGatewayService`) را پیاده‌سازی کنیم تا سیستم فعلی‌تان مستقیماً از این کلاینت استفاده کند؟