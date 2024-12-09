//using AuthService.DataAccess.Entities;
//using MediatR;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;

//namespace AuthService.Buisness.Commands.Users;

//public record DeleteUserCommand(string Id) : IRequest<bool>;

//internal sealed class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
//{
//    private readonly UserManager<User> _userManager;

//    public DeleteUserCommandHandler(UserManager<User> userManager) =>
//        _userManager = userManager;

//    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
//    {
//        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
//        var deleteResult = await _userManager.DeleteAsync(user);
//        return deleteResult.Succeeded;
//    }
//}