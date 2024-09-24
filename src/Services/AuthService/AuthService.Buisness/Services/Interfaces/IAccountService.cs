using AuthService.Buisness.Dtos.Tokens;
using AuthService.Buisness.Dtos.User;

namespace AuthService.Buisness.Services.Interfaces;

public interface IAccountService
{
    Task<UserReadDto> RegisterAsync(UserRegisterDto userRegisterDto);
    Task<TokensResponse> LoginAsync(UserLoginDto userLoginDto);
    Task<TokensResponse> RefreshTokenAsync(TokenRefreshRequest tokenRefreshRequest);
    Task<UserReadDto> UpdateAsync(UserUpdateDto userUpdateDto);
    Task<UserReadDto> DeleteAsync(string userId);
    Task<UserReadDto> GetUserById(string userId);
}