using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crossroads.Utilities.Interfaces;
using log4net;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services.GoCincinnati
{
    public class GroupConnectorService : BaseService
    {
        private readonly IMinistryPlatformService _ministryPlatformService;


        private readonly ILog _logger = LogManager.GetLogger(typeof(RoomService));

        public GroupConnectorService(IMinistryPlatformService ministryPlatformService, IAuthenticationService authenticationService, IConfigurationWrapper configuration)
            : base(authenticationService, configuration)
        {
            _ministryPlatformService = ministryPlatformService;
        }

        public int CreateGroupConnector()
        {
            
        }
    }
}
