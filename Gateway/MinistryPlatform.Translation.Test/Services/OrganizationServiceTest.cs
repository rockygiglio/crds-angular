using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Crossroads.Utilities.Interfaces;
using FsCheck;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Services;
using MinistryPlatform.Translation.Services.Interfaces;
using Moq;
using NUnit.Framework;
using Random = System.Random;

namespace MinistryPlatform.Translation.Test.Services
{
    [TestFixture]
    public class OrganizationServiceTest
    {
        [SetUp]
        public void SetUp()
        {
            _mpServiceMock = new Mock<IMinistryPlatformService>();
            _authService = new Mock<IAuthenticationService>();
            _configWrapper = new Mock<IConfigurationWrapper>();

            _configWrapper.Setup(m => m.GetEnvironmentVarAsString("API_USER")).Returns("uid");
            _configWrapper.Setup(m => m.GetEnvironmentVarAsString("API_PASSWORD")).Returns("pwd");
            _configWrapper.Setup(m => m.GetConfigIntValue("OrganizationsPage")).Returns(ORGPAGE);
            _configWrapper.Setup(m => m.GetConfigIntValue("LocationsForOrg")).Returns(LocPage);
            _authService.Setup(m => m.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(new Dictionary<string, object> {{"token", "ABC"}, {"exp", "123"}});

            _fixture = new OrganizationService(_authService.Object, _configWrapper.Object, _mpServiceMock.Object);
        }

        private OrganizationService _fixture;
        private Mock<IMinistryPlatformService> _mpServiceMock;
        private Mock<IAuthenticationService> _authService;
        private Mock<IConfigurationWrapper> _configWrapper;

        private const int ORGPAGE = 1234;
        private const int LocPage = 180;

        private List<Dictionary<string, object>> ListOfValidOrganizations()
        {
            return new List<Dictionary<string, object>>()
            {
                ValidMPOrganization("Ingage"),
                ValidMPOrganization("Crossroads")
            };
        }

        private static List<Dictionary<string, object>> ListOfOneUniquelyNamedOrganization(bool valid = true)
        {
            if (valid)
            {
                return new List<Dictionary<string, object>>()
                {
                    ValidMPOrganization("Ingage")
                };
            }
            return new List<Dictionary<string, object>>()
            {
                InvalidMPOrganization("Ingage")
            };
        }

        private List<Dictionary<string, object>> ListOfOrganizationsWithSameName()
        {
            return new List<Dictionary<string, object>>()
            {
                ValidMPOrganization("Ingage"),
                ValidMPOrganization("Ingage")
            };
        }

        private static Dictionary<string, object> ValidMPOrganization(string name)
        {
            return new Dictionary<string, object>()
            {
                {"dp_RecordID", It.IsAny<int>()},
                {"Primary_Contact", It.IsAny<int>()},
                {"End_Date", It.IsAny<DateTime>()},
                {"Start_Date", It.IsAny<DateTime>()},
                {"Name", name},
                {"Open_Signup", true}
            };
        }

        private static Dictionary<string, object> InvalidMPOrganization(string name)
        {
            return new Dictionary<string, object>()
            {
                {"dp_RecordID", It.IsAny<int>()},
                {"Primary_Contact", It.IsAny<int>()},
                {"Start_Date", It.IsAny<DateTime>()},
                {"Name", name},
                {"Open_Signup", true}
            };
        }

        private static List<Dictionary<string, object>> LocationList()
        {
            return new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"Location ID", It.IsAny<int>()},
                    {"Location Name", "Oakley"},
                    {"Location Type ID", 1},
                    {"Organization ID", 1},
                    {"Name", "Crossroads"},
                    {"Address Line 1", "123 Sesame St."},
                    {"City", "Cincinnati"},
                    {"State/Region", "OH"},
                    {"Postal Code", 45209},
                    {"Image URL", "www.com.net"}
                },
                new Dictionary<string, object>
                {
                    {"Location ID", It.IsAny<int>()},
                    {"Location Name", "Mason"},
                    {"Location Type ID", 1},
                    {"Organization ID", 1},
                    {"Name", "Crossroads"},
                    {"Address Line 1", "221B Baker St."},
                    {"City", "Cincinnati"},
                    {"State/Region", "OH"},
                    {"Postal Code", 45209},
                    {"Image URL", "www.com.net"}
                }
            };
        }

        [Test]
        public void ReallyRandom()
        {
            var x = RandomProvider.GetThreadRandom();
            var y = x.Next();
            var z = x.Next();
            Assert.AreNotEqual(y,z);

            var list = Enumerable.Repeat(getNum(x), 5).ToList();
            Assert.IsNotNull(list);
        }

        private int getNum(Random rnd)
        {
            return rnd.Next();
        }

        [Test]
        public void NotReallyRandom()
        {
            var rnd1 = new Random();
            var y = rnd1.Next();
            var rnd2 = new Random();
            var z = rnd2.Next();
            Assert.AreEqual(y, z);
        }

        [Test]
        public void ShouldGetAListOfOrgs()
        {
            var fakeToken = "randomString";
            _mpServiceMock.Setup(m => m.GetRecordsDict(
                ORGPAGE,
                fakeToken,
                It.IsAny<string>(),
                It.IsAny<string>())
                ).Returns(ListOfValidOrganizations);
            var ret = _fixture.GetOrganizations(fakeToken);
            Assert.IsInstanceOf<List<MPOrganization>>(ret, "The return value is not an instance of an MPOrganization");
        }

        [Test]
        public void shouldGetLocations()
        {
            var fakeToken = "randomString";
            _mpServiceMock.Setup(m => m.GetSubpageViewRecords(LocPage, It.IsAny<int>(), fakeToken, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).Returns(LocationList());
            var ret = _fixture.GetLocationsForOrganization(1, fakeToken);
            Assert.IsInstanceOf<List<Location>>(ret);
            Assert.IsNotNull(ret);
        }

        [Test]
        public void ShouldGetOrganization()
        {
            var orgs = ListOfOneUniquelyNamedOrganization();
            var fakeToken = "randomString";
            var name = "Ingage";
            _mpServiceMock.Setup(m => m.GetRecordsDict(
                ORGPAGE,
                fakeToken,
                string.Format("{0},", name),
                It.IsAny<string>())
                ).Returns(orgs);
            var ret = _fixture.GetOrganization(name, fakeToken);

            Assert.IsInstanceOf<MPOrganization>(ret, "The return value is not an instance of an MPOrganization");
            Assert.AreEqual(orgs.FirstOrDefault().ToString("Name"), ret.Name);
        }

        [Test]
        public void ShouldHandleAnInvalidOrg()
        {
            var orgs = ListOfOneUniquelyNamedOrganization(false);
            var fakeToken = "randomString";
            var name = "Ingage";
            _mpServiceMock.Setup(m => m.GetRecordsDict(
                ORGPAGE,
                fakeToken,
                string.Format("{0},", name),
                It.IsAny<string>())
                ).Returns(orgs);
            Assert.Throws<KeyNotFoundException>(() => _fixture.GetOrganization(name, fakeToken));
        }

        [Test]
        public void ShouldHandleEmptyListOfOrgs()
        {
            var fakeToken = "randomString";
            _mpServiceMock.Setup(m => m.GetRecordsDict(
                ORGPAGE,
                fakeToken,
                It.IsAny<string>(),
                It.IsAny<string>())
                ).Returns(new List<Dictionary<string, object>>());
            var ret = _fixture.GetOrganizations(fakeToken);
            Assert.IsInstanceOf<List<MPOrganization>>(ret, "The return value is not an instance of an MPOrganization");
            Assert.IsEmpty(ret);
        }

        [Test]
        public void ShouldHandleNoOrgs()
        {
            var emptyList = new List<Dictionary<string, object>>();
            var fakeToken = "randomString";
            var name = "Ingage";
            _mpServiceMock.Setup(m => m.GetRecordsDict(
                ORGPAGE,
                fakeToken,
                string.Format("{0},", name),
                It.IsAny<string>())
                ).Returns(emptyList);
            var ret = _fixture.GetOrganization(name, fakeToken);
            Assert.IsNull(ret);
        }

        [Test]
        public void ShouldThrowExceptionIfMultipleOrgsReturned()
        {
            var fakeToken = "randomString";
            var name = "Ingage";
            _mpServiceMock.Setup(m => m.GetRecordsDict(
                ORGPAGE,
                fakeToken,
                string.Format("{0},", name),
                It.IsAny<string>())
                ).Returns(ListOfOrganizationsWithSameName());
            Assert.Throws<InvalidOperationException>(() => _fixture.GetOrganization(name, fakeToken));
        }
    }

    public static class RandomProvider
    {
        private static int _seed = Environment.TickCount;

        private static readonly ThreadLocal<Random> RandomWrapper = new ThreadLocal<Random>(() =>
            new Random(Interlocked.Increment(ref _seed))
        );

        public static Random GetThreadRandom()
        {
            return RandomWrapper.Value;
        }
    }
}