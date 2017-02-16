using System.Collections.Generic;
using Crossroads.Web.Common.Configuration;
using Crossroads.Web.Common.MinistryPlatform;
using MinistryPlatform.Translation.Models.Payments;
using MinistryPlatform.Translation.Repositories;
using MinistryPlatform.Translation.Repositories.Interfaces;
using Moq;
using NUnit.Framework;

namespace MinistryPlatform.Translation.Test.Services
{
    [TestFixture]
    public class InvoiceRepositoryTest
    {

        private Mock<IMinistryPlatformRestRepository> _ministryPlatformRest;
        private Mock<IApiUserRepository> _apiUserRepository;
        private Mock<IProductRepository> _productRepository;
        private Mock<IConfigurationWrapper> _configurationWrapper;
        private IInvoiceRepository _fixture;

        [SetUp]
        public void SetUp()
        {
            _ministryPlatformRest = new Mock<IMinistryPlatformRestRepository>();
            _apiUserRepository = new Mock<IApiUserRepository>();
            _productRepository = new Mock<IProductRepository>();
            _configurationWrapper = new Mock<IConfigurationWrapper>();

            _configurationWrapper.Setup(m => m.GetConfigIntValue("InvoiceCancelled")).Returns(4);
            
            _fixture = new InvoiceRepository(_ministryPlatformRest.Object, _apiUserRepository.Object, _productRepository.Object, _configurationWrapper.Object);  
        }

        [Test]
        public void CancelledInvoiceShouldNotShowAsExists()
        {
            const int invoiceId = 123445;
            const string token = "letmein";
            var filter = $"Invoice_ID={invoiceId} AND Invoice_Status_ID!={4}";

            _apiUserRepository.Setup(m => m.GetToken()).Returns(token);
            _ministryPlatformRest.Setup(m => m.UsingAuthenticationToken(token)).Returns(_ministryPlatformRest.Object);
            _ministryPlatformRest.Setup(m => m.Search<MpInvoice>(filter, null as string, null, false)).Returns(new List<MpInvoice>());

            var result =_fixture.InvoiceExists(invoiceId);
            Assert.IsFalse(result);

        }

    }
}