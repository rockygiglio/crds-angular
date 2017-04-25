using crds_angular.Models.Crossroads;
using Newtonsoft.Json;

namespace crds_angular.Models.Finder
{
    public class ConnectCommunicationDto
    {
        public int ConnectCommunicationsId { get; set; }
        public int FromContactId { get; set; }
        public int ToContactId { get; set; }
        public int CommunicationId { get; set; }
        public int CommunicationTypeId { get; set; }
        public int CommunicationStatusId { get; set; }
    }
}