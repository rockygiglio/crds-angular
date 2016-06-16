using System;
using System.Collections.Generic;
using MinistryPlatform.Translation.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IObjectAttributeRepository
    {
        List<MpObjectAttribute> GetCurrentObjectAttributes(string token, int objectId, MpObjectAttributeConfiguration configuration, int? attributeTypeIdFilter = null);
        int CreateAttribute(string token, int objectId, MpObjectAttribute attribute, MpObjectAttributeConfiguration configuration);
        //void CreateAttributeAsync(string token, int objectId, ObjectAttribute attribute, ObjectAttributeConfiguration configuration);
        IObservable<int> CreateAttributeAsync(string token, int objectId, MpObjectAttribute attribute, MpObjectAttributeConfiguration configuration); 
        void UpdateAttribute(string token, MpObjectAttribute attribute, MpObjectAttributeConfiguration configuration);
        void UpdateAttributeAsync(string token, MpObjectAttribute attribute, MpObjectAttributeConfiguration configuration);
    }
}