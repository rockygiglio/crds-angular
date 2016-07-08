using System;

namespace crds_angular.Exceptions
{
    public class NotHeadOfHouseholdException : Exception
    {
        public NotHeadOfHouseholdException(int contactId) : base(string.Format("Contact {0} is not a Head of Household", contactId)) { }
    }
}