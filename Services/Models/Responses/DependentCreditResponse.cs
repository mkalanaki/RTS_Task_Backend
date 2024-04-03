using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Responses
{
    public class DependentCreditNoteResponse
    {
        public long CreditNumber { get; set; }

        public string ExternalCreditNumber { get; set; }

        public SubmitStatus CreditStatus { get; set; }

        public decimal TotalAmount { get; set; }
        public long ParentInvoiceNumber { get; set; }
     
    }
}
