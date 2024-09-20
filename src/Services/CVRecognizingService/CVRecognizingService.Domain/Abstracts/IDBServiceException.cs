namespace CVRecognizingService.Domain.Abstracts
{
    internal interface IDBServiceException
    {
        public string Operation { get; }
        public object Value { get; }
    }
}
