﻿using System;
using System.Collections.Generic;
using crds_angular.Models.Crossroads.Groups;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Interfaces;
using log4net;
using MinistryPlatform.Translation.Repositories.Interfaces;
using System.Linq;
using AutoMapper;
using Crossroads.Web.Common;
using Crossroads.Web.Common.Configuration;
using MinistryPlatform.Translation.Models;
using IFormBuilderRepository = MinistryPlatform.Translation.Repositories.Interfaces.IFormBuilderRepository;

namespace crds_angular.Services
{
    public class FormBuilderService : crds_angular.Services.Interfaces.IFormBuilderService
    {
        private readonly ILog logger = LogManager.GetLogger(typeof (FormBuilderService));
        private readonly IFormBuilderRepository _mpFormBuilderService;
        private readonly IConfigurationWrapper _configurationWrapper;

        private readonly int _undividedGroupsPageViewId;

        public FormBuilderService(IFormBuilderRepository mpFormBuilderService,                
                                  IConfigurationWrapper configurationWrapper)
        {
            _mpFormBuilderService = mpFormBuilderService;
            _configurationWrapper = configurationWrapper;
            _undividedGroupsPageViewId = configurationWrapper.GetConfigIntValue("UndividedGroupsPageViewId");
        }

        public List<GroupDTO> GetGroupsUndivided(string templateType)
        {
            var pageViewId = GetPageViewId(templateType);
            var groupList = _mpFormBuilderService.GetGroupsUndividedSession(pageViewId);
            return groupList.Select(Mapper.Map<MpGroup, GroupDTO>).ToList();
        }

        private int GetPageViewId(string templateType)
        {
            switch (templateType)
            {
                case "GroupsUndivided":
                    return _undividedGroupsPageViewId;
                default:
                    var message = String.Format("Could not find matching template {0}", templateType);
                    logger.Error(message);
                    throw new ApplicationException();
            }
        }

    }
}
