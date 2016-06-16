using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class ApiUserRepository : IApiUserService
    {
        private readonly IConfigurationWrapper _configurationWrapper;
        private readonly IAuthenticationService _authenticationService;

        public ApiUserRepository(IConfigurationWrapper configurationWrapper, IAuthenticationService authenticationService)
        {
            _configurationWrapper = configurationWrapper;
            _authenticationService = authenticationService;
        }

        public string GetToken()
        {
            var apiUser = _configurationWrapper.GetEnvironmentVarAsString("API_USER");
            var apiPasword = _configurationWrapper.GetEnvironmentVarAsString("API_PASSWORD");
            var authData = _authenticationService.Authenticate(apiUser, apiPasword);
            var token = authData["token"].ToString();

            return (token);
        }
    }
}