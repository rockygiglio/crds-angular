using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Repositories.Interfaces;

namespace MinistryPlatform.Translation.Repositories
{
    public class ProgramRepository : BaseRepository, IProgramRepository
    {
        private readonly IMinistryPlatformService _ministryPlatformService;
        private readonly int _onlineGivingProgramsPageViewId;
        private readonly int _programsPageId;

        public ProgramRepository(IMinistryPlatformService ministryPlatformService, IAuthenticationRepository authenticationService, IConfigurationWrapper configurationWrapper)
            : base(authenticationService, configurationWrapper)
        {
            _ministryPlatformService = ministryPlatformService;
            _onlineGivingProgramsPageViewId = configurationWrapper.GetConfigIntValue("OnlineGivingProgramsPageViewId");
            _programsPageId = configurationWrapper.GetConfigIntValue("Programs");
        }

        public List<MpProgram> GetAllPrograms()
        {
            var token = ApiLogin();
            var records = _ministryPlatformService.GetPageViewRecords("AllProgramsList", token);
            var programs = new List<MpProgram>();
            if (records == null || records.Count == 0)
            {
                return programs;
            }
            programs.AddRange(records.Select(Mapper.Map<MpProgram>));

            return programs;
        }

        public List<MpProgram> GetOnlineGivingPrograms(int? programType)
        {
            var searchString = programType == null ? null : string.Format(",,,{0}", programType);
            var programs =
                WithApiLogin(
                    apiToken => (_ministryPlatformService.GetPageViewRecords(_onlineGivingProgramsPageViewId, apiToken, searchString)));

            var programList = new List<MpProgram>();
            if (programs == null || programs.Count == 0)
            {
                return programList;
            }
            programList.AddRange(programs.Select(Mapper.Map<MpProgram>));

            return programList;
        }

        public MpProgram GetProgramById(int programId)
        {
            return (WithApiLogin(token => (Mapper.Map<MpProgram>(_ministryPlatformService.GetRecordDict(_programsPageId, programId, token)))));
        }
    }
}