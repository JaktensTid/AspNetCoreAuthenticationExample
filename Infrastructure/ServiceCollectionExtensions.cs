using Microsoft.EntityFrameworkCore;

namespace AuthenticationExample.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEntityFramework(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddScoped<IApplicationContext>(sp => sp.GetRequiredService<ApplicationContext>())
            .AddDbContext<ApplicationContext>(options => options.UseInMemoryDatabase("db"));
    }
}