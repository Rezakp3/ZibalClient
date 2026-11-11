namespace ZibalClient.Enums;

/// <summary>
/// وضعیت پرداخت تراکنش در شبکه شاپرک و درگاه بانکی
/// </summary>
public enum ZibalPaymentStatus
{
    /// <summary>
    /// در انتظار پرداخت توسط کاربر در درگاه
    /// </summary>
    WaitingForPayment = -1,

    /// <summary>
    /// خطای داخلی درگاه شاپرک یا بانک
    /// </summary>
    InternalError = -2,

    /// <summary>
    /// پرداخت شده و با موفقیت تایید نهایی شده است
    /// </summary>
    PaidAndVerified = 1,

    /// <summary>
    /// پرداخت با موفقیت انجام شده اما هنوز تایید (Verify) نشده است
    /// </summary>
    PaidNotVerified = 2,

    /// <summary>
    /// فرایند پرداخت توسط کاربر در درگاه لغو شد
    /// </summary>
    CanceledByUser = 3,

    /// <summary>
    /// شماره کارت وارد شده نامعتبر است
    /// </summary>
    InvalidCardNumber = 4,

    /// <summary>
    /// موجودی حساب کاربر کافی نیست
    /// </summary>
    InsufficientBalance = 5,

    /// <summary>
    /// رمز وارد شده (رمز دوم / پویا) اشتباه است
    /// </summary>
    IncorrectPassword = 6,

    /// <summary>
    /// تعداد درخواست‌های پرداخت بیش از حد مجاز است
    /// </summary>
    TooManyRequests = 7,

    /// <summary>
    /// تعداد تراکنش‌های روزانه اینترنتی بیش از حد مجاز است
    /// </summary>
    DailyTransactionCountExceeded = 8,

    /// <summary>
    /// مبلغ پرداخت اینترنتی روزانه کاربر بیش از سقف مجاز است
    /// </summary>
    DailyAmountLimitExceeded = 9,

    /// <summary>
    /// صادرکننده کارت نامعتبر است
    /// </summary>
    InvalidCardIssuer = 10,

    /// <summary>
    /// خطای سوییچ شاپرک یا بانک صادرکننده
    /// </summary>
    SwitchError = 11,

    /// <summary>
    /// کارت بانکی کاربر در دسترس یا فعال نیست
    /// </summary>
    CardNotAccessible = 12,

    /// <summary>
    /// مبلغ تراکنش به مشتری استرداد شده است
    /// </summary>
    Refunded = 15,

    /// <summary>
    /// تراکنش در فرایند استرداد وجه قرار دارد
    /// </summary>
    Refunding = 16,

    /// <summary>
    /// تراکنش ریورس (برگشت داده شده به حساب خریدار) شد
    /// </summary>
    Reversed = 18,

    /// <summary>
    /// پذیرنده درگاه نامعتبر است
    /// </summary>
    InvalidAcceptor = 21,

    /// <summary>
    /// وضعیت نامشخص
    /// </summary>
    Unknown = 999
}
