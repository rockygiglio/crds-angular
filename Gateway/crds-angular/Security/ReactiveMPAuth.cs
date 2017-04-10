using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using crds_angular.Services.Interfaces;
using crds_angular.Util;
using Crossroads.Web.Common.Security;
using log4net;

namespace crds_angular.Security
{
    public class ReactiveMPAuth : ApiController
    {

        protected readonly log4net.ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IUserImpersonationService _userImpersonationService;
        protected readonly IAuthenticationRepository AuthenticationRepository;

        public ReactiveMPAuth(IUserImpersonationService userImpersonationService, IAuthenticationRepository authenticationRepository)
        {
            _userImpersonationService = userImpersonationService;
            AuthenticationRepository = authenticationRepository;
        }

        /// <summary>
        /// Ensure that a user is authenticated before executing the given lambda expression.  The expression will
        /// have a reference to the user's authentication token (the value of the "Authorization" cookie).  If
        /// the user is not authenticated, an UnauthorizedResult Observable will be returned.
        /// </summary>
        /// <param name="doIt">A lambda expression to execute if the user is authenticated</param>
        /// <returns>An Observable<IHttpActionResult> from the "doIt" expression, or UnauthorizedResult if the user is not authenticated.</returns>
        protected async Task<IHttpActionResult> Authorized(Func<string, IHttpActionResult> doIt)
        {
            return await (Authorized(doIt, () => { return Unauthorized(); }));
        }

        protected async Task<IHttpActionResult> Authorized(Func<string, IHttpActionResult> actionWhenAuthorized, Func<IHttpActionResult> actionWhenNotAuthorized)
        {
            try
            {
                IEnumerable<string> refreshTokens;
                IEnumerable<string> impersonateUserIds;
                bool impersonate = false;
                var authorized = "";

                if (Request.Headers.TryGetValues("ImpersonateUserId", out impersonateUserIds) && impersonateUserIds.Any())
                {
                    impersonate = true;
                }

                if (Request.Headers.TryGetValues("RefreshToken", out refreshTokens) && refreshTokens.Any())
                {
                    var authData = AuthenticationRepository.RefreshToken(refreshTokens.FirstOrDefault());
                    if (authData != null)
                    {
                        authorized = authData.AccessToken;
                        var refreshToken = authData.RefreshToken;
                        IHttpActionResult result = null;
                        if (impersonate)
                        {
                            result =
                                new HttpAuthResult(
                                    _userImpersonationService.WithImpersonation(authorized, impersonateUserIds.FirstOrDefault(), () => actionWhenAuthorized(authorized)),
                                    authorized,
                                    refreshToken);
                        }
                        else
                        {
                            result = new HttpAuthResult(actionWhenAuthorized(authorized), authorized, refreshToken);
                        }
                        return await Observable.Return(result);
                    }
                }

                authorized = Request.Headers.GetValues("Authorization").FirstOrDefault();
                if (authorized != null && (authorized != "null" || authorized != ""))
                {
                    if (impersonate)
                    {
                        return await Observable.Return(_userImpersonationService.WithImpersonation(authorized, impersonateUserIds.FirstOrDefault(), () => actionWhenAuthorized(authorized)));
                    }
                    else
                    {
                        return await Observable.Return( actionWhenAuthorized(authorized) );
                    }
                }
                return await Observable.Return(actionWhenNotAuthorized());
            }
            catch (System.InvalidOperationException e)
            {
                return await Observable.Return(actionWhenNotAuthorized());
            }
        }
    }
}