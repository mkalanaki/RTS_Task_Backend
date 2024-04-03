using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Helper;
using Application.Models.Requests;

namespace Domain.Repositories
{
    public interface IInvoiceDocumentRepository
    {
        Task<(IEnumerable<InvoiceDocument> Records, Pagination Pagination)> GetAllPagedAsync(
            BigQueryUrlQueryParameters urlQueryParameters);
    }
}