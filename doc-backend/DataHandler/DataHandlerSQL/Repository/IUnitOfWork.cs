namespace DataHandlerSQL.Repository
{
    public interface IUnitOfWork
    {
        IRepository<TEntity> Repository<TEntity>() where TEntity : class;

        void Commit();
    }
}
