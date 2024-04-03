using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Responses
{
    public class InvoiceDocumentResponse
    {
        public long InvoiceNumber { get; set; }

        public string ExternalInvoiceNumber { get; set; }

        public SubmitStatus InvoiceStatus { get; set; }

        public long TotalAmount { get; set; }
        public IEnumerable<DependentCreditNote> DependentCreditNote { get; set; }
    }
}
