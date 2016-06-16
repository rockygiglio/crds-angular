using System.Collections.Generic;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Models.DTO;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IUserRepository
    {
        MpUser GetByUserId(string userId);
        MpUser GetByAuthenticationToken(string authToken);
        void UpdateUser(Dictionary<string, object> userUpdateValues);
        void UpdateUser(MpUser user);
        int GetUserIdByUsername(string username);
        int GetContactIdByUserId(int userId);
        MpUser GetUserByResetToken(string resetToken);
        List<MpRoleDto> GetUserRoles(int userId);

        MpUser GetUserByRecordId(int recordId);
    }
}
