using System.ComponentModel.DataAnnotations;
using Application.Helper;
using Domain.Enums;
using FluentValidation;

namespace Application.Models.Requests
{
    public class CreateInvoiceDocumentReq
    {
        public long InvoiceNumber { get; set; }

        public string ExternalInvoiceNumber { get; set; }

        public SubmitStatus InvoiceStatus { get; set; }

        public decimal TotalAmount { get; set; }
    }

    public class CreateInvoiceDocumentReqValidator : AbstractValidator<CreateInvoiceDocumentReq>
    {
        public CreateInvoiceDocumentReqValidator()
        {
            RuleFor(x => x.InvoiceNumber.ToString())
                .Cascade(CascadeMode = CascadeMode.Continue)
                .NotNull().WithMessage("The invoice Number is not valid")
                .Must(PropertyValidation.IsValidNumber)
                .WithMessage("The invoice Number is not valid");


            When(o => o.ExternalInvoiceNumber != default, () =>
            {
                RuleFor(o => o.ExternalInvoiceNumber)
                    .Cascade(CascadeMode = CascadeMode.Continue)
                    .NotNull()
                    .Length(10)
                    .WithMessage("The externalInvoiceNumber is not valid");
            });

            When(o => o.InvoiceStatus != default, () =>
            {
                RuleFor(o => o.InvoiceStatus)
                    .Cascade(CascadeMode = CascadeMode.Continue)
                    .IsInEnum()
                    .WithMessage("The invoiceStatus is not valid");
            });


            RuleFor(o => o.TotalAmount)
                .Cascade(CascadeMode = CascadeMode.Continue)
                .NotNull()
                .GreaterThan(0)
                .WithMessage("The totalAmount is not valid");
        }
    }
}