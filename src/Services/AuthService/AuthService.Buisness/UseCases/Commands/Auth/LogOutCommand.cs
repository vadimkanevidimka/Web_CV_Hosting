//using System.Security.Claims;
//using AuthService.DataAccess.DBContext.Entities;
//using AuthService.DataAccess.Persistans.DbContext;
//using MediatR;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;

//namespace AuthService.Buisness.Commands.Auth;

//public record LogOutCommand : IRequest<Unit>;

//internal sealed class LogOutCommandHandler 
//    : IRequestHandler<LogOutCommand, Unit>
//{
//    private readonly IHttpContextAccessor _httpContextAccessor;
//    private readonly SignInManager<User> _signInManager;
//    private readonly AuthorizationDbContext _context;

//    public LogOutCommandHandler(IHttpContextAccessor httpContextAccessor,
//        SignInManager<User> signInManager,
//        AuthorizationDbContext context)
//    {
//        _httpContextAccessor = httpContextAccessor;
//        _signInManager = signInManager;
//        _context = context;
//    }

//    public async Task<Unit> Handle(LogOutCommand request, 
//        CancellationToken cancellationToken)
//    {
//        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue("id");
//        await _signInManager.SignOutAsync();
//        var refreshTokens = await _context.RefreshTokens
//            .Where(x => x.UserId == userId)
//            .ToListAsync(cancellationToken);
//        _context.RefreshTokens.RemoveRange(refreshTokens);
//        await _context.SaveChangesAsync(cancellationToken);
//        return Unit.Value;
//    }
//}