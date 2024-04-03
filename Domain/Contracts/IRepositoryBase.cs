
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IRepositoryBase<T>
    {
        Task<IQueryable<T>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<T> GetByIdAsync(long id,CancellationToken cancellationToken = default);

        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        Task<T> Create(T entity);
        void Update(Expression<Func<T, bool>> predicate, T entity);
        void Delete(T entity);
    }
}