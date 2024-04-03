using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Data;
using Infrustructure;
using Infrustructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Application.UnitOfWork;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private RTSDbContext _dbContext;
    private readonly IDictionary<Type, dynamic> _repositories;
    private bool _disposed;

    public UnitOfWork(RTSDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Rollback(CancellationToken cancellationToken = default)
    {
        await _dbContext.Database.RollbackTransactionAsync();
    }


    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbContext.SaveChangesAsync();
           return true;
        }
        catch (Exception e)
        {
            throw new Exception(e.ToString());
        }
    }


    public IRepositoryBase<T> GetRepository<T>() where T : class
    {
        var entityType = typeof(T);

        var repositoryType = typeof(RepositoryManager<>);

        var repository = Activator.CreateInstance(repositoryType.MakeGenericType(entityType), _dbContext);


        return (RepositoryManager<T>) repository;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
        }

        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

   
}