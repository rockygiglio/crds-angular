using System.Collections.Generic;
using MinistryPlatform.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IObjectAttributeService
    {
        List<ObjectAttribute> GetCurrentObjectAttributes(string token, int objectId, ObjectAttributeConfiguration configuration, int? attributeTypeIdFilter = null);
        int CreateAttribute(string token, int objectId, ObjectAttribute attribute, ObjectAttributeConfiguration configuration);
        void UpdateAttribute(string token, ObjectAttribute attribute, ObjectAttributeConfiguration configuration);
    }
}