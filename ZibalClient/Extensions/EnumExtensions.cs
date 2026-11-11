using ZibalClient.Enums;

namespace ZibalClient.Extensions;

/// <summary>
/// متدهای توسعه جهت دریافت پیام فارسی و توضیحات وضعیت‌های درگاه زیبال
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// تبدیل کد نتیجه دریافتی از درگاه به Enum متناظر
    /// </summary>
    /// <param name="code">کد عددی نتیجه</param>
    /// <returns>مقدار Enum متناظر یا Unknown</returns>
    public static ZibalResultCode ToResultCode(this int code) =>
        Enum.IsDefined(typeof(ZibalResultCode), code)
            ? (ZibalResultCode)code
            : ZibalResultCode.Unknown;

    /// <summary>
    /// دریافت پیام و متن خطای فارسی متناظر با نتیجه وب‌سرویس زیبال
    /// </summary>
    /// <param name="resultCode">کد نتیجه پاسخ درگاه</param>
    /// <returns>پیام استاندارد و فارسی برای نمایش</returns>
    public static string ToPersianMessage(this ZibalResultCode resultCode) =>
        resultCode switch
        {
            ZibalResultCode.Success => "عملیات با موفقیت انجام شد.",
            ZibalResultCode.Processing => "درخواست دیگری برای این تراکنش در حال پردازش است.",
            ZibalResultCode.MerchantNotFound => "کد پذیرنده (مرچنت) در سیستم زیبال یافت نشد.",
            ZibalResultCode.MerchantInactive => "درگاه پذیرنده غیرفعال است یا قرارداد آن تایید نشده است.",
            ZibalResultCode.MerchantInvalid => "کد پذیرنده (مرچنت) نامعتبر است.",
            ZibalResultCode.AmountTooLow => "مبلغ تراکنش باید حداقل ۱,۰۰۰ ریال (۱۰۰ تومان) باشد.",
            ZibalResultCode.InvalidCallbackUrl => "آدرس بازگشت (CallbackUrl) نامعتبر است.",
            ZibalResultCode.InvalidPercentMode => "فرمت تسهیم درصدی نامعتبر است.",
            ZibalResultCode.InvalidMultiplexingBeneficiary => "یک یا چند ذی‌نفع در اطلاعات تسهیم نامعتبر هستند.",
            ZibalResultCode.InactiveMultiplexingBeneficiary => "یک یا چند ذی‌نفع در اطلاعات تسهیم غیرفعال هستند.",
            ZibalResultCode.WagePayerNotSpecified => "پرداخت‌کننده کارمزد در تسهیم مشخص نشده است.",
            ZibalResultCode.MultiplexingAmountMismatch => "مجموع سهم‌های تسهیم با مبلغ کل تراکنش برابر نیست.",
            ZibalResultCode.InsufficientWalletBalanceForFee => "موجودی کیف پول برای کسر کارمزد کافی نیست.",
            ZibalResultCode.AmountExceedsLimit => "مبلغ تراکنش بیش از سقف مجاز روزانه یا مجاز درگاه است.",
            ZibalResultCode.InvalidNationalCode => "کد ملی ارسالی نامعتبر است.",
            ZibalResultCode.IpNotAllowed => "آدرس IP سرور شما در پنل کاربری زیبال ثبت نشده است.",
            ZibalResultCode.InvalidFeeMode => "نحوه کسر کارمزد نامعتبر است.",
            ZibalResultCode.InvalidMultiplexingStructure => "ساختار اطلاعات تسهیم نامعتبر است.",
            ZibalResultCode.CallbackUrlNotProvided => "آدرس بازگشت (CallbackUrl) ارسال نشده است.",
            ZibalResultCode.WagePayerShareLessThanFee => "سهم پرداخت‌کننده کارمزد از مبلغ کارمزد کمتر است.",
            ZibalResultCode.AlreadyVerified => "این تراکنش قبلاً با موفقیت تایید شده است.",
            ZibalResultCode.PaymentFailedOrNotPaid => "سفارش پرداخت نشده یا تراکنش در بانک ناموفق بوده است.",
            ZibalResultCode.TrackIdNotFound => "شناسه پیگیری (trackId) نامعتبر است یا یافت نشد.",
            _ => $"خطای ناشناخته در درگاه پرداخت (کد: {(int)resultCode})"
        };

    /// <summary>
    /// تبدیل کد وضعیت پرداختی شاپرک به Enum متناظر
    /// </summary>
    /// <param name="status">کد عددی وضعیت شاپرک</param>
    /// <returns>مقدار Enum متناظر یا Unknown</returns>
    public static ZibalPaymentStatus ToPaymentStatus(this int status) =>
        Enum.IsDefined(typeof(ZibalPaymentStatus), status)
            ? (ZibalPaymentStatus)status
            : ZibalPaymentStatus.Unknown;

    /// <summary>
    /// دریافت توضیحات فارسی وضعیت پرداخت شاپرک
    /// </summary>
    /// <param name="status">کد وضعیت پرداخت کاربر</param>
    /// <returns>پیام قابل فهم و کاربرپسند به زبان فارسی</returns>
    public static string ToPersianMessage(this ZibalPaymentStatus status) =>
        status switch
        {
            ZibalPaymentStatus.WaitingForPayment => "در انتظار پرداخت در درگاه بانکی.",
            ZibalPaymentStatus.InternalError => "خطای داخلی درگاه پرداخت.",
            ZibalPaymentStatus.PaidAndVerified => "پرداخت با موفقیت انجام و تایید شد.",
            ZibalPaymentStatus.PaidNotVerified => "پرداخت انجام شده اما هنوز توسط سایت تایید (Verify) نشده است.",
            ZibalPaymentStatus.CanceledByUser => "پرداخت توسط کاربر لغو شد.",
            ZibalPaymentStatus.InvalidCardNumber => "شماره کارت وارد شده نامعتبر است.",
            ZibalPaymentStatus.InsufficientBalance => "موجودی حساب کافی نمی‌باشد.",
            ZibalPaymentStatus.IncorrectPassword => "رمز کارت اشتباه است.",
            ZibalPaymentStatus.TooManyRequests => "تعداد درخواست‌های پرداخت بیش از حد مجاز است.",
            ZibalPaymentStatus.DailyTransactionCountExceeded => "تعداد تراکنش‌های اینترنتی روزانه این کارت بیش از سقف مجاز است.",
            ZibalPaymentStatus.DailyAmountLimitExceeded => "مبلغ خرید اینترنتی روزانه این کارت بیش از سقف مجاز است.",
            ZibalPaymentStatus.InvalidCardIssuer => "بانک صادرکننده کارت نامعتبر است.",
            ZibalPaymentStatus.SwitchError => "خطای موقت سوییچ شاپرک یا بانک. لطفاً مجدداً تلاش کنید.",
            ZibalPaymentStatus.CardNotAccessible => "کارت بانکی در دسترس یا فعال نمی‌باشد.",
            ZibalPaymentStatus.Refunded => "تراکنش استرداد شده است.",
            ZibalPaymentStatus.Refunding => "تراکنش در حال استرداد است.",
            ZibalPaymentStatus.Reversed => "مبلغ تراکنش به حساب خریدار بازگشت داده شد (ریورس).",
            ZibalPaymentStatus.InvalidAcceptor => "پذیرنده درگاه نامعتبر است.",
            _ => $"وضعیت نامشخص تراکنش (کد: {(int)status})"
        };
}