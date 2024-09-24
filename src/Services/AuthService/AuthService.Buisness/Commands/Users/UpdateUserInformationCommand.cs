//using AuthService.Buisness.Common.DTOs.Users;
//using AuthService.DataAccess.DBContext.Entities;
//using MediatR;
//using Microsoft.AspNetCore.Identity;

//namespace AuthService.Buisness.Commands.Users;

//public record UpdateUserInformationCommand() 
//    : IRequest<bool>;

//internal sealed class UpdateUserInformationCommandHandler : IRequestHandler<UpdateUserInformationCommand, bool>
//{
//    private readonly UserManager<User> _userManager;
//    private readonly IMapper _mapper;

//    public UpdateUserInformationCommandHandler(UserManager<User> userManager, IMapper mapper) =>
//        (_userManager, _mapper) = (userManager, mapper);


//    public async Task<bool> Handle(UpdateUserInformationCommand request, CancellationToken cancellationToken)
//    {
//        var updateInfoDto = request.UpdateUserInformationRequest;
//        var user = await _userManager.FindByEmailAsync(updateInfoDto.Email);
//        _mapper.Map(updateInfoDto, user);
//        var updateResult = await _userManager.UpdateAsync(user);
//        return updateResult.Succeeded;
//    }
//}