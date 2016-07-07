using System;
using System.Collections.Generic;
using AutoMapper;
using crds_angular.Models.Crossroads;
using crds_angular.Services.Interfaces;
using log4net;
using MinistryPlatform.Translation.Repositories.Interfaces;

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

        public List<Invitation> GetInvitations(int sourceId, int invitationTypeId, string token)
        {
            var invitations = new List<Invitation>();
            try
            {
                var mpInvitations = _groupToolRepository.GetInvitations(sourceId, invitationTypeId, token);
                mpInvitations.ForEach(x => invitations.Add(Mapper.Map<Invitation>(x)));
            }
            catch (Exception e)
            {
                var message = $"Exception retrieving invitations for SourceID = {sourceId}, InvitationTypeID = {invitationTypeId}.";
                _logger.Error(message, e);
                throw;
            }
            return invitations;
        }
    }
}