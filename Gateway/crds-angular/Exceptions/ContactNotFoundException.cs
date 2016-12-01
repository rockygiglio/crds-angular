using System;

namespace crds_angular.Exceptions
{
    public class ContactNotFoundException : Exception
    {
        public int ContactId;

        public ContactNotFoundException(int contactId) : base($"Contact {contactId} not found.")
        {
            ContactId = contactId;
        }
    }
}