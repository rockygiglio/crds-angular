using System;

namespace crds_angular.Exceptions
{
    public class DuplicateUserException : Exception
    {
        public DuplicateUserException(string userName) : base($"User {userName} already exists") { }
    }
}