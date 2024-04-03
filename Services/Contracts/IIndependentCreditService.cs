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
    public interface IInDependentCreditNoteService
    {
        public Task<InDependentCreditNote> Create(InDependentCreditNote entity, CancellationToken cancellationToken = default);
        public Task<InDependentCreditNote> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<IQueryable<InDependentCreditNote>> GetAllAsync(CancellationToken cancellationToken = default);
        void Update(InDependentCreditNote entity);
        void Delete(InDependentCreditNote entity);
        Task<IEnumerable<InDependentCreditNote>> FindAsync(Expression<Func<InDependentCreditNote, bool>> predicate);

    }
}