using Domain.Entities;

using Infrastructure.Data;
using Infrustructure;
using Infrustructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Domain.Repositories;

public class RepositoryManager<T> : IRepositoryBase<T> where T : class
{
    private readonly RTSDbContext _dbContext;

    public RepositoryManager(RTSDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<IQueryable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.Set<T>().AsQueryable();
    }

    public async Task<T> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.Set<T>().FindAsync(id);
        return result;
    }

    public async Task<T> Create(T entity)
    {
       // await _dbContext.EnableIdentityInsertAsync<T>();
        await _dbContext.Set<T>().AddAsync(entity);
        return entity;
    }


    public void Update(Expression<Func<T, bool>> predicate, T entity)
    {
        var existingEntity = _dbContext.Set<T>().SingleOrDefault(predicate);
        if (existingEntity != null)
        {
            _dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);
            _dbContext.SaveChanges();
        }
    }


    public void Delete(T entity)
    {
        _dbContext.Entry(entity).State = EntityState.Deleted;
        _dbContext.Set<T>().Remove(entity);
        _dbContext.SaveChanges();
     

    }

   

    //public async Task<T> GetByIdAsync(long id, params Expression<Func<T, object>>[] includeProperties)
    //{
    //    IQueryable<T> query = _dbContext.Set<T>().Where()

    //    foreach (var includeProperty in includeProperties)
    //    {
    //        query = query.Include(includeProperty);
    //    }

    //    return await query.FirstOrDefaultAsync(e => (long)e.GetType().GetProperty("Id").GetValue(e) == id);
    //}

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate) 
    { 
        return await _dbContext.Set<T>().Where(predicate).ToListAsync(); 
    }

    
}

