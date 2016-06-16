using AutoMapper;
using crds_angular.Models.Crossroads;
using crds_angular.Services.Interfaces;
using log4net;
using MinistryPlatform.Translation.Models;
using IDonorRepository = MinistryPlatform.Translation.Services.Interfaces.IDonorRepository;

namespace crds_angular.Services
{
    public class DonorStatementService : IDonorStatementService
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof (DonorService));
        private readonly IDonorRepository _mpDonorService;


        public DonorStatementService(IDonorRepository mpDonorService)
        {
            _mpDonorService = mpDonorService;           
        }

        public DonorStatementDTO GetDonorStatement(string token)
        {            
            var mpDonorStatement = _mpDonorService.GetDonorStatement(token);            
            var donorStatement = Mapper.Map<MpDonorStatement, DonorStatementDTO>(mpDonorStatement);
            return donorStatement;
        }

        public void SaveDonorStatement(string token, DonorStatementDTO donorStatement)
        {
            var mpDonorStatement = Mapper.Map<DonorStatementDTO, MpDonorStatement>(donorStatement);
            _mpDonorService.UpdateDonorStatement(token, mpDonorStatement);
        }
    }
}