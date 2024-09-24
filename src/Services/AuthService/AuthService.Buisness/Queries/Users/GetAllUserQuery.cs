//using MediatR;
//using AuthService.Buisness.Common.DTOs.Users;
//using AuthService.DataAccess.DBContext.Entities;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;

//namespace AuthService.Buisness.Queries.Users;

//public record GetAllUserQuery : IRequest<IReadOnlyList<GetUserRequest>>;

//internal sealed class GetAllUserQueryHandler : IRequestHandler<GetAllUserQuery, IReadOnlyList<GetUserRequest>>
//{
//    private readonly UserManager<User> _userManager;

//    public GetAllUserQueryHandler(UserManager<User> userManager) =>
//        _userManager = userManager;

//    public async Task<IReadOnlyList<GetUserRequest>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
//    {
//        var users = await _userManager.Users
//            .ToListAsync(cancellationToken);
//        return users.AsReadOnly<GetUserRequest>();
//    }
//}