namespace AuthService.Buisness.Exceptions;

public class UpdateException(
    string message)
    : OperationFailedException(message)
{

}