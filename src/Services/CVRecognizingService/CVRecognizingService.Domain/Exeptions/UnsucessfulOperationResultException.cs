namespace CVRecognizingService.Domain.Exeptions
{
    [Serializable]
    public class UnsucessfulOperationResultException : Exception
    {
        public UnsucessfulOperationResultException() 
            : base() { }

        public UnsucessfulOperationResultException(string message) 
            : base(message) { }

        public UnsucessfulOperationResultException(string message, Exception innerException) 
            : base(message, innerException) { }
    }
}
