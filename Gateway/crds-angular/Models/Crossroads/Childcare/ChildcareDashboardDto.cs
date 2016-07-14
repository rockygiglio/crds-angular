using System.Collections.Generic;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Childcare
{
    public class ChildcareDashboardDto
    {
        [JsonProperty(PropertyName = "childcareDates")]
        public List<ChildCareDate> AvailableChildcareDates { get; set; }

        public ChildcareDashboardDto()
        {
            AvailableChildcareDates = new List<ChildCareDate>();
        }
    }
}