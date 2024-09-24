//using System.Linq.Expressions;
//using AuthService.Buisness.Common.DTOs.Users;
//using AuthService.DataAccess.DBContext.Entities;
//using MediatR;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;

//namespace AuthService.Buisness.Queries.Users;

//public record GetUserQuery(Expression<Func<User, bool>> Predicate) : IRequest<GetUserRequest>;

//internal sealed class GetUserQueryHandler : IRequestHandler<GetUserQuery, GetUserRequest>
//{
//    private readonly UserManager<User> _userManager;

//    public GetUserQueryHandler(UserManager<User> userManager) =>
//        _userManager = userManager;

//    public async Task<GetUserRequest> Handle(GetUserQuery request, CancellationToken cancellationToken)
//    {
//        var user = await _userManager.Users.FirstOrDefaultAsync(request.Predicate, cancellationToken);
//        return user.Adapt<GetUserRequest>();
//    }
//}