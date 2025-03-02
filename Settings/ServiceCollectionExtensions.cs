namespace AuthenticationExample.Settings;

internal static class ServiceCollectionExtensions
{
    public static void AddWebSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<JwtSettings>()
            .Bind(configuration.GetSection(JwtSettings.Key))
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }
}