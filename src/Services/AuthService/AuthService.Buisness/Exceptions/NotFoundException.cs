namespace AuthService.Buisness.Exceptions;

public class NotFoundException(
    string message)
    : Exception(message)
{

}