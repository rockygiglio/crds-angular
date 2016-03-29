using System.Collections.Generic;
using crds_angular.Models.Crossroads.GoVolunteer;

namespace crds_angular.Services.Interfaces
{
    public interface IGoEquipmentService
    {
        List<GoEquipment> RetrieveGoEquipment();
    }
}