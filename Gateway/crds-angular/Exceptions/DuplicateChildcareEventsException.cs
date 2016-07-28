using System;
namespace crds_angular.Exceptions
{
    public class DuplicateChildcareEventsException : Exception
    {
        public DateTime RequestedDate;
        public DuplicateChildcareEventsException(DateTime date) : base(string.Format("Duplicate Events found for the childcare dates"))
        {
           this.RequestedDate = date;
        }
        
    }
} 