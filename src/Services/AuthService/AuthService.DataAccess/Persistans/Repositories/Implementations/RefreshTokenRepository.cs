using AuthService.DataAccess.Entities;
using AuthService.DataAccess.Persistans.DbContext;
using AuthService.DataAccess.Persistans.Repositories.Interfaces;

namespace AuthService.DataAccess.Persistans.Repositories.Implementations;

public class RefreshTokenRepository : RepositoryBase<RefreshToken>, IRefreshTokenRepository
{
    public RefreshTokenRepository(AuthorizationDbContext context) : base(context)
    {
    }
}