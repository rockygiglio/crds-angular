﻿using System;
using System.Collections.Generic;
using MinistryPlatform.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IDonationService
    {
        int UpdateDonationStatus(int donationId, int statusId, DateTime statusDate, string statusNote = null);
        int UpdateDonationStatus(string processorPaymentId, int statusId, DateTime statusDate, string statusNote = null);
        int CreateDonationBatch(string batchName, DateTime setupDateTime, decimal batchTotalAmount, int itemCount, int batchEntryType, int? depositId, DateTime finalizedDateTime, string processorTransferId);
        DonationBatch GetDonationBatchByProcessorTransferId(string processorTransferId);
        DonationBatch GetDonationBatch(int batchId);
        DonationBatch GetDonationBatchByDepositId(int depositId);
        Donation GetDonationByProcessorPaymentId(string processorPaymentId);
        List<DonationBatch> GetSelectedDonationBatches(int selectionId); 
        void AddDonationToBatch(int batchId, int donationId);
        void ProcessDeclineEmail(string processorPaymentId);
        int CreateDeposit(string depositName, decimal depositTotalAmount, decimal depositAmount, decimal depositProcessorFee, DateTime depositDateTime, string accountNumber, int batchCount, bool exported, string notes, string processorTransferId);
        void CreatePaymentProcessorEventError(DateTime? eventDateTime, string eventId, string eventType,
            string eventMessage, string responseMessage);

        List<TripDistribution> GetMyTripDistributions(int contactId, string token);
        List<GPExportDatum> CreateGPExport(int depositId, string token);
        void UpdateDepositToExported(int depositId);
    }
}