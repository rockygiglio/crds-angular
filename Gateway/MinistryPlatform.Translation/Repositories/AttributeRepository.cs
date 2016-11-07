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
        private readonly IMinistryPlatformRestRepository _ministryPlatformRestService;
        private readonly int _attributesByTypePageViewId = Convert.ToInt32(AppSettings("AttributesPageView"));
        private readonly int _attributesPageId = Convert.ToInt32(AppSettings("Attributes"));

        public AttributeRepository(IMinistryPlatformService ministryPlatformService, IAuthenticationRepository authenticationService, IConfigurationWrapper configurationWrapper,
                                    IMinistryPlatformRestRepository ministryPlatformRestRepository)
            : base(authenticationService, configurationWrapper)
        {
            _ministryPlatformService = ministryPlatformService;
            _ministryPlatformRestService = ministryPlatformRestRepository;
        }
        
        public List<MpAttribute> GetAttributes(int? attributeTypeId)
        {
            var token = base.ApiLogin();
            const string COLUMNS =
                "Attribute_ID, Attribute_Name, Attribute_Category_ID_Table.Attribute_Category, Attributes.Attribute_Category_ID, Attribute_Category_ID_Table.Description as Attribute_Category_Description, Attributes.Sort_Order, Attribute_Type_ID_Table.Prevent_Multiple_Selection";

            var search = attributeTypeId.HasValue ? $"Attributes.Attribute_Type_ID = { attributeTypeId}  AND " : string.Empty;
            search += $"(Attributes.Start_Date Is Null OR Attributes.Start_Date <= GetDate()) ";
            search += $"AND (Attributes.End_Date Is Null OR Attributes.End_Date >= GetDate())";
            var records = _ministryPlatformRestService.UsingAuthenticationToken(token).Search<MpAttribute>(search, COLUMNS);
            return records;
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
    }
}