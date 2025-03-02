using AuthenticationExample.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationExample.Infrastructure;

public class ApplicationContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>, IApplicationContext
{
    public const string DefaultScheme = "authentication_example";
    
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)

    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ArgumentNullException.ThrowIfNull(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return await base.SaveChangesAsync(ct);
    }
}