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

        private readonly ILog _logger = LogManager.GetLogger(typeof(GroupToolService));

        public GroupToolService(
                           IGroupToolRepository groupToolRepository)
        {

            _groupToolRepository = groupToolRepository;

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
                var message = string.Format("Exception retrieving invitations for SourceID = {0}, InvitationTypeID = {1}.", sourceId, invitationTypeId);
                _logger.Error(message, e);
                throw;
            }
            return invitations;
        }

        public List<Inquiry> GetInquiries(int groupId, string token)
        {
            var requests = new List<Inquiry>();
            try
            {
                var mpRequests = _groupToolRepository.GetInquiries(groupId, token);
                mpRequests.ForEach(x => requests.Add(Mapper.Map<Inquiry>(x)));
            }
            catch (Exception e)
            {
                var message = string.Format("Exception retrieving inquiries for group id = {0}.", groupId);
                _logger.Error(message, e);
                throw;
            }
            return requests;
        }
    }
}