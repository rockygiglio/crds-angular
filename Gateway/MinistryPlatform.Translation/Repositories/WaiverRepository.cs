using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Crossroads.Web.Common.Configuration;
using Crossroads.Web.Common.Security;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Repositories.Interfaces;

namespace MinistryPlatform.Translation.Repositories
{
    public class WaiverRepository : BaseRepository,IWaiverRepository
    {
        public WaiverRepository(IAuthenticationRepository authenticationService, IConfigurationWrapper configurationWrapper) : base(authenticationService, configurationWrapper)
        {
        }

        public IObservable<MpWaivers> GetWaiver(int waiverId)
        {
            return Observable.Create<MpWaivers>(observer =>
            {
                return Disposable.Empty;
            });
        }
    }
}