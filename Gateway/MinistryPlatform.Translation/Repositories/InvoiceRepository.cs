using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Models.Payments;
using MinistryPlatform.Translation.Repositories.Interfaces;

namespace MinistryPlatform.Translation.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly IMinistryPlatformRestRepository _ministryPlatformRest;
        private readonly IConfigurationWrapper _configurationWrapper;
        private readonly IApiUserRepository _apiUserRepository;

        public InvoiceRepository(IConfigurationWrapper configurationWrapper, IMinistryPlatformRestRepository ministryPlatformRest, IApiUserRepository apiUserRepository)
        {
            _configurationWrapper = configurationWrapper;
            _ministryPlatformRest = ministryPlatformRest;
            _apiUserRepository = apiUserRepository;
        }
        public bool InvoiceExists(int invoiceId)
        {
           return GetInvoice(invoiceId) != null;
        }

        public void SetInvoiceStatus(int invoiceId, int statusId)
        {
            var dict = new Dictionary<string, object> {{"Invoice_ID", invoiceId}, {"Invoice_Status_ID", statusId}};

            var update = new List<Dictionary<string, object>> {dict};

            var apiToken = _apiUserRepository.GetToken();
            _ministryPlatformRest.UsingAuthenticationToken(apiToken).Put("Invoices",update);
        }

        public MpInvoice GetInvoice(int invoiceId)
        {
            var apiToken = _apiUserRepository.GetToken();
            return _ministryPlatformRest.UsingAuthenticationToken(apiToken).Get<MpInvoice>(invoiceId);
        }

        public MpInvoiceDetail GetInvoiceDetailForInvoice(int invoiceId)
        {
            var filter = new Dictionary<string, object> {{"Invoice_ID", invoiceId}};

            var apiToken = _apiUserRepository.GetToken();
            return _ministryPlatformRest.UsingAuthenticationToken(apiToken).Get<MpInvoiceDetail>("Invoice_Detail", filter).FirstOrDefault();
        }
    }
}
