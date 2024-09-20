namespace CVRecognizingService.Domain.Abstracts.UOW
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// Transaction logic
        /// </summary>
        void CreateTransaction();
        void Commit();
        void Rollback();
        Task Save(CancellationToken token);
    }
}
