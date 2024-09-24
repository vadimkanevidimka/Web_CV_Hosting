namespace AuthService.Buisness.Exceptions;

public class AlreadyExistsException(
    string message)
    : OperationFailedException(message)
{

}