namespace AuthService.Buisness.Exceptions;

public class ExpireException(
    string message)
    : UnauthorizedException(message)
{

}