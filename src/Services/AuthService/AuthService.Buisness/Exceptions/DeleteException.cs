namespace AuthService.Buisness.Exceptions;

public class DeleteException(
    string message)
    : OperationFailedException(message)
{

}