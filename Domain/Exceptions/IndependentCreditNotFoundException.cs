namespace Domain.Exceptions
{
    public class InDependentCreditNoteNotFoundException : Exception
    {
        public InDependentCreditNoteNotFoundException() : base("InvoiceDocument not found")
        { }
    }
}
