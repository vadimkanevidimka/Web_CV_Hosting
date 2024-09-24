namespace AuthService.Buisness.Exceptions;

public class CreationException(
    string message)
    : OperationFailedException(message)
{

}