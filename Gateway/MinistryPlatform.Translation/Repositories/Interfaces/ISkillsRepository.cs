using System.Collections.Generic;
using MinistryPlatform.Translation.Models;

namespace MinistryPlatform.Translation.Repositories.Interfaces
{
    public interface ISkillsRepository
    {
        List<MpGoVolunteerSkill> GetGoVolunteerSkills(string token);
    }    
}