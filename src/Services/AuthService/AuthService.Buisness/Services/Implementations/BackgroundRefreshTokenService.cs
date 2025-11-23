using AuthService.Buisness.Services.Interfaces;
using AuthService.DataAccess.Persistans.Repositories.Implementations;
using AuthService.DataAccess.Persistans.Repositories.Interfaces;
using Hangfire;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace AuthService.Buisness.Services.Implementations
{
    public class BackgroundRefreshTokenService
        : IBackgroundRefreshTokenService
    {
        private readonly ILogger _logger;
        private readonly RefreshTokenRepository _repository;
        public BackgroundRefreshTokenService(
            ILogger<BackgroundRefreshTokenService> logger,
            IRefreshTokenRepository repository)
        {
            _logger = logger;
            _repository = (RefreshTokenRepository)repository;
        }

        public void DeleteExpiredTokens()
        {
            RecurringJob.AddOrUpdate(Guid.NewGuid().ToString(),
                () => _repository.DeleteExpiredTokens(),
                Cron.HourInterval(1));

            _logger.LogInformation($"DeleteExpiredTokensAsync had been added to reccuring jobs");
        }

        public void DeleteUnusedRefreshTokens()
        {
            RecurringJob.AddOrUpdate(Guid.NewGuid().ToString(),
                () => _repository.DeleteUnusedRefreshTokens(),
                Cron.HourInterval(1));

            _logger.LogInformation($"DeleteUnusedRefreshTokensAsync had been added to reccuring jobs");
        }

        public void RefreshToken(string token)
        {
            throw new NotImplementedException();
        }

        public void AddNewRecurringJob(Expression<Action> action, string cronExpression)
        {
            RecurringJob.AddOrUpdate(Guid.NewGuid().ToString(), action, cronExpression);

            _logger.LogInformation($"New action had been added to reccuring jobs");
        }
    }
}
