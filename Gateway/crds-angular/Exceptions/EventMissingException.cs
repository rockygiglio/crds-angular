using System;
using System.Collections.Generic;

namespace crds_angular.Exceptions
{
    public class EventMissingException : Exception
    {
        public List<DateTime> MissingDateTimes { get; }

        public EventMissingException(List<DateTime> events) : base("There are childcare events missing.")
        {
            this.MissingDateTimes = events;
        }
    }

}