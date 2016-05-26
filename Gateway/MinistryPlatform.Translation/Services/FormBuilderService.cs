using System;
using System.Collections.Generic;
using System.Linq;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Services.Interfaces;
using Attribute = MinistryPlatform.Models.Attribute;

namespace MinistryPlatform.Translation.Services
{
    public class FormBuilderService : BaseService, IFormBuilderService
    {
        private readonly IMinistryPlatformService _ministryPlatformService;
            
        public FormBuilderService(IMinistryPlatformService ministryPlatformService, IAuthenticationService authenticationService, IConfigurationWrapper configurationWrapper)
            : base(authenticationService, configurationWrapper)
        {
            _ministryPlatformService = ministryPlatformService;
        }

        public List<Group> GetGroupsUndividedSession(int pageViewId)
        {
            var token = base.ApiLogin();
            var records = _ministryPlatformService.GetPageViewRecords(pageViewId, token);

            return records.Select(record => new Group
            {

                GroupId = record.ToInt("dp_RecordID"),
                GroupDescription = record.ToString("Description"),                
                Name = record.ToString("Group_Name"),
                CongregationId = record.ToInt("Congregation_ID"),
                StartDate = record.ToDate("Start_Date"),
                EndDate = record.ToDate("End_Date")
            }).ToList();
        }
    }
}