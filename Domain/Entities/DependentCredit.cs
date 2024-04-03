using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Domain.Entities
{
    public class DependentCreditNote : BaseEntity
    {
        [MaxLength(10)]
        [Required]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]
        public long CreditNumber { get; set; }

        [MaxLength(10)]
        [Required]
        public string ExternalCreditNumber { get; set; }

        public SubmitStatus CreditStatus { get; set; }
        public long ParentInvoiceNumber { get; set; }
        public virtual InvoiceDocument InvoiceDocument { get; set; }

    }
}
