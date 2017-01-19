using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Models.Crossroads.Attribute;
using crds_angular.Security;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Repositories.Interfaces;

namespace crds_angular.Controllers.API
{
    public class GroupAttributeController :MPAuth
    {
        private readonly IObjectAttributeService _objectAttributeService;
        private readonly IAttributeRepository _attributeRepository;

        public GroupAttributeController(IObjectAttributeService objectAttributeService, IAttributeRepository attributeRepository)
        {
            _objectAttributeService = objectAttributeService;
            _attributeRepository = attributeRepository;
        }

        [Route("api/attributes/group/{groupId}"), HttpGet]
        [RequiresAuthorization]
        [ResponseType(typeof(ObjectAllAttributesDTO))]
        public IHttpActionResult GetGroupAttributes([FromUri] int groupId)
        {
            //var filter = new List<MpAttribute>
            //{
            //    new MpAttribute
            //    {
            //        AttributeTypeId = 90
            //    }
            //};
            var filter = _attributeRepository.GetAttributes(90);
            return Authorized(token =>
            {
                var result = _objectAttributeService.GetObjectAttributes(token, groupId, MpObjectAttributeConfigurationFactory.Group(), filter);
                return Ok(result);
            });
        }
    }
}