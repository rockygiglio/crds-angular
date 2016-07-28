using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Models.Crossroads.Attribute;
using crds_angular.Security;
using crds_angular.Services.Interfaces;

namespace crds_angular.Controllers.API
{
    public class AttributeController : MPAuth
    {
        private readonly IAttributeService _attributeService;

        public AttributeController(IAttributeService attributeService)
        {
            _attributeService = attributeService;
        }

        [ResponseType(typeof(int))]
        [Route("api/attribute/createorupdate")]
        public IHttpActionResult CreateOrUpdate([FromBody] List<AttributeDTO> attrList)
        {
            var status = _attributeService.CreateOrUpdateAttributes(attrList);
            return this.Ok(status);
        }

    }
}