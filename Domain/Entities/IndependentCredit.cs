using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using Domain.Enums;

namespace Domain.Entities
{
    public class InDependentCreditNote : BaseEntity
    {
        [MaxLength(10)]
        [Required]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]
        public long CreditNumber { get; set; }

        [MaxLength(10)] 
        [Required] 
        public string ExternalCreditNumber { get; set; }

        public SubmitStatus CreditStatus { get; set; }
    }
}