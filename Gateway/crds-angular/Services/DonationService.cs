﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using crds_angular.Models.Crossroads.Stewardship;
using MPServices=MinistryPlatform.Translation.Services.Interfaces;
using crds_angular.Services.Interfaces;
using crds_angular.Util;
using MinistryPlatform.Models;
using Newtonsoft.Json;

namespace crds_angular.Services
{
    public class DonationService: IDonationService
    {
        private readonly MPServices.IDonationService _mpDonationService;
        private readonly MPServices.IDonorService _mpDonorService;
        private readonly MPServices.IAuthenticationService _mpAuthenticationService;

        public DonationService(MPServices.IDonationService mpDonationService, MPServices.IDonorService mpDonorService, MPServices.IAuthenticationService mpAuthenticationService)
        {
            _mpDonationService = mpDonationService;
            _mpDonorService = mpDonorService;
            _mpAuthenticationService = mpAuthenticationService;
        }

        public DonationDTO GetDonationByProcessorPaymentId(string processorPaymentId)
        {
            var d = _mpDonationService.GetDonationByProcessorPaymentId(processorPaymentId);
            if (d == null)
            {
                return (null);
            }

            var donation = new DonationDTO
            {
                Amount = d.donationAmt,
                Id = d.donationId + "",
                BatchId = d.batchId
            };
            return (donation);
        }

        public int UpdateDonationStatus(int donationId, int statusId, DateTime? statusDate, string statusNote = null)
        {
            return(_mpDonationService.UpdateDonationStatus(donationId, statusId, statusDate ?? DateTime.Now, statusNote));
        }

        public int UpdateDonationStatus(string processorPaymentId, int statusId, DateTime? statusDate, string statusNote = null)
        {
            return(_mpDonationService.UpdateDonationStatus(processorPaymentId, statusId, statusDate ?? DateTime.Now, statusNote));
        }

        public DonationBatchDTO CreateDonationBatch(DonationBatchDTO batch)
        {
            var batchId = _mpDonationService.CreateDonationBatch(batch.BatchName, batch.SetupDateTime, batch.BatchTotalAmount,batch.ItemCount, batch.BatchEntryType, batch.DepositId, batch.FinalizedDateTime, batch.ProcessorTransferId);

            batch.Id = batchId;

            foreach (var donation in batch.Donations)
            {
                _mpDonationService.AddDonationToBatch(batchId, int.Parse(donation.Id));
            }

            return (batch);
        }

        public DonationBatchDTO GetDonationBatchByProcessorTransferId(string processorTransferId)
        {
            return (Mapper.Map<DonationBatch, DonationBatchDTO>(_mpDonationService.GetDonationBatchByProcessorTransferId(processorTransferId)));
        }

        public DonationBatchDTO GetDonationBatch(int batchId)
        {
            return (Mapper.Map<DonationBatch, DonationBatchDTO>(_mpDonationService.GetDonationBatch(batchId)));
        }

        public List<DonationDTO> GetDonationsForAuthenticatedUser(string userToken, string donationYear = null, bool softCredit = false)
        {
            var donorId = GetDonorIdForAuthenticatedUser(userToken);
            return (donorId == null ? null : GetDonationsForDonor(donorId.Value));
        }

        public List<string> GetDonationYearsForAuthenticatedUser(string userToken)
        {
            var donorId = GetDonorIdForAuthenticatedUser(userToken);
            return (donorId == null ? null : GetDonationYearsForDonor(donorId.Value));
        }

        private int? GetDonorIdForAuthenticatedUser(string userToken)
        {
            var donor = _mpDonorService.GetContactDonor(_mpAuthenticationService.GetContactId(userToken));
            return (donor != null && donor.ExistingDonor ? donor.DonorId : (int?)null);
        }

        public List<DonationDTO> GetDonationsForDonor(int donorId, string donationYear = null, bool softCredit = false)
        {
            var donations = softCredit ? _mpDonorService.GetSoftCreditDonations(donorId) : _mpDonorService.GetDonations(donorId);
            if (donations == null || donations.Count == 0)
            {
                return (null);
            }

            var response = donations.Select(Mapper.Map<DonationDTO>).ToList();

            foreach (var donation in response)
            {
                switch (donation.SourceType)
                {
                    case PaymentType.Cash:
                        donation.SourceTypeDescription = "cash";
                        break;

                    case PaymentType.CreditCard:
                        // TODO Need to lookup info at stripe
                        donation.CardType = CreditCardType.Visa;
                        donation.SourceTypeDescription = "ending in 1234";
                        break;

                    case PaymentType.Bank:
                    case PaymentType.Check:
                        // TODO Need to lookup info at stripe
                        donation.SourceTypeDescription = "ending in 5678";
                        break;
                }
            }

            return (response);
        }

        public List<string> GetDonationYearsForDonor(int donorId)
        {
            var allDonations = new List<Donation>();
            var softCreditDonations = _mpDonorService.GetSoftCreditDonations(donorId);
            var donations = _mpDonorService.GetDonations(donorId);

            if (softCreditDonations != null)
            {
                allDonations.AddRange(softCreditDonations);
            }
            if (donations != null)
            {
                allDonations.AddRange(donations);
            }

            var years = allDonations.ToDictionary(d => d.donationDate.Year.ToString()).Keys.ToList();

            return (years);
        }

        public DonationBatchDTO GetDonationBatchByDepositId(int depositId)
        {
            return (Mapper.Map<DonationBatch, DonationBatchDTO>(_mpDonationService.GetDonationBatchByDepositId(depositId)));
        }

        public List<DepositDTO> GetSelectedDonationBatches(int selectionId, string token)
        {
            var selectedDeposits = _mpDonationService.GetSelectedDonationBatches(selectionId, token);
            var deposits = new List<DepositDTO>();

            foreach (var deposit in selectedDeposits)
            {
                deposits.Add(Mapper.Map<Deposit, DepositDTO>(deposit));
            }

            return deposits;
        }

        public void ProcessDeclineEmail(string processorPaymentId)
        {
            _mpDonationService.ProcessDeclineEmail(processorPaymentId);
        }

        public DepositDTO CreateDeposit(DepositDTO deposit)
        {
            deposit.Id = _mpDonationService.CreateDeposit(deposit.DepositName, deposit.DepositTotalAmount, deposit.DepositAmount, deposit.ProcessorFeeTotal, deposit.DepositDateTime,
                deposit.AccountNumber, deposit.BatchCount, deposit.Exported, deposit.Notes, deposit.ProcessorTransferId);
            
            return (deposit);

        }

        public void CreatePaymentProcessorEventError(StripeEvent stripeEvent, StripeEventResponseDTO stripeEventResponse)
        {
            _mpDonationService.CreatePaymentProcessorEventError(stripeEvent.Created, stripeEvent.Id, stripeEvent.Type, JsonConvert.SerializeObject(stripeEvent, Formatting.Indented), JsonConvert.SerializeObject(stripeEventResponse, Formatting.Indented));
        }


        public List<GPExportDatumDTO> GetGPExport(int depositId, string token)
        {
            var gpExportData = _mpDonationService.GetGPExport(depositId, token);
            var gpExport = new List<GPExportDatumDTO>();

            foreach (var gpExportDatum in gpExportData)
            {
                gpExport.Add(Mapper.Map<GPExportDatum, GPExportDatumDTO>(gpExportDatum));
            }

            return gpExport;
        }

        public MemoryStream CreateGPExport(int selectionId, int depositId, string token)
        {
            var gpExport = GetGPExport(depositId, token);
            var stream = new MemoryStream();
            CSV.Create(gpExport, GPExportDatumDTO.Headers, stream, "\t");
            UpdateDepositToExported(selectionId, depositId, token);

            return stream;
        }

        private void UpdateDepositToExported(int selectionId, int depositId, string token)
        {
            _mpDonationService.UpdateDepositToExported(selectionId, depositId, token);
        }

        public List<DepositDTO> GenerateGPExportFileNames(int selectionId, string token)
        {
            var deposits = GetSelectedDonationBatches(selectionId, token);

            foreach (var deposit in deposits)
            {
                deposit.ExportFileName = GPExportFileName(deposit.Id);
            }

            return deposits;
        }

        public string GPExportFileName(int depositId)
        {
            var batch = GetDonationBatchByDepositId(depositId);

            var date = DateTime.Today.ToString("yyMMdd");
            var batchName = batch.BatchName.Replace(" ", "_");
            return string.Format("XRDReceivables-{0}_{1}.txt", batchName, date);
        }
    }
}