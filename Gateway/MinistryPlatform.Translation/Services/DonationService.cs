using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Enum;
using MinistryPlatform.Translation.Exceptions;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.PlatformService;
using MinistryPlatform.Translation.Services.Interfaces;
using Communication = MinistryPlatform.Models.Communication;

namespace MinistryPlatform.Translation.Services
{
    public class DonationService : BaseService, IDonationService
    {
        private readonly int _donationsPageId;
        private readonly int _donationDistributionPageId;
        private readonly int _donorMessageTemplateId;
        private readonly int _distributionPageId;
        private readonly int _batchesPageId;
        private readonly int _depositsPageId;
        private readonly int _paymentProcessorErrorsPageId;
        private readonly int _tripDistributionsPageView;
        private readonly int _gpExportPageView;
        private readonly int _processingProgramId;
        private readonly int _scholarshipPaymentTypeId;
        private readonly int _tripDonationMessageTemplateId;
        private readonly int _donationCommunicationsPageId;
        private readonly int _messagesPageId;
        private readonly int _glAccountMappingByProgramPageView;

        private readonly IMinistryPlatformService _ministryPlatformService;
        private readonly IDonorService _donorService;
        private readonly ICommunicationService _communicationService;
        private readonly IPledgeService _pledgeService;
        
        private readonly int _donationStatusSucceeded;

        public DonationService(IMinistryPlatformService ministryPlatformService, IDonorService donorService, ICommunicationService communicationService, IPledgeService pledgeService, IConfigurationWrapper configuration, IAuthenticationService authenticationService, IConfigurationWrapper configurationWrapper)
            : base(authenticationService, configurationWrapper)
        {
            _ministryPlatformService = ministryPlatformService;
            _donorService = donorService;
            _communicationService = communicationService;
            _pledgeService = pledgeService;
            _donationsPageId = configuration.GetConfigIntValue("Donations");
            _donationDistributionPageId = configuration.GetConfigIntValue("Distributions");
            _donorMessageTemplateId = configuration.GetConfigIntValue("DonorMessageTemplateId");
            _distributionPageId = configuration.GetConfigIntValue("Distributions");
            _batchesPageId = configuration.GetConfigIntValue("Batches");
            _depositsPageId = configuration.GetConfigIntValue("Deposits");
            _paymentProcessorErrorsPageId = configuration.GetConfigIntValue("PaymentProcessorEventErrors");
            _tripDistributionsPageView = configuration.GetConfigIntValue("TripDistributionsView");
            _gpExportPageView = configuration.GetConfigIntValue("GPExportView");
            _processingProgramId = configuration.GetConfigIntValue("ProcessingProgramId");
            _scholarshipPaymentTypeId = configuration.GetConfigIntValue("ScholarshipPaymentTypeId");
            _tripDonationMessageTemplateId = configuration.GetConfigIntValue("TripDonationMessageTemplateId");
            _donationStatusSucceeded = configuration.GetConfigIntValue("DonationStatusSucceeded");
            _donationCommunicationsPageId = configuration.GetConfigIntValue("DonationCommunications");
            _messagesPageId = configuration.GetConfigIntValue("Messages");
            _glAccountMappingByProgramPageView = configuration.GetConfigIntValue("GLAccountMappingByProgramPageView");
        }

        public int UpdateDonationStatus(int donationId, int statusId, DateTime statusDate,
            string statusNote = null)
        {
            UpdateDonationStatus(ApiLogin(), donationId, statusId, statusDate, statusNote);
            return (donationId);
        }

        public int UpdateDonationStatus(string processorPaymentId, int statusId,
            DateTime statusDate, string statusNote = null)
        {
            return(WithApiLogin(token =>
            {
                var result = GetDonationByProcessorPaymentId(processorPaymentId, token);

                UpdateDonationStatus(token, result.donationId, statusId, statusDate, statusNote);
                if (statusId == _donationStatusSucceeded)
                {
                    FinishSendMessageFromDonor(result.donationId, true);
                }
                return (result.donationId);
            }));
        }

        public DonationBatch GetDonationBatchByProcessorTransferId(string processorTransferId)
        {
            return(WithApiLogin(token =>
            {
                var search = string.Format(",,,,,,,,{0},", processorTransferId);
                var batches = _ministryPlatformService.GetRecordsDict(_batchesPageId, token, search);
                if (batches == null || batches.Count == 0)
                {
                    return (null);
                }

                return (Mapper.Map<Dictionary<string, object>, DonationBatch>(batches[0]));
            }));
            
        }

        public DonationBatch GetDonationBatch(int batchId)
        {
            return (WithApiLogin(token => (Mapper.Map<Dictionary<string,object>, DonationBatch>(_ministryPlatformService.GetRecordDict(_batchesPageId, batchId, token)))));
        }

        public DonationBatch GetDonationBatchByDepositId(int depositId)
        {
            return (WithApiLogin(token =>
            {
                var search = string.Format(",,,,,{0}", depositId);
                var batches = _ministryPlatformService.GetRecordsDict(_batchesPageId, token, search);
                if (batches == null || batches.Count == 0)
                {
                    return (null);
                }

                return (Mapper.Map<Dictionary<string, object>, DonationBatch>(batches[0]));
            }));
        }

        public List<Deposit> GetSelectedDonationBatches(int selectionId, string token)
        {
            var results = _ministryPlatformService.GetSelectionsForPageDict(_depositsPageId, selectionId, token);
            var deposits = new List<Deposit>();

            foreach (var result in results)
            {
                deposits.Add(Mapper.Map<Dictionary<string, object>, Deposit>(result));
            }

            return deposits;
        } 

        public int CreateDonationBatch(string batchName, DateTime setupDateTime, decimal batchTotalAmount, int itemCount,
            int batchEntryType, int? depositId, DateTime finalizedDateTime, string processorTransferId)
        {
            
            var parms = new Dictionary<string, object>
            {
                {"Batch_Name", batchName},
                {"Setup_Date", setupDateTime},
                {"Batch_Total", batchTotalAmount},
                {"Item_Count", itemCount},
                {"Batch_Entry_Type_ID", batchEntryType},
                {"Deposit_ID", depositId},
                {"Finalize_Date", finalizedDateTime},
                {"Processor_Transfer_ID", processorTransferId}
            };
            try
            {
                var token = ApiLogin();
                var batchId = _ministryPlatformService.CreateRecord(_batchesPageId, parms, token);

                // Important! These two fields have to be set on an update, not on the initial
                // create.  They are nullable fields with default values, but setting a null
                // value on the CreateRecord call has no effect (the default values still get used).
                var updateParms = new Dictionary<string, object>
                {
                    {"Batch_ID", batchId},
                    {"Currency", null},
                    {"Default_Payment_Type", null}
                };
                _ministryPlatformService.UpdateRecord(_batchesPageId, updateParms, token);

                return (batchId);
            }
            catch (Exception e)
            {
                throw new ApplicationException(
                    string.Format(
                        "CreateDonationBatch failed. batchName: {0}, setupDateTime: {1}, batchTotalAmount: {2}, itemCount: {3}, batchEntryType: {4}, depositId: {5}, finalizedDateTime: {6}",
                        batchName, setupDateTime, batchTotalAmount, itemCount, batchEntryType, depositId,
                        finalizedDateTime), e);
            }
        }

        public void AddDonationToBatch(int batchId, int donationId)
        {
            var parms = new Dictionary<string, object>
            {
                {"Donation_ID", donationId},
                {"Batch_ID", batchId}
            };

            try
            {
                WithApiLogin(token =>
                {
                    _ministryPlatformService.UpdateRecord(_donationsPageId, parms, token);
                    return (true);
                });
            }
            catch (Exception e)
            {
                throw new ApplicationException(
                    string.Format(
                        "AddDonationToBatch failed. batchId: {0}, donationId: {1}",
                        batchId, donationId), e);
            }
        }

        public int CreateDeposit(string depositName, decimal depositTotalAmount, decimal depositAmount, decimal depositProcessorFee, DateTime depositDateTime,
            string accountNumber, int batchCount, bool exported, string notes, string processorTransferId)
        {
            var parms = new Dictionary<string, object>
            {
                {"Deposit_Name", depositName},
                {"Deposit_Total", depositTotalAmount},
                {"Deposit_Amount", depositAmount},
                {"Processor_Fee_Total", depositProcessorFee},
                {"Deposit_Date", depositDateTime},
                {"Account_Number", accountNumber},
                {"Batch_Count", batchCount},
                {"Exported", exported},
                {"Notes", notes},
                {"Processor_Transfer_ID", processorTransferId}
            };

            try
            {
                return (WithApiLogin(token => (_ministryPlatformService.CreateRecord(_depositsPageId, parms, token))));
            }
            catch (Exception e)
            {
                throw new ApplicationException(
                    string.Format(
                        "CreateDeposit failed. depositName: {0}, depositTotalAmount: {1}, depositAmount: {2}, depositProcessorFee: {3}, depositDateTime: {4}, accountNumber: {5}, batchCount: {6}, exported: {7}, notes: {8}",
                        depositName, depositTotalAmount, depositAmount, depositProcessorFee, depositDateTime, accountNumber, batchCount, exported, notes), e);
            }
        }

        public void CreatePaymentProcessorEventError(DateTime? eventDateTime, string eventId, string eventType, string eventMessage,
            string responseMessage)
        {
            var parms = new Dictionary<string, object>
            {
                {"Event_Date_Time", eventDateTime ?? DateTime.Now},
                {"Event_ID", eventId},
                {"Event_Type", eventType},
                {"Event_Message", eventMessage},
                {"Response_Message", responseMessage}
            };
            try
            {
                WithApiLogin(token => _ministryPlatformService.CreateRecord(_paymentProcessorErrorsPageId, parms, token));
            }
            catch (Exception e)
            {
                throw (new ApplicationException(string.Format("Could not insert event error dateTime: {0}, eventId: {1}, eventType: {2}, eventMessage: {3}, responseMessage: {4}, Error: {5}", eventDateTime, eventId, eventType, eventMessage, responseMessage, e.Message)));
            }
        }

        private void UpdateDonationStatus(string apiToken, int donationId, int statusId, DateTime statusDate,
            string statusNote)
        {
            var parms = new Dictionary<string, object>
            {
                {"Donation_ID", donationId},
                {"Donation_Status_Date", statusDate},
                {"Donation_Status_Notes", statusNote},
                {"Donation_Status_ID", statusId}
            };

            try
            {
                _ministryPlatformService.UpdateRecord(_donationsPageId, parms, apiToken);
            }
            catch (Exception e)
            {
                throw new ApplicationException(
                    string.Format(
                        "UpdateDonationStatus failed. donationId: {0}, statusId: {1}, statusNote: {2}, statusDate: {3}",
                        donationId, statusId, statusNote, statusDate), e);
            }
        }

        public void ProcessDeclineEmail(string processorPaymentId)
        {
            try
            {
                var apiToken = ApiLogin();
                var result = GetDonationByProcessorPaymentId(processorPaymentId, apiToken);

                var rec = _ministryPlatformService.GetRecordsDict(_distributionPageId, apiToken, string.Format(",,,,,,,,\"{0}\"", result.donationId));
                
                if (rec.Count == 0 || (rec.Last().ToNullableInt("dp_RecordID")) == null)
                {
                    throw (new DonationNotFoundException(processorPaymentId));
                }
                
                var program = rec.First().ToString("Statement_Title");
                var paymentType = PaymentType.GetPaymentType(result.paymentTypeId).name;
                var declineEmailTemplate = PaymentType.GetPaymentType(result.paymentTypeId).declineEmailTemplateId;

                _donorService.SendEmail(declineEmailTemplate, result.donorId, result.donationAmt, paymentType, result.donationDate,
                    program, result.donationNotes);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    string.Format(
                        "ProcessDeclineEmail failed. processorPaymentId: {0},", processorPaymentId), ex);
            }
        }

        public Donation GetDonationByProcessorPaymentId(string processorPaymentId)
        {
            return (WithApiLogin(token => GetDonationByProcessorPaymentId(processorPaymentId, token)));
        }
        
        private Donation GetDonationByProcessorPaymentId(string processorPaymentId, string apiToken)
        {
            var result = _ministryPlatformService.GetRecordsDict(_donationsPageId, apiToken,
                string.Format(",,,,,,,\"{0}\"", processorPaymentId));
          
            if (result.Count == 0 || (result.Last().ToNullableInt("dp_RecordID")) == null)
            {
                throw (new DonationNotFoundException(processorPaymentId));
            }

            var dictionary = result.First();
          
            var d = new Donation()
            {
                donationId = dictionary.ToInt("dp_RecordID"),
                donorId = dictionary.ToInt("Donor_ID"),
                donationDate = dictionary.ToDate("Donation_Date"),
                donationAmt = Convert.ToInt32(dictionary["Donation_Amount"]),
                paymentTypeId = PaymentType.GetPaymentType(dictionary.ToString("Payment_Type")).id,
                donationNotes = dictionary.ToString("Donation_Status_Notes"),
                batchId = dictionary.ToNullableInt("Batch_ID")
            };
            return (d);
        }

        public List<TripDistribution> GetMyTripDistributions(int contactId)
        {
            var results = _ministryPlatformService.GetPageViewRecords(_tripDistributionsPageView, ApiLogin(), contactId.ToString());
            var trips = new List<TripDistribution>();
            foreach (var result in results)
            {
                var trip = new TripDistribution
                {
                    EventId = result.ToInt("Event ID"),
                    EventTypeId = result.ToInt("Event Type ID"),
                    EventTitle = result.ToString("Event Title"),
                    EventStartDate = result.ToDate("Event Start Date"),
                    EventEndDate = result.ToDate("Event End Date"),
                    TotalPledge = Convert.ToInt32(result["Total Pledge"]),
                    CampaignStartDate = result.ToDate("Start Date"),
                    CampaignEndDate = result.ToDate("End Date"),
                    DonorId = result.ToInt("Donor ID"),
                    DonationDistributionId = result.ToInt("dp_RecordID"),
                    DonorNickname = result.ToString("Nickname"),
                    DonorFirstName = result.ToString("First Name"),
                    DonorLastName = result.ToString("Last Name"),
                    DonorEmail = result.ToString("Email Address"),
                    DonationDate = result.ToDate("Donation Date"),
                    DonationAmount = Convert.ToInt32(result["Amount"]),
                    PaymentTypeId = Convert.ToInt32(result["Payment Type ID"]),
                    AnonymousGift = result.ToBool("Anonymous"),
                    RegisteredDonor = result.ToBool("Registered Donor"),
                    MessageSent = result.ToBool("Message Sent")
                };

                trips.Add(trip);
            }
            return trips;
        }

        public List<GPExportDatum> GetGPExport(int depositId, string token)
        {
            var gpExport = new List<GPExportDatum>();
            var glLevelGPExport = GetGLLevelGpExport(depositId, token);

            var id = 1;
            foreach (var glLevelGPData in glLevelGPExport)
            {
                gpExport.Add(AddDocumentNumber(glLevelGPData.Value[0], id));
                gpExport.Add(AddDocumentNumber(glLevelGPData.Value[1], id));
                id++;
            }

            return gpExport;
        }

        private GPExportDatum AddDocumentNumber(GPExportDatum datum, int index)
        {
            datum.DocumentNumber = String.Format("{0}000{1}", datum.DepositId, index);

            return datum;
        }

        private Dictionary<string, List<GPExportDatum>> GetGLLevelGpExport(int depositId, string token)
        {
            var processingFeeGLMapping = GetProcessingFeeGLMapping(token);
            var gpExportDonationLevel = GetGPExportData(depositId, token);
            //index 0 will contain the contribution index 1 the processor fee
            var gpExportGLLevel = new Dictionary<string, List<GPExportDatum>>(); 

            //loop through each donation and add it to the the correct GL Mapping
            foreach (var gpExportDistLevel in gpExportDonationLevel)
            {
                GenerateGLLevelGpExport(gpExportGLLevel, gpExportDistLevel.Value, processingFeeGLMapping);
            }

            return gpExportGLLevel;
        }

        private void GenerateGLLevelGpExport(IDictionary<string, List<GPExportDatum>> gpExportGLLevel, ICollection<GPExportDatum> gpExportDistLevel, Dictionary<string, object> processingFeeGLMapping)
        {

            var previousProcessorFees = (decimal)0.0;
            var indx = 0;

            foreach (var datum in gpExportDistLevel)
            {

                var processorFee = CalculateProcessorFee(datum, previousProcessorFees, gpExportDistLevel.Count, indx);
                var key = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}", datum.DocumentType, datum.BatchName, datum.DonationDate.ToString("MM/dd/yyyy"), datum.CheckbookId, datum.CashAccount, datum.ReceivableAccount, datum.DistributionAccount);
                
                //if key is not there add it
                if (!gpExportGLLevel.ContainsKey(key))
                {
                    gpExportGLLevel.Add(key, new List<GPExportDatum>());
                }

                //If this is a new GL Level Contribution take out the processor fee and add it in
                //If it is not take out the processor fee and add to the existing GL Level Contribution
                if (gpExportGLLevel[key].Count == 0)
                {
                    gpExportGLLevel[key].Add(AdjustGPExportDatumAmount(datum, processorFee));
                }
                else
                {
                     AdjustGpExportDatum(gpExportGLLevel[key][0], datum, processorFee);
                }

                //the same as above except processor fee and not contribution
                if (gpExportGLLevel[key].Count == 1)
                {
                    gpExportGLLevel[key].Add(CreateProcessorFee(datum, processorFee, processingFeeGLMapping));
                }
                else
                {
                    AdjustProcessorFee(gpExportGLLevel[key][1], processorFee);
                }

                //set this as we will use this to help determine how much of a processor fee is on the
                //next distribution of this donation
                previousProcessorFees += processorFee;
                indx++;
            }
        }

        private static decimal CalculateProcessorFee(GPExportDatum datum, decimal previousFee, long distCount, long distProcessed)
        {
            var processorFee = (datum.ProcessorFeeAmount - previousFee) / (distCount - distProcessed);
            return Math.Round(processorFee, 2, MidpointRounding.AwayFromZero);
        }

        private static void AdjustGpExportDatum(GPExportDatum glLevelDatum, GPExportDatum datum, decimal processorFee)
        {
            AdjustGPExportDatumAmount(datum, processorFee);
            glLevelDatum.Amount += datum.Amount;
        }

        private static GPExportDatum AdjustGPExportDatumAmount(GPExportDatum datum, decimal processorFee)
        {
            if (!datum.DocumentType.Equals("RETURNS"))
            {
                datum.Amount = datum.Amount - processorFee;
            }

            return datum;
        }

        private GPExportDatum CreateProcessorFee(GPExportDatum datum, decimal processorFee, Dictionary<string, object> processingFeeGLMapping)
        {
            processorFee = processorFee<0 ? -1*processorFee : processorFee;
            return new GPExportDatum
            {
                ProccessFeeProgramId = _processingProgramId,
                ProgramId = _processingProgramId,
                DocumentType = datum.DocumentType,
                DepositId = datum.DepositId,
                DonationId = datum.DonationId,
                BatchName = datum.BatchName,
                DonationDate = datum.DonationDate,
                DepositDate = datum.DepositDate,
                CustomerId = processingFeeGLMapping.ToString("Customer_ID"),
                DepositAmount = datum.DepositAmount,
                DonationAmount = datum.DonationAmount,
                CheckbookId = processingFeeGLMapping.ToString("Checkbook_ID"),
                CashAccount = processingFeeGLMapping.ToString("Cash_Account"),
                ReceivableAccount = datum.ReceivableAccount,
                DistributionAccount = datum.DistributionAccount,
                ScholarshipExpenseAccount = datum.ScholarshipExpenseAccount,
                Amount = processorFee,
                ScholarshipPaymentTypeId = datum.ScholarshipPaymentTypeId,
                PaymentTypeId = datum.PaymentTypeId,
                ProcessorFeeAmount = datum.ProcessorFeeAmount
            };
        }

        private static void AdjustProcessorFee(GPExportDatum glLevelFee, decimal processorFee)
        {
            processorFee = processorFee < 0 ? -1 * processorFee : processorFee;
            glLevelFee.Amount = glLevelFee.Amount + processorFee;
        }

        public Dictionary<int, List<GPExportDatum>> GetGPExportData(int depositId, string token)
        {
            var results = _ministryPlatformService.GetPageViewRecords(_gpExportPageView, token, depositId.ToString());
            var gpExport = new Dictionary<int, List<GPExportDatum>>();


            foreach (var result in results)
            {
                var donationID = result.ToInt("Donation_ID");

                if (!gpExport.ContainsKey(donationID))
                {
                    gpExport.Add(donationID, new List<GPExportDatum>());
                }

                var amount = Convert.ToDecimal(result.ToString("Amount"));
                gpExport[donationID].Add(
                    new GPExportDatum
                    {
                        ProccessFeeProgramId = _processingProgramId,
                        DepositId = result.ToInt("Deposit_ID"),
                        ProgramId = result.ToInt("Program_ID"),
                        DocumentType = (amount < 0) ? "RETURNS" : result.ToString("Document_Type"),
                        DonationId = result.ToInt("Donation_ID"),
                        BatchName = result.ToString("Batch_Name"),
                        DonationDate = result.ToDate("Donation_Date"),
                        DepositDate = result.ToDate("Deposit_Date"),
                        CustomerId = result.ToString("Customer_ID"),
                        DonationAmount = result.ToString("Donation_Amount"),
                        DepositAmount = result.ToString("Deposit_Amount"),
                        CheckbookId = result.ToString("Checkbook_ID"),
                        CashAccount = result.ToString("Cash_Account"),
                        ReceivableAccount = result.ToString("Receivable_Account"),
                        DistributionAccount = result.ToString("Distribution_Account"),
                        ScholarshipExpenseAccount = result.ToString("Scholarship_Expense_Account"),
                        Amount = (amount < 0) ? -1*amount : amount,
                        ScholarshipPaymentTypeId = _scholarshipPaymentTypeId,
                        PaymentTypeId = result.ToInt("Payment_Type_ID"),
                        ProcessorFeeAmount = Convert.ToDecimal(result.ToString("Processor_Fee_Amount"))
                    });
            }

            return gpExport;
        }

        private Dictionary<string, object> GetProcessingFeeGLMapping(string token)
        {
            return _ministryPlatformService.GetPageViewRecords(_glAccountMappingByProgramPageView, token, _processingProgramId.ToString()).First();
        }

        public void UpdateDepositToExported(int selectionId, int depositId, string token)
        {
            var paramaters = new Dictionary<string, object>
            {
                {"Deposit_ID", depositId},
                {"Exported", true},
            };

            _ministryPlatformService.UpdateRecord(_depositsPageId, paramaters, token);
            _ministryPlatformService.RemoveSelection(selectionId, new [] {depositId}, token);
        }

        public void SendMessageToDonor(int donorId, int donationDistributionId, int fromContactId, string body, string tripName )
        {
            var template = _communicationService.GetTemplate(_donorMessageTemplateId);
            var defaultContactId = AppSetting("DefaultGivingContactEmailId");
            var defaultContactEmail = _communicationService.GetEmailFromContactId(defaultContactId);

            var messageData = new Dictionary<string, object>
            {
                {"TripName", tripName},
                {"DonorMessage", body}
            };
            var toEmail = _donorService.GetEmailViaDonorId(donorId);

            var to = new List<Contact>()
            {
                new Contact()
                {
                     ContactId = toEmail.ContactId,
                    EmailAddress = toEmail.Email
                }
            };

            var authorId = _communicationService.GetUserIdFromContactId(fromContactId);
            var fromEmail = _communicationService.GetEmailFromContactId(fromContactId);

            var comm = new Communication
            {
                AuthorUserId = authorId,
                DomainId = 1,
                ToContacts = to,
                FromContact = new Contact(){ContactId = defaultContactId, EmailAddress = defaultContactEmail},
                ReplyToContact = new Contact(){ContactId = fromContactId, EmailAddress = fromEmail},
                EmailSubject = _communicationService.ParseTemplateBody(template.Subject, messageData),
                EmailBody = _communicationService.ParseTemplateBody(template.Body, messageData),
                MergeData = messageData
            };
            _communicationService.SendMessage(comm);

            //mark donation distribution with message sent

            var distributionData = new Dictionary<string, object>
            {
                {"Donation_Distribution_ID", donationDistributionId},
                {"Message_Sent", true}
            };
           
            _ministryPlatformService.UpdateRecord(_donationDistributionPageId, distributionData, ApiLogin());
        }

        public void SendMessageFromDonor(int pledgeId, int donationId, string message)
        {
            var toDonor = _pledgeService.GetDonorForPledge(pledgeId);
            var donorContact = _donorService.GetEmailViaDonorId(toDonor);
            var template = _communicationService.GetTemplate(_tripDonationMessageTemplateId);

            var toContacts = new List<Contact> {new Contact {ContactId = donorContact.ContactId, EmailAddress = donorContact.Email}};

            var from = new Contact()
            {
                ContactId = 5,
                EmailAddress = "updates@crossroads.net"
            };

            var comm = new Communication
            {
                AuthorUserId = 5,
                DomainId = 1,
                EmailBody = message,
                EmailSubject = template.Subject,
                FromContact = from,
                ReplyToContact = from,
                ToContacts = toContacts,
                MergeData = new Dictionary<string, object>()
            };
            var communicationId = _communicationService.SendMessage(comm, true);
            AddDonationCommunication(donationId, communicationId);
        }

        public void FinishSendMessageFromDonor(int donationId, bool succeeded)
        {
            // this code sets the status of a pending message to donor to ready to send, once there's a successful donation
            // stripe webhook returned - JPC 2/25/2016
            var donationCommunicationRecords = _ministryPlatformService.GetRecordsDict(_donationCommunicationsPageId, ApiLogin(), string.Format("\"{0}\"",donationId), "");
            var donationCommunicationRecord = donationCommunicationRecords.FirstOrDefault();
            var communicationId = Int32.Parse(donationCommunicationRecord["Communication_ID"].ToString());
            var recordId = Int32.Parse(donationCommunicationRecord["dp_RecordID"].ToString());

            Dictionary<string, object> communicationUpdateValues = new Dictionary<string, object>();
            communicationUpdateValues.Add("Communication_ID", communicationId);

            if (succeeded == true)
            {
                communicationUpdateValues.Add("Communication_Status_ID", 3);
                _ministryPlatformService.UpdateRecord(_messagesPageId, communicationUpdateValues, ApiLogin());
            }

            DeleteOption[] deleteOptions = new DeleteOption[]
            {
                new DeleteOption
                {
                    Action = DeleteAction.Delete
                }
            };

            try
            {
                _ministryPlatformService.DeleteRecord(_donationCommunicationsPageId, recordId, deleteOptions, ApiLogin());
            }
            catch (Exception e)
            {
                throw new ApplicationException(string.Format("RemoveDonationCommunication failed.  DonationCommunicationId: {0}", recordId), e);
            }
        }

        public void AddDonationCommunication(int donationId, int communicationId)
        {
            var communication = new Dictionary<string, object>
            {
                {"Donation_ID", donationId},
                {"Communication_ID", communicationId},
                {"Domain_ID", 1 }
            };
            var pageId = AppSetting("DonationCommunication");
            _ministryPlatformService.CreateRecord(pageId, communication, ApiLogin(), true);
        }

    }
}
