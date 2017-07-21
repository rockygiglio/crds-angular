using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Crossroads.Web.Common.Configuration;
using Crossroads.Web.Common.MinistryPlatform;
using Crossroads.Web.Common.Security;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Repositories.Interfaces;

namespace MinistryPlatform.Translation.Repositories
{
    public class WaiverRepository : BaseRepository,IWaiverRepository
    {

        private readonly IMinistryPlatformRestRepository _ministryPlatformRestRepository;

        public WaiverRepository(IMinistryPlatformRestRepository ministryPlatformRestRepository, IAuthenticationRepository authenticationService, IConfigurationWrapper configurationWrapper) : base(authenticationService, configurationWrapper)
        {
            _ministryPlatformRestRepository = ministryPlatformRestRepository;
        }

        public IObservable<MpWaivers> GetWaiver(int waiverId)
        {
            return Observable.Create<MpWaivers>(observer =>
            {
                try
                {
                    var apiToken = ApiLogin();
                    var res = _ministryPlatformRestRepository.UsingAuthenticationToken(apiToken).Get<MpWaivers>(waiverId);
                    observer.OnNext(res);
                    observer.OnCompleted();
                }
                catch (Exception e)
                {
                    observer.OnError(e);
                }               
                return Disposable.Empty;
            });
        }
    }
}