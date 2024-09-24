//using AuthService.Buisness.Common.DTOs.Auth;
//using AuthService.Buisness.Common.Interfaces;
//using AuthService.DataAccess.DBContext.Entities;
//using AuthService.DataAccess.Persistans.DbContext;
//using MediatR;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;

//namespace AuthService.Buisness.Commands.Auth;

//public record RefreshCommand(RefreshRequest RefreshRequest) 
//    : IRequest<AuthenticateResponse>;

//internal sealed class RefreshCommandHandler 
//    : IRequestHandler<RefreshCommand, AuthenticateResponse>
//{
//    private readonly IAuthenticateService _authenticateService;
//    private readonly IRefreshTokenValidator _refreshTokenValidator;
//    private readonly AuthorizationDbContext _context;
//    private readonly UserManager<User> _userManager;

//    public RefreshCommandHandler(IRefreshTokenValidator refreshTokenValidator,
//        AuthorizationDbContext context,
//        UserManager<User> userManager, 
//        IAuthenticateService authenticateService)
//    {
//        _refreshTokenValidator = refreshTokenValidator;
//        _context = context;
//        _userManager = userManager;
//        _authenticateService = authenticateService;
//    }

//    public async Task<AuthenticateResponse> Handle(RefreshCommand request, 
//        CancellationToken cancellationToken)
//    {
//        var refreshRequest = request.RefreshRequest;
//        var isValidRefreshToken = _refreshTokenValidator.Validate(refreshRequest.RefreshToken);
//        var refreshToken = await _context.RefreshTokens
//            .FirstOrDefaultAsync(x => x.Token == refreshRequest.RefreshToken, cancellationToken);


//        _context.RefreshTokens.Remove(refreshToken);
//        await _context.SaveChangesAsync(cancellationToken);

//        var user = await _userManager.FindByIdAsync(refreshToken.UserId);
//        return await _authenticateService.Authenticate(user, cancellationToken);
//    }
//}