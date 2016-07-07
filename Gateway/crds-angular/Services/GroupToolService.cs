using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using crds_angular.Models.Crossroads;
using crds_angular.Models.Crossroads.Trip;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Extensions;
using Crossroads.Utilities.Interfaces;
using Crossroads.Utilities.Services;
using log4net;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Repositories.Interfaces;
using MpEvent = MinistryPlatform.Translation.Models.MpEvent;

namespace crds_angular.Services
{
    public class GroupToolService : MinistryPlatformBaseService, IGroupToolService
    {

        private readonly IGroupToolRepository _groupToolRepository;
        private readonly ICommunicationRepository _communicationService;

        private readonly ILog _logger = LogManager.GetLogger(typeof(GroupToolService));

        public GroupToolService(
                           IGroupToolRepository groupToolRepository,
                           ICommunicationRepository communicationService)
        {

            _groupToolRepository = groupToolRepository;
            _communicationService = communicationService;

        }

        public List<Invitation> GetInvitations(int SourceId, int InvitationTypeId, string token)
        {
            var invitations = new List<Invitation>();
            try
            {
                var mpInvitations = _groupToolRepository.GetInvitations(SourceId, InvitationTypeId, token);
                mpInvitations.ForEach(x => invitations.Add(Mapper.Map<Invitation>(x)));
            }
            catch (Exception e)
            {
                var message = $"Exception retrieving invitations for SourceID = {SourceId}, InvitationTypeID = {InvitationTypeId}.";
                _logger.Error(message, e);
                throw;
            }
            return invitations;
        }
    }
}