using System;
using System.Collections.Generic;
using System.Linq;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Repositories.Interfaces;
using MpAttribute = MinistryPlatform.Translation.Models.MpAttribute;

namespace MinistryPlatform.Translation.Repositories
{
    public class AttributeRepository : BaseRepository, IAttributeRepository
    {
        private readonly IMinistryPlatformService _ministryPlatformService;
            
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

            return records.Select(record => new MpAttribute
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
            }).ToList();
        }

        public void createMissingAttributes(List<MpAttribute> attributes)
        {
            List<MpAttribute> attributesToCreate = GetNewAttributes(attributes);
        }

        private List<MpAttribute> GetNewAttributes(List<MpAttribute> attributes)
        {
            var token = base.ApiLogin();
            var filter = "," + String.Join(" OR ", attributes.Select(attribute => attribute.Name.ToLower()).ToList())+",,90";

            var foundNames = _ministryPlatformService.GetPageViewRecords(2185, token, filter).Select(records => records["Attribute_Name"]).ToList();

            var missingAttributes = attributes.Where(attribute => foundNames.All(o => attribute.Name.Contains((string) o)) == false).ToList();
            return missingAttributes;
        }
    }
}