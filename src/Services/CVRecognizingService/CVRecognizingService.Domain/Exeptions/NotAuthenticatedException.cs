namespace CVRecognizingService.Domain.Exeptions
{
    [Serializable]
    public class NotAuthenticatedException : Exception
    {
        public NotAuthenticatedException() : base() { }

        public NotAuthenticatedException(string message) : base(message) { }

        public NotAuthenticatedException(string message, Exception innerException) : base(message, innerException) { }
    }
}
