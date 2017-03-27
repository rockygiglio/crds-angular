using crds_angular.App_Start;
using crds_angular.Services;
using Moq;
using NUnit.Framework;
using MinistryPlatform.Translation.Repositories.Interfaces;
using Crossroads.Web.Common.Configuration;

namespace crds_angular.test.Services
{
    [Category("IntegrationTests")]
    public class AwsCloudsearchServiceTest
    {
        //private AwsCloudsearchService _fixture;
        
        //[SetUp]
        //public void SetUp()
        //{
        //    _mpFinderRepository = new Mock<IFinderRepository>();
        //    _mpConfigurationWrapper = new Mock<IConfigurationWrapper>();

        //    _fixture = new AwsCloudsearchService(_mpFinderRepository.Object, _mpConfigurationWrapper.Object);

        //    //force AutoMapper to register
        //    AutoMapperConfig.RegisterMappings();
        //}

        //[Test]
        //[Category("IntegrationTests")]
        //public void ShouldReturnBoundingBox()
        //{
        //    var rc = _fixture.BuildBoundingBox("39$1234", "-84$51", "27$66", "-81.55");
        //    Assert.IsNotNull(rc);
        //    Assert.IsTrue( rc.UpperLeftCoordinates.Lat == 39.1234);
        //}

        //[Test]
        //[Category("IntegrationTests")]
        //public void TestGetProximityBadOrigin()
        //{
            
        //    var result = _fixture.GetProximity("1234 test st, testy mctestface, te, 99999-9999, xyz", new List<AddressDTO> { address });
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(1, result.Count);
        //    Assert.IsNull(result[0]);
        //}
    }
}
