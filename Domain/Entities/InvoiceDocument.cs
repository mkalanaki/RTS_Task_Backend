using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Domain.Entities
{
    public class InvoiceDocument : BaseEntity
    {
        [MaxLength(10)]
        [Required]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]
        public long InvoiceNumber { get; set; }

        [MaxLength(10)] [Required] public string ExternalInvoiceNumber { get; set; }

        public SubmitStatus InvoiceStatus { get; set; }

        // Navigation property for dependent credit notes
        public virtual ICollection<DependentCreditNote> DependentCreditNote { get; set; }
    }
}