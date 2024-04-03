using System.ComponentModel.DataAnnotations;
using Application.Helper;
using Domain.Enums;
using FluentValidation;

namespace Application.Models.Requests
{
    public class CreateInDependentCreditNoteReq
    {
        public long CreditNumber { get; set; }

        public string ExternalCreditNumber { get; set; }

        public SubmitStatus CreditStatus { get; set; }

        public decimal TotalAmount { get; set; }
    }

    public class CreateInDependentCreditNoteReqValidator : AbstractValidator<CreateInDependentCreditNoteReq>
    {
        public CreateInDependentCreditNoteReqValidator()
        {
            RuleFor(x => x.CreditNumber.ToString())
                .Cascade(CascadeMode = CascadeMode.Continue)
                .NotNull()
                .Must(PropertyValidation.IsValidNumber)
                .WithMessage("The  credit Number is not valid");


            When(o => o.ExternalCreditNumber != default, () =>
            {
                RuleFor(o => o.ExternalCreditNumber)
                    .Cascade(CascadeMode = CascadeMode.Continue)
                    .NotNull()
                    .Length(10)
                    .WithMessage("The extrenal Credit Number is not valid");
            });

            When(o => o.CreditStatus != default, () =>
            {
                RuleFor(o => o.CreditStatus)
                    .Cascade(CascadeMode = CascadeMode.Continue)
                    .IsInEnum()
                    .WithMessage("The creditStatus is not valid");
            });


            RuleFor(o => o.TotalAmount)
                .Cascade(CascadeMode = CascadeMode.Continue)
                .NotNull()
                .LessThan(0)
                .WithMessage("The totalAmount is not valid");
        }
    }
}