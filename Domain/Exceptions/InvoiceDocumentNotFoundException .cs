namespace Domain.Exceptions
{
    public class InvoiceDocumenNotFoundException : Exception
    {
        public InvoiceDocumenNotFoundException() : base("InvoiceDocumen not found")
        { }
    }
}
