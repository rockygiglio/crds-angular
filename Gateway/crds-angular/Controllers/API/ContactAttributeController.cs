using System.Web.Http;
using crds_angular.Models.Crossroads.Profile;
using crds_angular.Security;
using crds_angular.Services.Interfaces;

namespace crds_angular.Controllers.API
{
    public class ContactAttributeController : MPAuth
    {
        private readonly IObjectAttributeService _objectAttributeService;

        public ContactAttributeController(IPersonService personService, IObjectAttributeService objectAttributeService)
        {
            _objectAttributeService = objectAttributeService;
        }
        
        [Route("api/contact/attribute/{contactId}")]
        public IHttpActionResult Post(int contactId, [FromBody] ObjectAttributeDTO objectAttribute)
        {
            return Authorized(token =>
            {                
                _objectAttributeService.SaveObjectMultiAttribute(token , contactId, objectAttribute);                
                return this.Ok();
            });
        }
    }
}