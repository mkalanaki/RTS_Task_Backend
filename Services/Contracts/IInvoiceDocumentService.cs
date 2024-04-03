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
    public interface IInvoiceDocumentService
    {
         Task<InvoiceDocument> Create(InvoiceDocument entity, CancellationToken cancellationToken = default);
         Task<InvoiceDocument> GetByIdAsync(long id, CancellationToken cancellationToken = default);
     
        Task<IQueryable<InvoiceDocument>> GetAllAsync(CancellationToken cancellationToken = default);
         Task<IEnumerable<InvoiceDocument>> FindAsync(Expression<Func<InvoiceDocument, bool>> predicate);
        void Update(InvoiceDocument entity);
        void Delete(InvoiceDocument entity);
    }
}