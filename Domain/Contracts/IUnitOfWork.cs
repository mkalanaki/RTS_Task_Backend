using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IUnitOfWork
    {
        Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task Rollback(CancellationToken cancellationToken = default);
        IRepositoryBase<T> GetRepository<T>() where T : class;
       
    }
}