using System.Text.Json.Serialization;

namespace ZibalClient.Models.Common;

/// <summary>
/// اطلاعات تسهیم تراکنش میان چند حساب یا ذی‌نفع
/// </summary>
public record MultiplexingInfo
{
    /// <summary>
    /// شماره شبای بانکی ذی‌نفع (با پیشوند IR)
    /// </summary>
    [JsonPropertyName("bankAccount")]
    public string? BankAccount { get; init; }

    /// <summary>
    /// شناسه ذی‌نفع تعریف شده در پنل زیبال (یا مقدار 'self' برای پذیرنده اصلی)
    /// </summary>
    [JsonPropertyName("subMerchantId")]
    public string? SubMerchantId { get; init; }

    /// <summary>
    /// مبلغ سهم به ریال (یا مقدار درصد در صورت فعال بودن percentMode)
    /// </summary>
    [JsonPropertyName("amount")]
    public long Amount { get; init; }

    /// <summary>
    /// آیا کارمزد تراکنش از سهم این ذی‌نفع کسر شود؟
    /// (فقط در حالتی معتبر است که feeMode برابر 0 باشد)
    /// </summary>
    [JsonPropertyName("wagePayer")]
    public bool? WagePayer { get; init; }
}