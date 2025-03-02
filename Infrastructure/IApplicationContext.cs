using AuthenticationExample.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationExample.Infrastructure;

public interface IApplicationContext
{
    public DbSet<User> Users { get; }
    
    public Task<int> SaveChangesAsync(CancellationToken ct = default);
}