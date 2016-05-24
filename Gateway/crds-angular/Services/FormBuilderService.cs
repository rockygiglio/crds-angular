using System;
using System.Collections.Generic;
using crds_angular.Models.Crossroads.Groups;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Interfaces;
using log4net;
using MinistryPlatform.Translation.Services.Interfaces;
using System.Linq;
using AutoMapper;
using MinistryPlatform.Models;
using IFormBuilderService = MinistryPlatform.Translation.Services.Interfaces.IFormBuilderService;

namespace crds_angular.Services
{
    public class FormBuilderService : crds_angular.Services.Interfaces.IFormBuilderService
    {
        private readonly ILog logger = LogManager.GetLogger(typeof (FormBuilderService));
        private readonly IFormBuilderService _mpFormBuilderService;
        private readonly IConfigurationWrapper _configurationWrapper;

        private readonly int _undividedGroupsPageViewId;

        public FormBuilderService(IFormBuilderService mpFormBuilderService,                
                                  IConfigurationWrapper configurationWrapper)
        {
            _mpFormBuilderService = mpFormBuilderService;
            _configurationWrapper = configurationWrapper;
            _undividedGroupsPageViewId = configurationWrapper.GetConfigIntValue("UndividedGroupsPageViewId");
        }

        public List<GroupDTO> GetGroupsUndivided(string templateType)
        {
            int pageViewId;
            GroupDTO groups;

            switch (templateType)
            {
                case "GroupsUndivided":
                    pageViewId = _undividedGroupsPageViewId;
                    break;
                default:
                    throw new Exception();
                    break;
            }

            var groupList = _mpFormBuilderService.GetGroupsUndividedSession(pageViewId);
            var groupDTOList = groupList.Select(Mapper.Map<Group, GroupDTO>).ToList();
            return groupDTOList;
        }

    }
}
