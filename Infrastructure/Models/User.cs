using Microsoft.AspNetCore.Identity;

namespace AuthenticationExample.Infrastructure.Models;

public class User : IdentityUser<Guid>
{
    public User(Guid id, string email)
    {
        Id = id;
        Email = email;
        UserName = email;
    }

    public User()
    {
    }
}