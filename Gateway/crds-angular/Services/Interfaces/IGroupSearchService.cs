using System.Collections.Generic;
using crds_angular.Models.Crossroads.Groups;

namespace crds_angular.Services.Interfaces
{
    public interface IGroupSearchService
    {             
        List<GroupDTO> FindMatches(int groupTypeId, GroupParticipantDTO participant);
    }
}
