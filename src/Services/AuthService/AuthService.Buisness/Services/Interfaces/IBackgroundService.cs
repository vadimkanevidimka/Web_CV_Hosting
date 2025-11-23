namespace AuthService.Buisness.Services.Interfaces
{
    public interface IBackgroundRefreshTokenService
    {
        public void DeleteExpiredTokens();
        public void DeleteUnusedRefreshTokens();
        public void RefreshToken(string token);
    }
}
