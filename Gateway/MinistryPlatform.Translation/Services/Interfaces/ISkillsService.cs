using System.Collections.Generic;
using MinistryPlatform.Translation.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface ISkillsService
    {
        List<MPGoVolunteerSkill> GetGoVolunteerSkills(string token);
    }    
}