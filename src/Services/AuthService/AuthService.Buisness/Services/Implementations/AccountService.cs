using AuthService.Buisness.Dtos.Tokens;
using AuthService.Buisness.Dtos.User;
using AuthService.Buisness.Exceptions;
using AuthService.Buisness.Services.Algorithms;
using AuthService.Buisness.Services.Interfaces;
using AuthService.DataAccess.Entities;
using AuthService.DataAccess.Persistans.Repositories.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using RefreshToken = AuthService.DataAccess.Entities.RefreshToken;

namespace AuthService.Buisness.Services.Implementations;

public class AccountService 
    : IAccountService
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly TokensGenerator _tokensGenerator;
    private readonly IMapper _mapper;

    public AccountService(UserManager<User> userManager,
    RoleManager<IdentityRole> roleManager,
    IRefreshTokenRepository refreshTokenRepository,
    TokensGenerator tokensGenerator, IMapper mapper)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _refreshTokenRepository = refreshTokenRepository;
        _tokensGenerator = tokensGenerator;
        _mapper = mapper;
    }

    public async Task<UserReadDto> RegisterAsync(UserRegisterDto userRegisterDto)
    {
        var user = await _userManager.FindByEmailAsync(userRegisterDto.Email);

        if (user is not null)
        {
            throw new AlreadyExistsException(typeof(User).ToString());
        }

        var newUser = _mapper.Map<User>(userRegisterDto);

        var createUserResult = await _userManager.CreateAsync(newUser, userRegisterDto.Password);
        var addToRoleResult = await _userManager.AddToRoleAsync(newUser, Roles.User);


        if (!createUserResult.Succeeded)
        {
            throw new CreationException(createUserResult.Errors.ToString());
        }

        if (!addToRoleResult.Succeeded)
        {
            throw new CreationException(addToRoleResult.Errors.ToString());
        }

        return _mapper.Map<UserReadDto>(newUser);
    }

    public async Task<TokensResponse> RefreshTokenAsync(TokenRefreshRequest tokenRefreshRequest)
    {
        var currentRefreshToken = await _refreshTokenRepository.GetByIdAsync(
            new Guid(tokenRefreshRequest.RefreshToken));

        if (currentRefreshToken is null)
        {
            throw new NotFoundException(typeof(RefreshToken).ToString());
        }

        var user = currentRefreshToken.User;
        _refreshTokenRepository.Delete(currentRefreshToken);
        if (!currentRefreshToken.IsActive)
        {
            throw new ExpireException(typeof(RefreshToken).ToString());
        }

        return await GenerateAndSaveTokens(user);
    }

    public async Task<UserReadDto> UpdateAsync(UserUpdateDto userUpdateDto)
    {
        var user = await _userManager.FindByIdAsync(userUpdateDto.Id);

        if (user is null)
        {
            throw new NotFoundException(typeof(User).ToString());
        }

        user = _mapper.Map<User>(userUpdateDto); ;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            throw new UpdateException(typeof(User).ToString());
        }

        return _mapper.Map<UserReadDto>(user);
    }

    public async Task<UserReadDto> DeleteAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
        {
            throw new NotFoundException(typeof(User).ToString());
        }

        var result = await _userManager.DeleteAsync(user);

        if (!result.Succeeded)
        {
            throw new DeleteException(typeof(User).ToString());
        }

        return _mapper.Map<UserReadDto>(user);
    }

    public async Task<UserReadDto> GetUserById(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
        {
            throw new NotFoundException(typeof(User).ToString());
        }

        return _mapper.Map<UserReadDto>(user);
    }

    public async Task<TokensResponse> LoginAsync(UserLoginDto userLoginDto)
    {
        var user = await _userManager.FindByEmailAsync(userLoginDto.Email);

        if (user is null)
        {
            throw new NotFoundException(typeof(User).ToString());
        }

        var result = await _userManager.CheckPasswordAsync(user, userLoginDto.Password);

        if (!result)
        {
            throw new UnauthorizedException("Wrong email or password");
        }

        return await GenerateAndSaveTokens(user);
    }

    private async Task<TokensResponse> GenerateAndSaveTokens(User user)
    {
        var refreshToken = _tokensGenerator.GenerateRefreshToken();
        var refreshTokenEntity = _mapper.Map<RefreshToken>(refreshToken);
        refreshTokenEntity.User = user;

        await _refreshTokenRepository.AddAsync(refreshTokenEntity);

        await _refreshTokenRepository.SaveChangesAsync();

        var tokenResponse = new TokensResponse
        {
            AccessToken = _tokensGenerator.GenerateAccessToken(refreshTokenEntity.User,
                await _userManager.GetRolesAsync(refreshTokenEntity.User)),
            RefreshToken = refreshToken
        };

        return tokenResponse;
    }
}