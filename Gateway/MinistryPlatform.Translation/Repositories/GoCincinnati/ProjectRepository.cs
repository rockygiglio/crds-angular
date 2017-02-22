using System.Collections.Generic;
using Crossroads.Utilities.FunctionalHelpers;
using Crossroads.Web.Common.MinistryPlatform;
using MinistryPlatform.Translation.Models.GoCincinnati;
using MinistryPlatform.Translation.Repositories.Interfaces.GoCincinnati;

namespace MinistryPlatform.Translation.Repositories.GoCincinnati
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly IMinistryPlatformRestRepository _ministryPlatformRest;

        public ProjectRepository(IMinistryPlatformRestRepository ministryPlatformRestRepository)
        {
            _ministryPlatformRest = ministryPlatformRestRepository;
        }

        public Result<MpProject> GetProject(int projectId, string token)
        {
            const string filter = "Project_ID=564 AND Initiative_ID_Table.[Volunteer_Signup_Start_Date]<=GetDate() AND Initiative_ID_Table.[Volunteer_Signup_End_Date]>=GetDate()";
            var columns = new List<string>
            {
                "Project_ID",
                "Project_Name",
                "Project_Status_ID",
                "Location_ID",
                "Project_Type_ID",
                "Organization_ID",
                "cr_Projects.Initiative_ID",
                "Address_ID"
            };
        }
    }
}