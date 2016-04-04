using System.Collections.Generic;
using MinistryPlatform.Translation.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface ISkillsService
    {
        List<MpGoVolunteerSkill> GetGoVolunteerSkills(string token);
    }    
}