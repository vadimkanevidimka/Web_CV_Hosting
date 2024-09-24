//using MediatR;
//using Microsoft.AspNetCore.Identity;

//namespace AuthService.Buisness.Commands.Auth;

//public record LoginUserCommand(LoginUserRequest LoginUserRequest) : IRequest<AuthenticateResponse>;

//internal sealed class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, AuthenticateResponse>
//{
//    private readonly IAuthenticateService _authenticateService;
//    private readonly UserManager<User> _userManager;
//    private readonly SignInManager<User> _signInManager;

//    public LoginUserCommandHandler(UserManager<User> userManager, SignInManager<User> signInManager,
//        IAuthenticateService authenticateService)
//    {
//        _userManager = userManager;
//        _signInManager = signInManager;
//        _authenticateService = authenticateService;
//    }

//    public async Task<AuthenticateResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
//    {
//        var user = await _userManager.FindByEmailAsync(request.LoginUserRequest.Email);
        
//        var signInResult =
//            await _signInManager.PasswordSignInAsync(user, request.LoginUserRequest.Password, false, false);
//        return await _authenticateService.Authenticate(user, cancellationToken);
//    }
//}