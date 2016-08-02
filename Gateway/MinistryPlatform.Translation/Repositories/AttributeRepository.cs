using System;
using System.Collections.Generic;
using System.Linq;
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

        public List<MpAttribute> CreateMissingAttributes(List<MpAttribute> attributes, int attributeType)
        {
            var token = base.ApiLogin();

            var attributeCategories = attributes.Select(attribute => attribute.CategoryId).Distinct().ToList();

            List<string> foundNames = new List<string>();

            foreach (var category in attributeCategories)
            {
                var filter = "," + String.Join(" OR ",
                                               attributes
                                                   .Where(attCategory => attCategory.CategoryId == category)
                                                   .Select(attribute => attribute.Name.ToLower()).ToList()) + ",," + attributeType + ",,," + category;

                foundNames.AddRange(_ministryPlatformService.GetPageViewRecords(_attributesByTypePageViewId, token, filter)
                    .Select(records => records.ToString("Attribute_Name").ToLower()).ToList());
            }

            foreach (var attribute in attributes)
            {
                if (foundNames.Contains(attribute.Name.ToLower()))
                {
                    var filter = $",{attribute.Name},,,,,,{attribute.Category}";
                    attributes.First(a => a.Name == attribute.Name && a.CategoryId == attribute.CategoryId)
                        .AttributeId = _ministryPlatformService.GetPageViewRecords(_attributesByTypePageViewId, token, filter)[0].ToInt("Attribute_ID");
                }
                else
                {
                    var values = new Dictionary<string, object>
                    {
                        {"Attribute_Name", attribute.Name.ToLower()},
                        {"Attribute_Category_ID", attribute.CategoryId},
                        {"Attribute_Type_ID", attribute.AttributeTypeId},
                        {"PreventMultipleSelection", attribute.PreventMultipleSelection},
                        {"Sort_Order", attribute.SortOrder}
                    };

                    attributes.First(a => a.Name == attribute.Name && a.CategoryId == attribute.CategoryId)
                        .AttributeId = _ministryPlatformService.CreateRecord(_attributesPageId, values, token, true);
                }
            }

            return attributes;
        }
    }
}