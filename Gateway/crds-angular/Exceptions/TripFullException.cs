using System;

namespace crds_angular.Exceptions
{
    public class TripFullException : Exception
    {
        public TripFullException() : base("Trip is full")
        {
        }

    }
}