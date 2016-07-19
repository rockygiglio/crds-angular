using System;
using System.Net;

namespace crds_angular.Exceptions
{
    public class GroupParticipantRemovalException : Exception
    {
        public HttpStatusCode StatusCode { get; protected set; }

        public GroupParticipantRemovalException(string message) : base(message)
        {
            StatusCode = HttpStatusCode.InternalServerError;
        }

        public GroupParticipantRemovalException(string message, Exception innerException) : base(message, innerException)
        {
            StatusCode = HttpStatusCode.InternalServerError;
        }
    }

    public class GroupNotFoundForParticipantException : GroupParticipantRemovalException
    {
        public GroupNotFoundForParticipantException(string message) : base(message) { StatusCode = HttpStatusCode.NotFound; }
    }

    public class NotGroupLeaderException : GroupParticipantRemovalException
    {
        public NotGroupLeaderException(string message) : base(message) { StatusCode = HttpStatusCode.Forbidden; }
    }
}