namespace CVRecognizingService.Domain.Exeptions
{
    [Serializable]
    public class NullObjectException : Exception
    {
        public NullObjectException() 
            : base() { }

        public NullObjectException(string message) 
            : base(message) { }

        public NullObjectException(string message, Exception innerException) 
            : base(message, innerException) { }
    }
}
