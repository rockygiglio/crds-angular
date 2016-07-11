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
    public class InvitationService : MinistryPlatformBaseService, IInvitationService
    {

        private readonly IInvitationRepository _invitationRepository;
        private readonly ICommunicationRepository _communicationService;

        private readonly ILog _logger = LogManager.GetLogger(typeof(GroupToolService));

        public InvitationService(
                           IInvitationRepository invitationRepository,
                           ICommunicationRepository communicationService)
        {

            _invitationRepository = invitationRepository;
            _communicationService = communicationService;

        }

        public int CreateInvitation(Invitation dto, string token)
        {
            try
            {
                var mpGroupInvitation = Mapper.Map<MpInvitation>(dto);

                return _invitationRepository.CreateInvitation(mpGroupInvitation, token);

            }
            catch (Exception e)
            {
                var message = string.Format("Exception creating invitation for {0}, SourceID = {1}.", dto.RecipientName, dto.SourceId);
                _logger.Error(message, e);
                return -1;
            }
        }
    }
}