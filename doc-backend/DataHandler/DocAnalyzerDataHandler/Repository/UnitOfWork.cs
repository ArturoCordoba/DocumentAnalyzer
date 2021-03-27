using Microsoft.EntityFrameworkCore;

namespace DocAnalyzerDataHandler.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private DocAnalyzerContext _dbContext;
        private BaseRepository<Employee> _employees;
        private BaseRepository<Usercredential> _usercredentials;

        public UnitOfWork(DocAnalyzerContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IRepository<Employee> Employees
        {
            get
            {
                return _employees ?? (_employees = new BaseRepository<Employee>(_dbContext));
            }
        }

        public IRepository<Usercredential> Usercredentials
        {
            get
            {
                return _usercredentials ?? (_usercredentials = new BaseRepository<Usercredential>(_dbContext));
            }
        }

        public void Commit()
        {
            _dbContext.SaveChanges();
        }
    }
}
