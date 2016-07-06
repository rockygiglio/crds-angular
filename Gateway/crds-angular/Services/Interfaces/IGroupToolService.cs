using crds_angular.Models.Crossroads;
using System.Collections.Generic;
using MinistryPlatform.Translation.Models;

namespace crds_angular.Services.Interfaces
{
    public interface IGroupToolService
    {
        List<Invitation> GetInvitees(int SourceId, string token);
    }
}
