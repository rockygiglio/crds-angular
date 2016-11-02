using MinistryPlatform.Translation.Models.Payments;

namespace MinistryPlatform.Translation.Repositories.Interfaces
{
    public interface IInvoiceRepository
    {
        bool InvoiceExists(int invoiceId);
        MpInvoice GetInvoice(int invoiceId);
        void SetInvoiceStatus(int invoiceId, int statusId);
        MpInvoiceDetail GetInvoiceDetailForInvoice(int invoiceId);
    }
}
