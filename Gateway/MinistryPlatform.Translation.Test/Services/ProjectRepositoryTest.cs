using System.Collections.Generic;
using System.Linq;
using Crossroads.Web.Common.MinistryPlatform;
using MinistryPlatform.Translation.Models.GoCincinnati;
using MinistryPlatform.Translation.Repositories.GoCincinnati;
using MinistryPlatform.Translation.Repositories.Interfaces.GoCincinnati;
using Moq;
using NUnit.Framework;

namespace MinistryPlatform.Translation.Test.Services
{
    [TestFixture]
    public class ProjectRepositoryTest
    {
        private Mock<IMinistryPlatformRestRepository> _ministryPlatformRest;        
        private IProjectRepository _fixture;

        [SetUp]
        public void Setup()
        {
            _ministryPlatformRest = new Mock<IMinistryPlatformRestRepository>();
            _fixture = new ProjectRepository(_ministryPlatformRest.Object);
        }

        [Test]
        public void GetEmptyListOfProjectById()
        {
            const int id = 564;
            const string token = "letmein";

            var filterString = $"Project_ID = {id} AND Initiative_ID_Table.[Volunteer_Signup_Start_Date]<=GetDate() AND Initiative_ID_Table.[Volunteer_Signup_End_Date]>=GetDate()";

            _ministryPlatformRest.Setup(m => m.UsingAuthenticationToken(token)).Returns(_ministryPlatformRest.Object);
            _ministryPlatformRest.Setup(m => m.Search<MpProject>(filterString, It.IsAny<List<string>>(), null as string, true))
                .Returns((string filter, List<string> columns, string blah, bool distinct) => new List<MpProject>());

            var ret = _fixture.GetProject(id, token);
            Assert.IsFalse(ret.Status);
            _ministryPlatformRest.VerifyAll();            
        }

        [Test]
        public void GetValidProjectById()
        {
            const int id = 564;
            const string token = "letmein";

            var filterString = $"Project_ID = {id} AND Initiative_ID_Table.[Volunteer_Signup_Start_Date]<=GetDate() AND Initiative_ID_Table.[Volunteer_Signup_End_Date]>=GetDate()";
            var returnVal = ValidProjectList(id);

            _ministryPlatformRest.Setup(m => m.UsingAuthenticationToken(token)).Returns(_ministryPlatformRest.Object);
            _ministryPlatformRest.Setup(m => m.Search<MpProject>(filterString, It.IsAny<List<string>>(), null as string, true))
                .Returns((string filter, List<string> columns, string blah, bool distinct) => returnVal);

            var ret = _fixture.GetProject(id, token);
            Assert.IsTrue(ret.Status);
            Assert.AreEqual(returnVal.First(), ret.Value);
            _ministryPlatformRest.VerifyAll();
        }

        [Test]
        public void GetMultipleValidProjectsById()
        {
            // This should never happen, but if it does, we'll be ready!
            const int id = 564;
            const string token = "letmein";

            var filterString = $"Project_ID = {id} AND Initiative_ID_Table.[Volunteer_Signup_Start_Date]<=GetDate() AND Initiative_ID_Table.[Volunteer_Signup_End_Date]>=GetDate()";
            var returnVal = ValidProjectList(id);
            returnVal.Add(new MpProject
            {
                AddressId = 2,
                InitiativeId = 65,
                LocationId = 123,
                OrganizationId = 12,
                ProjectId = 564,
                ProjectName = "Another project with the same id? Cmon Man!!",
                ProjectStatusId = 3, 
                ProjectTypeId = 3
            });

            _ministryPlatformRest.Setup(m => m.UsingAuthenticationToken(token)).Returns(_ministryPlatformRest.Object);
            _ministryPlatformRest.Setup(m => m.Search<MpProject>(filterString, It.IsAny<List<string>>(), null as string, true))
                .Returns((string filter, List<string> columns, string blah, bool distinct) => returnVal);

            var ret = _fixture.GetProject(id, token);
            Assert.IsTrue(ret.Status);
            Assert.AreEqual(returnVal.First(), ret.Value);
            _ministryPlatformRest.VerifyAll();
        }

        protected static List<MpProject> ValidProjectList(int id)
        {
            return new List<MpProject>
            {
                new MpProject
                {
                    AddressId = 1,
                    InitiativeId = 2,
                    LocationId = 3,
                    OrganizationId = 4,
                    ProjectId = id,
                    ProjectName = "Make Cleveland Great (Again?)",
                    ProjectStatusId = 1,
                    ProjectTypeId = 5
                }
            };
        }

    }
}