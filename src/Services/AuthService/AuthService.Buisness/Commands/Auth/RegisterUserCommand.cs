//using AuthService.Buisness.Common.DTOs.Auth;
//using AuthService.DataAccess.DBContext.Entities;
//using MediatR;
//using Microsoft.AspNetCore.Identity;

//namespace AuthService.Buisness.Commands.Auth;

//public record RegisterUserCommand(RegisterUserRequest RegisterUserRequest) : IRequest<Unit>;

//internal sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Unit>
//{
//    private readonly UserManager<User> _userManager;
//    public RegisterUserCommandHandler(UserManager<User> userManager) => _userManager = userManager;

//    public async Task<Unit> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
//    {
//        var user = new User() { Email = request.RegisterUserRequest.Email };
//        var createResult = await _userManager.CreateAsync(user, request.RegisterUserRequest.Password);
//        return createResult.Succeeded
//            ? Unit.Value
//            : Unit.Value;
//    }
//}