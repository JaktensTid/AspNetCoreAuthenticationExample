using AuthenticationExample.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthenticationExample.Web.Auth;

public static class ServiceCollectionExtensions
{
    public static void AddAuthenticationServices(this IServiceCollection services)
    {
        services.AddSingleton<IEmailSender<User>, CustomNoOpEmailSender>();
        services.AddScoped<PasswordHasher<User>>();
        services.AddScoped<TokenService>();
    }
}