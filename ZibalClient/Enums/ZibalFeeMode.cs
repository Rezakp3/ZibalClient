namespace ZibalClient.Enums;

/// <summary>
/// شیوه کسر کارمزد تراکنش در زیبال
/// </summary>
public enum ZibalFeeMode
{
    /// <summary>
    /// کسر کارمزد مستقیماً از مبلغ تراکنش
    /// (پیش‌فرض زیبال - عدد 0)
    /// </summary>
    DeductFromTransaction = 0,

    /// <summary>
    /// کسر کارمزد از موجودی کیف پول متصل به درگاه
    /// (در سرویس‌های پرداختیاری پشتیبانی نمی‌شود - عدد 1)
    /// </summary>
    DeductFromWallet = 1
}