using System.Collections.Generic;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Repositories.Interfaces;

namespace MinistryPlatform.Translation.Repositories
{
    public class DestinationRepository : BaseRepository, IDestinationRepository
    {
        private readonly IMinistryPlatformService _ministryPlatformService;

        public DestinationRepository(IMinistryPlatformService ministryPlatformService, IAuthenticationRepository authenticationService, IConfigurationWrapper configurationWrapper)
            : base(authenticationService, configurationWrapper)
        {
            _ministryPlatformService = ministryPlatformService;
        }

        public List<MpTripDocuments> DocumentsForDestination(int destinationId)
        {
            var token = ApiLogin();
            var searchString = string.Format(",{0}", destinationId);
            var records = _ministryPlatformService.GetPageViewRecords("TripDestinationDocuments", token, searchString);

            var documents = new List<MpTripDocuments>();
            foreach (var record in records)
            {
                var d = new MpTripDocuments();
                d.Description = record.ToString("Description");
                d.Document = record.ToString("Document");
                d.DocumentId = record.ToInt("Document_ID");
                documents.Add(d);
            }

            return documents;
        }
    }
}