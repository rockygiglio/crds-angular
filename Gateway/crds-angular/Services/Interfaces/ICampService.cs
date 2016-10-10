using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using crds_angular.Models.Crossroads.Camp;

namespace crds_angular.Services.Interfaces
{
    public interface ICampService
    {
        CampDTO GetCampEventDetails(int cotactId, int eventId);
    }
}
