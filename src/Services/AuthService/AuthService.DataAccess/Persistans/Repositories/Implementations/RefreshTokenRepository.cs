using AuthService.DataAccess.Entities;
using AuthService.DataAccess.Persistans.DbContext;
using AuthService.DataAccess.Persistans.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace AuthService.DataAccess.Persistans.Repositories.Implementations;

public class RefreshTokenRepository
    : RepositoryBase<RefreshToken>,
    IRefreshTokenRepository
{
    private readonly ILogger _logger;
    public RefreshTokenRepository(AuthorizationDbContext context, ILogger<RefreshTokenRepository> logger)
        : base(context)
    {
        _logger = logger;
    }

    public void DeleteExpiredTokens()
    {

        _context.RefreshTokens
            .RemoveRange(_context.RefreshTokens
            .Where(token => token.ExpirationTime < DateTime.Now));

        _logger.LogInformation($"All expired tokens has been deleted");
    }

    public void DeleteUnusedRefreshTokens()
    {
        _context.RefreshTokens
           .RemoveRange(_context.RefreshTokens
           .Where(token => token.IsActive == false));

        _logger.LogInformation($"All inactive tokens has been deleted");
    }
}