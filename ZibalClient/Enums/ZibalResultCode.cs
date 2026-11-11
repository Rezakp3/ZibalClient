namespace ZibalClient.Enums;

/// <summary>
/// کدهای نتیجه پاسخ وب‌سرویس زیبال به درخواست‌های سروری
/// </summary>
public enum ZibalResultCode
{
    /// <summary>
    /// درخواست با موفقیت انجام شد
    /// </summary>
    Success = 100,

    /// <summary>
    /// در حال پردازش / درخواست هم‌زمان دیگر در جریان است
    /// </summary>
    Processing = -1,

    /// <summary>
    /// مرچنت در سیستم زیبال یافت نشد
    /// </summary>
    MerchantNotFound = 102,

    /// <summary>
    /// مرچنت غیرفعال است یا قرارداد درگاه امضا نشده است
    /// </summary>
    MerchantInactive = 103,

    /// <summary>
    /// مرچنت ارسالی نامعتبر است
    /// </summary>
    MerchantInvalid = 104,

    /// <summary>
    /// مبلغ باید حداقل ۱,۰۰۰ ریال (۱۰۰ تومان) باشد
    /// </summary>
    AmountTooLow = 105,

    /// <summary>
    /// آدرس بازگشت (CallbackUrl) نامعتبر است (باید با http یا https شروع شود و با دامنه درگاه هم‌خوانی داشته باشد)
    /// </summary>
    InvalidCallbackUrl = 106,

    /// <summary>
    /// نحوه تسهیم نامعتبر است (تنها مقادیر 0 و 1 معتبر هستند)
    /// </summary>
    InvalidPercentMode = 107,

    /// <summary>
    /// یک یا چند ذی‌نفع در اطلاعات تسهیم نامعتبر هستند
    /// </summary>
    InvalidMultiplexingBeneficiary = 108,

    /// <summary>
    /// یک یا چند ذی‌نفع در اطلاعات تسهیم غیرفعال هستند
    /// </summary>
    InactiveMultiplexingBeneficiary = 109,

    /// <summary>
    /// پرداخت‌کننده کارمزد در تسهیم مشخص نشده است
    /// </summary>
    WagePayerNotSpecified = 110,

    /// <summary>
    /// مبلغ کل با مجموع مبالغ سهم‌ها در اطلاعات تسهیم برابر نیست
    /// </summary>
    MultiplexingAmountMismatch = 111,

    /// <summary>
    /// موجودی کیف پول جهت کسر کارمزد کافی نیست
    /// </summary>
    InsufficientWalletBalanceForFee = 112,

    /// <summary>
    /// مبلغ تراکنش از سقف مجاز بیشتر است (سقف کل درگاه یا شاپرک)
    /// </summary>
    AmountExceedsLimit = 113,

    /// <summary>
    /// کد ملی ارسالی نامعتبر است (باید ۱۰ رقم استاندارد باشد)
    /// </summary>
    InvalidNationalCode = 114,

    /// <summary>
    /// آدرس IP سرور درخواست‌دهنده در پنل کاربری زیبال ثبت نشده است
    /// </summary>
    IpNotAllowed = 115,

    /// <summary>
    /// نحوه کسر کارمزد نامعتبر است
    /// </summary>
    InvalidFeeMode = 116,

    /// <summary>
    /// ساختار اطلاعات تسهیم نامعتبر است
    /// </summary>
    InvalidMultiplexingStructure = 118,

    /// <summary>
    /// آدرس بازگشت (CallbackUrl) ارسال نشده است
    /// </summary>
    CallbackUrlNotProvided = 140,

    /// <summary>
    /// سهم ذی‌نفع کسرکننده کارمزد از مبلغ خود کارمزد کمتر است
    /// </summary>
    WagePayerShareLessThanFee = 141,

    /// <summary>
    /// تراکنش قبلاً با موفقیت تایید (Verify) شده است
    /// </summary>
    AlreadyVerified = 201,

    /// <summary>
    /// سفارش پرداخت نشده یا تراکنش ناموفق بوده است
    /// </summary>
    PaymentFailedOrNotPaid = 202,

    /// <summary>
    /// شناسه پیگیری (trackId) در زیبال یافت نشد یا نامعتبر است
    /// </summary>
    TrackIdNotFound = 203,

    /// <summary>
    /// خطای ناشناخته یا تعریف نشده
    /// </summary>
    Unknown = 999
}