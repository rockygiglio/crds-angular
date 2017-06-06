using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace crds_angular.Exceptions
{
    public class GatheringException : Exception
    {
        public GatheringException(int contactId) : base($"Contact: {contactId} already has a gathering at the supplied address.") { }
    }
}