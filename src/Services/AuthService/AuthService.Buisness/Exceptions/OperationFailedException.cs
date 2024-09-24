namespace AuthService.Buisness.Exceptions;

public class OperationFailedException(
    string message)
    : Exception(message)
{

}