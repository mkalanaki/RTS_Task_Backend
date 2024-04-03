using Application.Models.Requests;
using Application.Models.Responses;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IDependentCreditNoteService
    {
        public Task<DependentCreditNote> Create(DependentCreditNote DependentCreditNote,
            CancellationToken cancellationToken = default);

        public Task<DependentCreditNote> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<IQueryable<DependentCreditNote>> GetAllAsync(CancellationToken cancellationToken = default);
        void Update(DependentCreditNote entity);
        void Delete(DependentCreditNote entity);
        Task<IEnumerable<DependentCreditNote>> FindAsync(Expression<Func<DependentCreditNote, bool>> predicate);

    }
}