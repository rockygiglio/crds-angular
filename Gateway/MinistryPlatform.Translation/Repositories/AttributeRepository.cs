﻿using System;
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
        private readonly IApiUserRepository _apiUserService;
        private readonly IMinistryPlatformRestRepository _ministryPlatformRest;

        public AttributeRepository(IMinistryPlatformService ministryPlatformService,
                                   IAuthenticationRepository authenticationService,
                                   IConfigurationWrapper configurationWrapper,
                                   IApiUserRepository apiUserService,
                                   IMinistryPlatformRestRepository ministryPlatformRest)
            : base(authenticationService, configurationWrapper)
        {
            _ministryPlatformService = ministryPlatformService;
            _ministryPlatformRest = ministryPlatformRest;
            _apiUserService = apiUserService;
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

        public List<MpAttributeCategory> GetAttributeCategory(int attributeTypeId)
        {
            //Get all categories

            //cat cols haha
            string catCols = "Attribute_Category_ID_table.*";
            string catSearch = $"attribute_type_id = {attributeTypeId}";

            return _ministryPlatformRest.UsingAuthenticationToken(_apiUserService.GetToken()).Search<MpAttribute, MpAttributeCategory>(catSearch, catCols, null, true);
        }

        public MpAttribute GetOneAttributeByCategoryId(int categoryId)
        {
            //return distinct attributes of type categoryId
            //that is active based on today and start and end date
            // this must be able to return null if there are none
            string atSearch = $"attribute_category_id = {categoryId}";

            var ret = _ministryPlatformRest.UsingAuthenticationToken(_apiUserService.GetToken()).Search<MpAttribute>(atSearch, (string)null, (string)null, true);
            return ret.FirstOrDefault();

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