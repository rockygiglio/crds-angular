using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Crossroads.Utilities.Interfaces;
using log4net;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Repositories.Interfaces;

namespace MinistryPlatform.Translation.Repositories
{
    public class GroupToolRepository : BaseRepository, IGroupToolRepository
    {
        private readonly IConfigurationWrapper _configurationWrapper;
        private readonly ICommunicationRepository _communicationService;
        private readonly IContactRepository _contactService;
        private readonly IContentBlockService _contentBlockService;
        private readonly int _invitationPageId;
        private readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        private IMinistryPlatformService _ministryPlatformService;

        public GroupToolRepository(IMinistryPlatformService ministryPlatformService,
                               IConfigurationWrapper configurationWrapper,
                               IAuthenticationRepository authenticationService,
                               ICommunicationRepository communicationService)
            : base(authenticationService, configurationWrapper)
        {
            this._ministryPlatformService = ministryPlatformService;
            this._configurationWrapper = configurationWrapper;
            this._communicationService = communicationService;
            this._invitationPageId = _configurationWrapper.GetConfigIntValue("InvitationPageID");
        }

        public List<MpInvitation> GetInvitees(int SourceId, string token)
        {
            _ministryPlatformService.GetRecords(_invitationPageId, token, "" )
            return new List<MpInvitation>();
        }

    }
}