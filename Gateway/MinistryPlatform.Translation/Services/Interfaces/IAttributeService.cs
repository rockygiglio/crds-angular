using System.Collections.Generic;
using MinistryPlatform.Translation.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IAttributeService
    {
        List<Attribute> GetAttributes(int? attributeTypeId);
    }
}