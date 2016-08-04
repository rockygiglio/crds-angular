using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Repositories.Interfaces;

namespace MinistryPlatform.Translation.Repositories
{
    public class AttributeRepository : BaseRepository, IAttributeRepository
    {
        private readonly IMinistryPlatformService _ministryPlatformService;
        private readonly int _attributesByTypePageViewId = Convert.ToInt32(AppSettings("AttributesPageView"));
        private readonly int _attributesPageId = Convert.ToInt32(AppSettings("Attributes"));

        public AttributeRepository(IMinistryPlatformService ministryPlatformService, IAuthenticationRepository authenticationService, IConfigurationWrapper configurationWrapper)
            : base(authenticationService, configurationWrapper)
        {
            _ministryPlatformService = ministryPlatformService;
        }
        
        public List<MpAttribute> GetAttributes(int? attributeTypeId)
        {
            var token = base.ApiLogin();

            var filter = attributeTypeId.HasValue ? string.Format(",,,\"{0}\"", attributeTypeId) : string.Empty;
            var records = _ministryPlatformService.GetPageViewRecords("AttributesPageView", token, filter);

            return records.Select(MapMpAttribute).ToList();
        }

        public List<MpAttribute> GetAttributesByFilter(string filter)
        {
            var token = base.ApiLogin();
            return _ministryPlatformService.GetPageViewRecords(_attributesByTypePageViewId, token, filter).Select(MapMpAttribute).ToList();
        }

        public int CreateAttribute(MpAttribute attribute)
        {
            var token = base.ApiLogin();
            var values = new Dictionary<string, object>
                    {
                        {"Attribute_Name", attribute.Name.ToLower()},
                        {"Attribute_Category_ID", attribute.CategoryId},
                        {"Attribute_Type_ID", attribute.AttributeTypeId},
                        {"PreventMultipleSelection", attribute.PreventMultipleSelection},
                        {"Sort_Order", attribute.SortOrder}
                    };

            return _ministryPlatformService.CreateRecord(_attributesPageId, values, token, true);
        }


        private MpAttribute MapMpAttribute(Dictionary<string, object> record)
        {
            return new MpAttribute
            {

                AttributeId = record.ToInt("Attribute_ID"),
                Name = record.ToString("Attribute_Name"),
                Description = record.ToString("Attribute_Description"),
                CategoryId = record.ToNullableInt("Attribute_Category_ID"),
                Category = record.ToString("Attribute_Category"),
                CategoryDescription = record.ToString("Attribute_Category_Description"),
                AttributeTypeId = record.ToInt("Attribute_Type_ID"),
                AttributeTypeName = record.ToString("Attribute_Type"),
                PreventMultipleSelection = record.ToBool("Prevent_Multiple_Selection"),
                SortOrder = record.ToInt("Sort_Order")
            };
        }
    }
}