namespace ZibalClient.Common;

/// <summary>
/// ساختار یکپارچه و استاندارد برای نتیجه تمامی عملیات‌های درگاه پرداخت زیبال
/// </summary>
public record ZibalResult
{
    /// <summary>
    /// آیا عملیات با موفقیت انجام شد؟
    /// (معادل result == 100 یا در تایید مجدد 201)
    /// </summary>
    public bool IsSuccess { get; init; }

    /// <summary>
    /// کد نتیجه عددی دریافت شده از زیبال یا سیستم
    /// </summary>
    public int ResultCode { get; init; }

    /// <summary>
    /// پیام توضیحی (به زبان فارسی و قابل فهم برای کاربر یا سیستم)
    /// </summary>
    public string Message { get; init; } = string.Empty;

    /// <summary>
    /// پیام خام یا جزئیات خطای دریافتی از درگاه
    /// </summary>
    public string? RawMessage { get; init; }

    // ==================== متدهای سازنده کمکی (Factory Methods) ====================

    public static ZibalResult Success(int resultCode = 100, string message = "عملیات با موفقیت انجام شد.") =>
        new()
        {
            IsSuccess = true,
            ResultCode = resultCode,
            Message = message
        };

    public static ZibalResult Failure(int resultCode, string message, string? rawMessage = null) =>
        new()
        {
            IsSuccess = false,
            ResultCode = resultCode,
            Message = message,
            RawMessage = rawMessage
        };
}

/// <summary>
/// ساختار جنریک برای متدهایی که داده خروجی (Payload) به همراه دارند
/// </summary>
/// <typeparam name="TData">نوع داده خروجی پاسخ درگاه</typeparam>
public record ZibalResult<TData> : ZibalResult
{
    /// <summary>
    /// دیتای برگشتی از وب‌سرویس در صورت موفقیت
    /// </summary>
    public TData? Data { get; init; }

    // ==================== متدهای سازنده کمکی (Factory Methods) ====================

    public static ZibalResult<TData> Success(TData data, int resultCode = 100, string message = "عملیات با موفقیت انجام شد.") =>
        new()
        {
            IsSuccess = true,
            ResultCode = resultCode,
            Message = message,
            Data = data
        };

    public new static ZibalResult<TData> Failure(int resultCode, string message, string? rawMessage = null) =>
        new()
        {
            IsSuccess = false,
            ResultCode = resultCode,
            Message = message,
            RawMessage = rawMessage,
            Data = default
        };
}