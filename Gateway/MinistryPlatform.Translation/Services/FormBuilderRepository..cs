using System;
using System.Collections.Generic;
using System.Linq;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Services.Interfaces;
using Attribute = MinistryPlatform.Translation.Models.MpAttribute;

namespace MinistryPlatform.Translation.Services
{
    public class FormBuilderRepository : BaseRepository, IFormBuilderRepository
    {
        private readonly IMinistryPlatformService _ministryPlatformService;
            
        public FormBuilderRepository(IMinistryPlatformService ministryPlatformService, IAuthenticationRepository authenticationService, IConfigurationWrapper configurationWrapper)
            : base(authenticationService, configurationWrapper)
        {
            _ministryPlatformService = ministryPlatformService;
        }

        public List<MpGroup> GetGroupsUndividedSession(int pageViewId)
        {
            var token = base.ApiLogin();
            var records = _ministryPlatformService.GetPageViewRecords(pageViewId, token);

            return records.Select(record => new MpGroup
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