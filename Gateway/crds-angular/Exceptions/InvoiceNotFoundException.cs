using System;

namespace crds_angular.Exceptions
{
    public class InvoiceNotFoundException : Exception
    {
        public int InvoiceId;

        public InvoiceNotFoundException(int invoiceId) : base($"Invoice {invoiceId} not found.")
    {
            InvoiceId = invoiceId;
        }
    }
}