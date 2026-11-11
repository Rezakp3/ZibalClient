using System.Text.Json.Serialization;

namespace ZibalClient.Models.Verify;

/// <summary>
/// مدل درخواست تایید تراکنش به سرور زیبال
/// </summary>
public record ZibalVerifyRequest
{
    /// <summary>
    /// مرچنت کد پذیرنده
    /// </summary>
    [JsonPropertyName("merchant")]
    public string? Merchant { get; set; }

    /// <summary>
    /// شناسه پیگیری صادر شده توسط زیبال در مرحله ایجاد تراکنش
    /// </summary>
    [JsonPropertyName("trackId")]
    public required long TrackId { get; init; }
}