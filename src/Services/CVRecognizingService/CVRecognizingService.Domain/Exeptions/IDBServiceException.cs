namespace CVRecognizingService.Domain.Exeptions
{
    internal interface IDBServiceException
    {
        public string Operation { get; }
        public object Value { get; }
    }
}
