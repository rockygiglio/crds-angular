using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Models.Crossroads.Attribute;
using crds_angular.Security;
using crds_angular.Services.Interfaces;
using Crossroads.ApiVersioning;

namespace crds_angular.Controllers.API
{
    public class AttributeTypeController : MPAuth
    {
        private readonly IAttributeService _attributeService;        

        public AttributeTypeController(IAttributeService attributeService)
        {
            _attributeService = attributeService;
        }

        [ResponseType(typeof (List<AttributeTypeDTO>))]
        [VersionedRoute(template: "attributeType", minimumVersion: "1.0.0")]
        [Route("attributetype")]
        public IHttpActionResult Get()
        {
            var attributeTypes = _attributeService.GetAttributeTypes(null);
            return this.Ok(attributeTypes);
        }

        [ResponseType(typeof(AttributeTypeDTO))]
        [VersionedRoute(template: "attributeType/{attributeTypeId}", minimumVersion: "1.0.0")]
        [Route("attributetype/{attributeTypeId}")]
        public IHttpActionResult Get(int attributeTypeId)
        {   
            var attributeTypes = _attributeService.GetAttributeTypes(attributeTypeId);
            return this.Ok(attributeTypes[0]);
        }
    }
}