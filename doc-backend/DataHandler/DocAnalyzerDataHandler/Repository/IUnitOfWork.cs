namespace DocAnalyzerDataHandler.Repository
{
    public interface IUnitOfWork
    {
        IRepository<Employee> Employees { get; }

        IRepository<Usercredential> Usercredentials { get; }

        void Commit();
    }
}
