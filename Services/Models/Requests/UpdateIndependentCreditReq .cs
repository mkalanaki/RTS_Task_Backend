using System.ComponentModel.DataAnnotations;
using Application.Helper;
using Domain.Enums;
using FluentValidation;

namespace Application.Models.Requests
{
    public class UpdateInDependentCreditNoteReq
    {
     
        public string ExternalCreditNumber { get; set; }

        public SubmitStatus CreditStatus { get; set; }

        public decimal TotalAmount { get; set; }
    }

    public class UpdateInDependentCreditNoteReqValidator : AbstractValidator<UpdateInDependentCreditNoteReq>
    {
        public UpdateInDependentCreditNoteReqValidator()
        {
            

            When(o => o.ExternalCreditNumber != default, () =>
            {
                RuleFor(o => o.ExternalCreditNumber)
                    .Cascade(CascadeMode = CascadeMode.Continue)
                    .NotNull()
                    .Length(10)
                    .WithMessage("The externalInvoiceNumber is not valid");
            });

            When(o => o.CreditStatus != default, () =>
            {
                RuleFor(o => o.CreditStatus)
                    .Cascade(CascadeMode = CascadeMode.Continue)
                    .IsInEnum()
                    .WithMessage("The invoiceStatus is not valid");
            });


            RuleFor(o => o.TotalAmount)
                .Cascade(CascadeMode = CascadeMode.Continue)
                .NotNull()
                .LessThan(0)
                .WithMessage("The totalAmount is not valid");
        }
    }
}