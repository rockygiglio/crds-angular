using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using crds_angular.Security;
using crds_angular.Services.Interfaces;
using Newtonsoft.Json.Linq;
using MPInterfaces = MinistryPlatform.Translation.Services.Interfaces;

namespace crds_angular.Controllers.API
{
    public class PostsController : MPAuth
    {
        private readonly MPInterfaces.IAuthenticationService _authenticationService;
        private readonly IPostsService _postsService;

        public PostsController(MPInterfaces.IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        [Route("api/posts/flag/{id}")]
        [EnableCors("*", "*", "*")]
        public IHttpActionResult Flag(string id)
        {
            try
            {
                return Ok(_postsService.FlagPost(id));
            }
            catch (Exception ex)
            {
                logger.Error("Error: Could not Flag post", ex);
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("api/posts")]
        [EnableCors("*", "*", "*")]
        public IHttpActionResult Posts([FromBody]JToken jsonbody)
        {
            return Authorized(auth =>
            {
                try
                {
                    if (!ModelState.IsValid || jsonbody == null)
                    {
                        return BadRequest();
                    }

                    return Ok(_postsService.SavePost(jsonbody, auth));
                }
                catch (UnauthorizedAccessException ex)
                {
                    logger.Error("Error: Unauthorized!", ex);
                    return Unauthorized();
                }
                catch (Exception ex)
                {
                    logger.Error("Error: Could not Save Need", ex);
                    return InternalServerError();
                }
            });
        }

        [HttpGet]
        [Route("api/posts")]
        [EnableCors("*", "*", "*")]
        public IHttpActionResult GetPosts()
        {
            try
            {
                return Ok(_postsService.GetPosts());
            }
            catch (Exception ex)
            {
                logger.Error("Error: Could not GetAllPosts", ex);
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("api/syncposts")]
        [EnableCors("*", "*", "*")]
        public IHttpActionResult SyncPosts()
        {
            try
            {
                return Ok(_postsService.SyncCloudsearchPosts());
            }
            catch (Exception ex)
            {
                logger.Error("Error: Could not Sync Posts", ex);
                return InternalServerError();
            }
        }
    }
}