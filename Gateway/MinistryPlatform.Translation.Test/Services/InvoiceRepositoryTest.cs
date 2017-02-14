using Crossroads.Web.Common.MinistryPlatform;
using MinistryPlatform.Translation.Models.Payments;
using MinistryPlatform.Translation.Repositories;
using MinistryPlatform.Translation.Repositories.Interfaces;
using Moq;
using NUnit.Core;
using NUnit.Framework;

namespace MinistryPlatform.Translation.Test.Services
{
    [TestFixture()]
    public class InvoiceRepositoryTest
    {

        private Mock<IMinistryPlatformRestRepository> _ministryPlatformRest;
        private Mock<IApiUserRepository> _apiUserRepository;
        private Mock<IProductRepository> _productRepository;
        private IInvoiceRepository _fixture;

        [SetUp]
        public void SetUp()
        {
            _ministryPlatformRest = new Mock<IMinistryPlatformRestRepository>();
            _apiUserRepository = new Mock<IApiUserRepository>();
            _productRepository = new Mock<IProductRepository>();
            _fixture = new InvoiceRepository(_ministryPlatformRest.Object, _apiUserRepository.Object, _productRepository.Object);  
        }

        public void CancelledInvoiceShouldNotShowAsExists()
        {
            const int invoiceId = 123445;
            const string token = "letmein";

            _apiUserRepository.Setup(m => m.GetToken()).Returns(token);
            _ministryPlatformRest.Setup(m => m.UsingAuthenticationToken(token)).Returns(_ministryPlatformRest.Object);
            _ministryPlatformRest.Setup(m => m.Get<MpInvoice>(invoiceId, null as string)).Returns();

        }

    }
}