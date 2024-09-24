namespace AuthService.Buisness.Exceptions;

public class UnauthorizedException(
    string message)
    : Exception(message)
{

}