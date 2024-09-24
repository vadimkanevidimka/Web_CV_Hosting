using Microsoft.AspNetCore.Identity;

namespace AuthService.DataAccess.Entities;

public class User : IdentityUser
{
    public virtual IEnumerable<RefreshToken> RefreshTokens { get; set; }
}