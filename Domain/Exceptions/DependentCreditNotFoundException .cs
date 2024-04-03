namespace Domain.Exceptions
{
    public class DependentCreditNoteNotFoundException : Exception
    {
        public DependentCreditNoteNotFoundException() : base("DependentCreditNote not found")
        { }
    }
}
