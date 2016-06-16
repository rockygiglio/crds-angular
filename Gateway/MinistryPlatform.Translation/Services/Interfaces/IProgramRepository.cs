using System.Collections.Generic;
using MinistryPlatform.Translation.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IProgramRepository
    {
        List<MpProgram> GetOnlineGivingPrograms(int? programType);
        MpProgram GetProgramById(int programId);
        List<MpProgram> GetAllPrograms();
    }
}