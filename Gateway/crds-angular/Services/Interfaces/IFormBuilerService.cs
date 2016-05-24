using System.Collections.Generic;
using System.Web.Http;
using crds_angular.Models.Crossroads.Groups;

namespace crds_angular.Services.Interfaces
{
    public interface IFormBuilderService
    {
        List<GroupDTO> GetGroupsUndivided(string groupType);
    }
}
