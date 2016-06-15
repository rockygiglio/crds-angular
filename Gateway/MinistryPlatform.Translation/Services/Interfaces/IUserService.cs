using System.Collections.Generic;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Models.DTO;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IUserService
    {
        MinistryPlatformUser GetByUserId(string userId);
        MinistryPlatformUser GetByAuthenticationToken(string authToken);
        void UpdateUser(Dictionary<string, object> userUpdateValues);
        void UpdateUser(MinistryPlatformUser user);
        int GetUserIdByUsername(string username);
        int GetContactIdByUserId(int userId);
        MinistryPlatformUser GetUserByResetToken(string resetToken);
        List<RoleDto> GetUserRoles(int userId);

        MinistryPlatformUser GetUserByRecordId(int recordId);
    }
}
