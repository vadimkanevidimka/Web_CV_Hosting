using CVRecognizingService.Domain.Abstracts;

namespace Events_Web_application.Application.Services.Exceptions
{
    public class ServiceException : Exception, IDBServiceException
    {
        public string Operation { get; init; }
        public object Value { get; init; }
        public ServiceException(string Operation, object Value, string Message) 
            : base(Message) {}
    }
}
