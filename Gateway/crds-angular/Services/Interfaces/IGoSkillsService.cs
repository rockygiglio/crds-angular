using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using crds_angular.Models.Crossroads.GoVolunteer;

namespace crds_angular.Services.Interfaces
{
    public interface IGoSkillsService
    {
        List<GoSkills> RetrieveGoSkills(string token);
    }

}
