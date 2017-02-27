using System;
using System.Collections.Generic;
using System.Linq;
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
            var filter = $"Project_ID={projectId} AND Initiative_ID_Table.[Volunteer_Signup_Start_Date]<=GetDate() AND Initiative_ID_Table.[Volunteer_Signup_End_Date]>=GetDate()";
            var columns = new List<string>
            {
                "Project_ID",
                "Project_Name",
                "Project_Type_ID_Table.[Description]",
                "Project_Status_ID",
                "Location_ID",
                "Project_Type_ID_Table.Project_Type_ID",
                "Organization_ID",
                "cr_Projects.Initiative_ID",
                "Address_ID_Table.Address_ID",
                "Address_ID_Table.[City]",
                "Address_ID_Table.[State/Region] AS [State]"
            };
            try
            {
                var result = _ministryPlatformRest.UsingAuthenticationToken(token).Search<MpProject>(filter, columns, null, true);
                if (result.Count > 0)
                {   
                    // There should never be more than one value.
                    return new Ok<MpProject>(result.First());
                }
                return new Err<MpProject>($"Unable to find a valid project with Id = {projectId}");
            }
            catch (Exception e)
            {
                return new Err<MpProject>(e);
            }
        }

        public Result<MpGroupConnector> GetGroupConnector(int projectId, string token)
        {
            var filter = $"Project_ID_Table.Project_ID={projectId}";
            var columns = new List<string>
            {
                "cr_Group_Connectors.[Group_Connector_ID]",
                "Primary_Registration_Table_Participant_ID_Table_Contact_ID_Table.[Contact_ID]",
                "Primary_Registration_Table_Participant_ID_Table_Contact_ID_Table.[First_Name]",
                "Primary_Registration_Table_Participant_ID_Table_Contact_ID_Table.[Last_Name]",
                "Primary_Registration_Table_Participant_ID_Table_Contact_ID_Table.[Nickname]",
                "Primary_Registration_Table_Participant_ID_Table_Contact_ID_Table.[Email_Address]"
            };

            var result = _ministryPlatformRest.UsingAuthenticationToken(token).Search<MpGroupConnector>(filter, columns, null, false);

            // lets take to first result and run with it ¯\_(ツ)_/¯
            if (result == null) return new Err<MpGroupConnector>($"No group connectors found with for projectId = {projectId}");
            if (result.First() == null) return new Err<MpGroupConnector>($"No group connectors found with for projectId = {projectId}");

            return new Ok<MpGroupConnector>(result.First());
        }

        public List<MpProject> GetProjectsByInitiative(int initiativeId, string token)
        {
            var filter = $"Initiative_ID={initiativeId} AND Initiative_ID_Table.[Volunteer_Signup_Start_Date]<=GetDate() AND Initiative_ID_Table.[Volunteer_Signup_End_Date]>=GetDate()";
            var columns = new List<string>
            {
                "Project_ID",
                "Project_Name",
                "Project_Type_ID_Table.[Description]",
                "Project_Status_ID",
                "Location_ID",
                "Project_Type_ID_Table.Project_Type_ID",
                "Organization_ID",
                "cr_Projects.Initiative_ID",
                "Address_ID_Table.Address_ID",
                "Address_ID_Table.[City]",
                "Address_ID_Table.[State/Region] AS [State]"
            };
            try
            {
                var result = _ministryPlatformRest.UsingAuthenticationToken(token).Search<MpProject>(filter, columns, null, true);
                return result ?? new List<MpProject>();
            }
            catch (Exception e)
            {
                throw new ApplicationException($"GetProjectsByInitiative failed. InitiativeId: {initiativeId}", e);
            }
        }
    }
}