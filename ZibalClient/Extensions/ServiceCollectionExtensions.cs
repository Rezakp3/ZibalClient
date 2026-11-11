using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ZibalClient.Options;

namespace ZibalClient.Extensions;

/// <summary>
/// متدهای الحاقی جهت ثبت سرویس درگاه زیبال در کانتینر DI دات‌نت
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// ثبت کلاینت زیبال با استفاده از Action تنظیمات
    /// </summary>
    public static IServiceCollection AddZibalClient(
        this IServiceCollection services,
        Action<ZibalOptions> configureOptions)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configureOptions);

        services.Configure(configureOptions);
        RegisterHttpClient(services);

        return services;
    }

    /// <summary>
    /// ثبت کلاینت زیبال با خواندن مستقیم از بخش تنظیمات IConfiguration (مثل appsettings.json)
    /// </summary>
    public static IServiceCollection AddZibalClient(
        this IServiceCollection services,
        IConfiguration configuration,
        string sectionName = ZibalOptions.SectionName)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        var section = configuration.GetSection(sectionName);
        services.Configure<ZibalOptions>(section);
        RegisterHttpClient(services);

        return services;
    }

    private static void RegisterHttpClient(IServiceCollection services)
    {
        services.AddHttpClient<IZibalClient, ZibalClient>((serviceProvider, httpClient) =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<ZibalOptions>>().Value;

            httpClient.BaseAddress = new Uri(options.BaseUrl.TrimEnd('/') + "/");
            httpClient.Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        });
    }
}