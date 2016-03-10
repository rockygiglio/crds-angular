using System;
using System.Collections.Generic;
using System.Configuration;
using crds_angular.App_Start;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.PlatformService;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services;
using MinistryPlatform.Translation.Services.Interfaces;
using Moq;
using NUnit.Framework;

namespace MinistryPlatform.Translation.Test.Services
{
    public class DonationServiceTest
    {
        private DonationService _fixture;
        private Mock<IMinistryPlatformService> _ministryPlatformService;
        private Mock<IDonorService> _donorService;
        private Mock<IAuthenticationService> _authService;
        private Mock<IPledgeService> _pledgeService;
        private Mock<ICommunicationService> _communicationService;

        [SetUp]
        public void SetUp()
        {
            AutoMapperConfig.RegisterMappings();

            _ministryPlatformService = new Mock<IMinistryPlatformService>(MockBehavior.Strict);
            _donorService = new Mock<IDonorService>(MockBehavior.Strict);
            _authService = new Mock<IAuthenticationService>();
            _pledgeService = new Mock<IPledgeService>();
            _communicationService = new Mock<ICommunicationService>();

            var configuration = new Mock<IConfigurationWrapper>();
            configuration.Setup(mocked => mocked.GetConfigIntValue("Donations")).Returns(9090);
            configuration.Setup(mocked => mocked.GetConfigIntValue("Batches")).Returns(8080);
            configuration.Setup(mocked => mocked.GetConfigIntValue("Distributions")).Returns(1234);
            configuration.Setup(mocked => mocked.GetConfigIntValue("Deposits")).Returns(7070);
            configuration.Setup(mocked => mocked.GetConfigIntValue("PaymentProcessorEventErrors")).Returns(6060);
            configuration.Setup(mocked => mocked.GetConfigIntValue("GPExportView")).Returns(92198);
            configuration.Setup(mocked => mocked.GetConfigIntValue("ProcessingProgramId")).Returns(127);
            configuration.Setup(mocked => mocked.GetConfigIntValue("DonationCommunications")).Returns(540);
            configuration.Setup(mocked => mocked.GetConfigIntValue("Messages")).Returns(341);
            configuration.Setup(mocked => mocked.GetConfigIntValue("GLAccountMappingByProgramPageView")).Returns(2213);
            configuration.Setup(mocked => mocked.GetConfigIntValue("ScholarshipPaymentTypeId")).Returns(9);

            configuration.Setup(m => m.GetEnvironmentVarAsString("API_USER")).Returns("uid");
            configuration.Setup(m => m.GetEnvironmentVarAsString("API_PASSWORD")).Returns("pwd");
            _authService.Setup(m => m.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(new Dictionary<string, object> { { "token", "ABC" }, { "exp", "123" } });
            _fixture = new DonationService(_ministryPlatformService.Object, _donorService.Object, _communicationService.Object, _pledgeService.Object, configuration.Object, _authService.Object, configuration.Object);
        }

        [Test]
        public void TestGetDonationBatchByProcessorTransferId()
        {
            const string processorTransferId = "123";
            const int depositId = 456;
            const int batchId = 789;
            const string batchName = "TestBachName";
            var searchResult = new List<Dictionary<string, object>>
            {
                {
                    new Dictionary<string, object>
                    {
                        {"dp_RecordID", batchId},
                        {"Processor_Transfer_ID", processorTransferId},
                        {"Deposit_ID", depositId},
                        {"Batch_Name", batchName},
                    }
                }
            };
            _ministryPlatformService.Setup(mocked => mocked.GetRecordsDict(8080, It.IsAny<string>(), string.Format(",,,,,,,,{0},", processorTransferId), "")).Returns(searchResult);

            var result = _fixture.GetDonationBatchByProcessorTransferId(processorTransferId);
            _ministryPlatformService.VerifyAll();
            Assert.IsNotNull(result);
            Assert.AreEqual(processorTransferId, result.ProcessorTransferId);
            Assert.AreEqual(batchId, result.Id);
            Assert.AreEqual(depositId, result.DepositId);
            Assert.AreEqual(batchName, result.BatchName);
        }

        [Test]
        public void TestGetDonationBatch()
        {
            const string processorTransferId = "123";
            const int depositId = 456;
            const int batchId = 789;
            const string batchName = "TestBatchName";
            var getResult = new Dictionary<string, object>
                {
                    {"Batch_ID", batchId},
                    {"Processor_Transfer_ID", processorTransferId},
                    {"Deposit_ID", depositId},
                    {"Batch_Name", batchName},
                };
            _ministryPlatformService.Setup(mocked => mocked.GetRecordDict(8080, batchId, It.IsAny<string>(), false)).Returns(getResult);

            var result = _fixture.GetDonationBatch(batchId);
            _ministryPlatformService.VerifyAll();
            Assert.IsNotNull(result);
            Assert.AreEqual(processorTransferId, result.ProcessorTransferId);
            Assert.AreEqual(batchId, result.Id);
            Assert.AreEqual(depositId, result.DepositId);
            Assert.AreEqual(batchName, result.BatchName);
        }

        [Test]
        public void TestGetDonationBatchByDepositId()
        {
            const string processorTransferId = "123";
            const int depositId = 456;
            const int batchId = 789;
            const string batchName = "TestBachName";
            var searchResult = new List<Dictionary<string, object>>
            {
                {
                    new Dictionary<string, object>
                    {
                        {"dp_RecordID", batchId},
                        {"Processor_Transfer_ID", processorTransferId},
                        {"Deposit_ID", depositId},
                        {"Batch_Name", batchName},
                    }
                }
            };
            _ministryPlatformService.Setup(mocked => mocked.GetRecordsDict(8080, It.IsAny<string>(), string.Format(",,,,,{0}", depositId), "")).Returns(searchResult);

            var result = _fixture.GetDonationBatchByDepositId(depositId);
            _ministryPlatformService.VerifyAll();
            Assert.IsNotNull(result);
            Assert.AreEqual(processorTransferId, result.ProcessorTransferId);
            Assert.AreEqual(batchId, result.Id);
            Assert.AreEqual(depositId, result.DepositId);
            Assert.AreEqual(batchName, result.BatchName);
        }

        [Test]
        public void TestGetSelectedDonationBatches()
        {
            const int selectionId = 1248579;
            const int depositPageId = 7070;
            const string token = "afasdfoweradfafewwefafdsajfdafoew";

            _ministryPlatformService.Setup(mocked => mocked.GetSelectionsForPageDict(depositPageId, selectionId, token)).Returns(MockDepositList);

            var result = _fixture.GetSelectedDonationBatches(selectionId, token);
            _ministryPlatformService.VerifyAll();
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count, 2);
            Assert.AreEqual(DateTime.Parse("2/12/2015"), result[1].DepositDateTime);
            Assert.AreEqual(456, result[0].Id);
        }

        private List<Dictionary<string, object>> MockDepositList()
        {
            return new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"Deposit_Date", DateTime.Parse("2/12/2010")},
                    {"Deposit_Name", "Test Deposit Name 1"},
                    {"Deposit_ID", 456},
                    {"Deposit_Total", 7829.00},
                    {"Batch_Count", 1},
                    {"Exported", false},
                    {"Processor_Transfer_ID", "1233"},
                },
                new Dictionary<string, object>
                {
                    {"Deposit_Date", DateTime.Parse("2/12/2015")},
                    {"Deposit_Name", "Test Deposit Name 2"},
                    {"Deposit_ID", 777},
                    {"Deposit_Total", 2.00},
                    {"Batch_Count", 11},
                    {"Exported", false},
                    {"Processor_Transfer_ID", "122233"},
                }
            };
        }

        [Test]
        public void TestUpdateDonationStatusById()
        {
            const int donationId = 987;
            var donationStatusDate = DateTime.Now.AddDays(-1);
            const string donationStatusNotes = "note";
            const int donationStatusId = 654;

            var expectedParms = new Dictionary<string, object>
            {
                {"Donation_ID", donationId},
                {"Donation_Status_Date", donationStatusDate},
                {"Donation_Status_Notes", donationStatusNotes},
                {"Donation_Status_ID", donationStatusId}
            };
            _ministryPlatformService.Setup(mocked => mocked.UpdateRecord(9090, expectedParms, It.IsAny<string>()));

            _fixture.UpdateDonationStatus(donationId, donationStatusId, donationStatusDate, donationStatusNotes);

            _ministryPlatformService.VerifyAll();
        }

        [Test]
        public void TestUpdateDonationStatusByProcessorPaymentId()
        {
            const int donationId = 987;
            var donationStatusDate = DateTime.Now.AddDays(-1);
            const string donationStatusNotes = "note";
            const int donationStatusId = 654;
            const int donorId = 9876;
            const int donationAmt = 4343;
            const string paymentType = "Bank";
            const int batchId = 9090;

            var expectedParms = new Dictionary<string, object>
            {
                {"Donation_ID", donationId},
                {"Donation_Status_Date", donationStatusDate},
                {"Donation_Status_Notes", donationStatusNotes},
                {"Donation_Status_ID", donationStatusId}
            };

            _ministryPlatformService.Setup(mocked => mocked.UpdateRecord(9090, expectedParms, It.IsAny<string>()));

            var searchResult = new List<Dictionary<string, object>>
            {
                {
                    new Dictionary<string, object>
                    {
                        {"dp_RecordID", donationId},
                        {"Donor_ID", donorId},
                        {"Donation_Amount", donationAmt},
                        {"Donation_Date", donationStatusDate},
                        {"Donation_Status_Notes", donationStatusNotes},
                        {"Payment_Type", paymentType},
                        {"Batch_ID", batchId}
                    }
                }
            };
            _ministryPlatformService.Setup(
                mocked => mocked.GetRecordsDict(9090, It.IsAny<string>(), ",,,,,,,\"ch_123\"", It.IsAny<string>()))
                .Returns(searchResult);

            _fixture.UpdateDonationStatus("ch_123", donationStatusId, donationStatusDate, donationStatusNotes);
            _ministryPlatformService.VerifyAll();
        }

        [Test]
        public void TestCreateDonationBatch()
        {
            const string batchName = "MP12345";
            var setupDateTime = DateTime.Now;
            const decimal batchTotalAmount = 456.78M;
            const int itemCount = 55;
            const int batchEntryType = 44;
            const int depositId = 987;
            var finalizedDateTime = DateTime.Now;
            const string processorTransferId = "transfer 1";

            var expectedParms = new Dictionary<string, object>
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
            _ministryPlatformService.Setup(mocked => mocked.CreateRecord(8080, expectedParms, It.IsAny<string>(), false))
                .Returns(513);

            var expectedUpdateParms = new Dictionary<string, object>
            {
                {"Batch_ID", 513},
                {"Currency", null},
                {"Default_Payment_Type", null}
            };
            _ministryPlatformService.Setup(mocked => mocked.UpdateRecord(8080, expectedUpdateParms, It.IsAny<string>()));
            var batchId = _fixture.CreateDonationBatch(batchName, setupDateTime, batchTotalAmount, itemCount, batchEntryType,
                depositId, finalizedDateTime, processorTransferId);
            Assert.AreEqual(513, batchId);
            _ministryPlatformService.VerifyAll();
        }

        [Test]
        public void TestAddDonationToBatch()
        {
            const int batchId = 123;
            const int donationId = 456;

            var expectedParms = new Dictionary<string, object>
            {
                {"Donation_ID", donationId},
                {"Batch_ID", batchId}
            };
            _ministryPlatformService.Setup(mocked => mocked.UpdateRecord(9090, expectedParms, It.IsAny<string>()));
            _fixture.AddDonationToBatch(batchId, donationId);
            _ministryPlatformService.VerifyAll();
        }

        [Test]
        public void TestCreateDeposit()
        {
            const string depositName = "MP12345";
            const decimal depositTotalAmount = 456.78M;
            const decimal depositAmount = 450.00M;
            const decimal depositProcessorFee = 6.78M;
            var depositDateTime = DateTime.Now;
            const string accountNumber = "8675309";
            const int batchCount = 55;
            const bool exported = true;
            const string notes = "C Sharp";
            const string processorTransferId = "transfer 1";

            var expectedParms = new Dictionary<string, object>
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

            _ministryPlatformService.Setup(mocked => mocked.CreateRecord(7070, expectedParms, It.IsAny<string>(), false))
                .Returns(513);
            var depositId = _fixture.CreateDeposit(depositName, depositTotalAmount, depositAmount, depositProcessorFee, depositDateTime, accountNumber,
                batchCount, exported, notes, processorTransferId);
            Assert.AreEqual(513, depositId);
            _ministryPlatformService.VerifyAll();
        }

        [Test]
        public void TestCreatePaymentProcessorEventError()
        {
            var dateTime = DateTime.Now;
            const string eventId = "123";
            const string eventType = "456";
            const string message = "message";
            const string response = "response";
            var expectedParms = new Dictionary<string, object>
            {
                {"Event_Date_Time", dateTime},
                {"Event_ID", eventId},
                {"Event_Type", eventType},
                {"Event_Message", message},
                {"Response_Message", response}
            };
            _ministryPlatformService.Setup(mocked => mocked.CreateRecord(6060, expectedParms, It.IsAny<string>(), false)).Returns(513);

            _fixture.CreatePaymentProcessorEventError(dateTime, eventId, eventType, message, response);

            _ministryPlatformService.VerifyAll();
        }

        [Test]
        public void TestGetGPExport()
        {
            const int viewId = 92198;
            const int depositId = 789;

            _ministryPlatformService.Setup(mock => mock.GetPageViewRecords(viewId, It.IsAny<string>(), depositId.ToString(), "", 0)).Returns(MockGPExport());

            var result = _fixture.GetGPExport(depositId, It.IsAny<string>());
            _ministryPlatformService.VerifyAll();
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(new DateTime(2015, 3, 28, 8, 30, 0), result[10002][0].DepositDate);
            Assert.AreEqual(15, result[10002][0].ProgramId);
            Assert.AreEqual(8, result[10002][0].PaymentTypeId);
            Assert.AreEqual("19998-900-11", result[10002][0].ScholarshipExpenseAccount);
        }

        [Test]
        public void TestGetGPExportAndProcessorFees()
        {
            const int viewId = 92198;
            const int depositId = 789;
            var mockGPExportData = MockGPExportData();

            _ministryPlatformService.Setup(mock => mock.GetPageViewRecords(viewId, It.IsAny<string>(), depositId.ToString(), "", 0)).Returns(MockGPExport());
            _ministryPlatformService.Setup(mock => mock.GetPageViewRecords(2213, It.IsAny<string>(), 127.ToString(), "", 0)).Returns(MockProcessingFeeGLMapping());

            var result = _fixture.GetGPExportAndProcessorFees(depositId, It.IsAny<string>());
            _ministryPlatformService.VerifyAll();
            Assert.IsNotNull(result);
            Assert.AreEqual(6, result.Count);

            Assert.AreEqual(mockGPExportData[0].DocumentType, result[0].DocumentType);
            Assert.AreEqual(mockGPExportData[0].DonationId, result[0].DonationId);
            Assert.AreEqual(mockGPExportData[0].BatchName, result[0].BatchName);
            Assert.AreEqual(mockGPExportData[0].DonationDate, result[0].DonationDate);
            Assert.AreEqual(mockGPExportData[0].DepositDate, result[0].DepositDate);
            Assert.AreEqual(mockGPExportData[0].CustomerId, result[0].CustomerId);
            Assert.AreEqual(mockGPExportData[0].DonationAmount, result[0].DonationAmount);
            Assert.AreEqual(mockGPExportData[0].CheckbookId, result[0].CheckbookId);
            Assert.AreEqual(mockGPExportData[0].CashAccount, result[0].CashAccount);
            Assert.AreEqual(mockGPExportData[0].ReceivableAccount, result[0].ReceivableAccount);
            Assert.AreEqual(mockGPExportData[0].DistributionAccount, result[0].DistributionAccount);
            Assert.AreEqual(mockGPExportData[0].Amount, result[0].Amount);
            Assert.AreEqual(mockGPExportData[0].ProcessorFeeAmount, result[0].ProcessorFeeAmount);
            Assert.AreEqual(mockGPExportData[0].ProgramId, result[0].ProgramId);
            Assert.AreEqual(mockGPExportData[0].ProccessFeeProgramId, result[0].ProccessFeeProgramId);
            Assert.AreEqual(mockGPExportData[0].PaymentTypeId, result[0].PaymentTypeId);
            Assert.AreEqual(mockGPExportData[0].ScholarshipExpenseAccount, result[0].ScholarshipExpenseAccount);
            Assert.AreEqual(mockGPExportData[0].ScholarshipPaymentTypeId, result[0].ScholarshipPaymentTypeId);

            Assert.AreEqual(mockGPExportData[1].DocumentType, result[1].DocumentType);
            Assert.AreEqual(mockGPExportData[1].DonationId, result[1].DonationId);
            Assert.AreEqual(mockGPExportData[1].BatchName, result[1].BatchName);
            Assert.AreEqual(mockGPExportData[1].DonationDate, result[1].DonationDate);
            Assert.AreEqual(mockGPExportData[1].DepositDate, result[1].DepositDate);
            Assert.AreEqual(mockGPExportData[1].CustomerId, result[1].CustomerId);
            Assert.AreEqual(mockGPExportData[1].DonationAmount, result[1].DonationAmount);
            Assert.AreEqual(mockGPExportData[1].CheckbookId, result[1].CheckbookId);
            Assert.AreEqual(mockGPExportData[1].CashAccount, result[1].CashAccount);
            Assert.AreEqual(mockGPExportData[0].ReceivableAccount, result[1].ReceivableAccount);
            Assert.AreEqual(mockGPExportData[0].DistributionAccount, result[1].DistributionAccount);
            Assert.AreEqual(mockGPExportData[1].Amount, result[1].Amount);
            Assert.AreEqual(mockGPExportData[1].ProgramId, result[1].ProgramId);
            Assert.AreEqual(mockGPExportData[1].ProccessFeeProgramId, result[1].ProccessFeeProgramId);
            Assert.AreEqual(mockGPExportData[1].PaymentTypeId, result[1].PaymentTypeId);
        }

        private List<Dictionary<string, object>> MockProcessingFeeGLMapping()
        {
            return new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"dp_RecordID", 100},
                    {"Document_Type", "SALE"},
                    {"Customer_ID", "CONTRIBUTI001"},
                    {"Checkbook_ID", "PNC001"},
                    {"Cash_Account", "77777-031-20"},
                    {"Receivable_Account", "77777-031-21"},
                    {"Distribution_Account", "77777-031-22"},
                    {"Scholarship_Expense_Account", "77777-900-11"},
                }
            };
        }

        private List<Dictionary<string, object>> MockGPExport()
        {
            return new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"dp_RecordID", 100},
                    {"Document_Type", "SALE"},
                    {"Donation_ID", "10002"},
                    {"Batch_Name", "Test Batch"},
                    {"Donation_Date",new DateTime(2015, 3, 28, 8, 30, 0)},
                    {"Deposit_Date", new DateTime(2015, 3, 28, 8, 30, 0)},
                    {"Customer_ID", "CONTRIBUTI001"},
                    {"Donation_Amount", "200.00"},
                    {"Checkbook_ID", "PNC001"},
                    {"Cash_Account", "91213-031-20"},
                    {"Receivable_Account", "90013-031-21"},
                    {"Distribution_Account", "90001-031-22"},
                    {"Amount", "185.00"},
                    {"Program_ID", "15"},
                    {"Payment_Type_ID", 8},
                    {"Scholarship_Expense_Account", "19998-900-11"},
                    {"Processor_Fee_Amount", "0.25"}
                },
                new Dictionary<string, object>
                {
                    {"dp_RecordID", 200},
                    {"Document_Type", "SALE"},
                    {"Donation_ID", "10002"},
                    {"Batch_Name", "Test Batch"},
                    {"Donation_Date", new DateTime(2015, 3, 28, 8, 30, 0)},
                    {"Deposit_Date", new DateTime(2015, 3, 28, 8, 30, 0)},
                    {"Customer_ID", "CONTRIBUTI001"},
                    {"Donation_Amount", "200.00"},
                    {"Checkbook_ID", "PNC001"},
                    {"Cash_Account", "91213-031-20"},
                    {"Receivable_Account", "90013-031-21"},
                    {"Distribution_Account", "90001-031-22"},
                    {"Amount", "15.00"},
                    {"Program_ID", "127"},
                    {"Payment_Type_ID", 7},
                    {"Scholarship_Expense_Account", "19948-900-11"},
                    {"Processor_Fee_Amount", "0.25"}
                },
                new Dictionary<string, object>
                {
                    {"dp_RecordID", 300},
                    {"Document_Type", "SALE"},
                    {"Donation_ID", "10003"},
                    {"Batch_Name", "Test Batch 1"},
                    {"Donation_Date", new DateTime(2015, 3, 10, 8, 30, 0)},
                    {"Deposit_Date", new DateTime(2015, 3, 10, 8, 30, 0)},
                    {"Customer_ID", "CONTRIBUTI001"},
                    {"Donation_Amount", "300.00"},
                    {"Checkbook_ID", "PNC001"},
                    {"Cash_Account", "91213-031-20"},
                    {"Receivable_Account", "90013-031-21"},
                    {"Distribution_Account", "90001-031-22"},
                    {"Amount", "300.00"},
                    {"Program_ID", "150"},
                    {"Payment_Type_ID", 2},
                    {"Scholarship_Expense_Account", "49998-900-11"},
                    {"Processor_Fee_Amount", "0.25"}
                },
            };
        }

        private List<GPExportDatum> MockGPExportData()
        {
            return new List<GPExportDatum>
            {
                new GPExportDatum
                {
                    DocumentType = "SALE",
                    DonationId = 10002,
                    BatchName = "Test Batch",
                    DonationDate = new DateTime(2015, 3, 28, 8, 30, 0),
                    DepositDate = new DateTime(2015, 3, 28, 8, 30, 0),
                    CustomerId = "CONTRIBUTI001",
                    DonationAmount = "200.00",
                    CheckbookId = "PNC001",
                    CashAccount = "91213-031-20",
                    ReceivableAccount = "90013-031-21",
                    DistributionAccount = "90001-031-22",
                    Amount = Convert.ToDecimal("185.00")-Convert.ToDecimal(0.13),
                    ProcessorFeeAmount = Convert.ToDecimal(".25"),
                    ProgramId = 15,
                    ProccessFeeProgramId = 127,
                    PaymentTypeId = 8,
                    ScholarshipExpenseAccount = "19998-900-11",
                    ScholarshipPaymentTypeId = 9
                },
                new GPExportDatum
                {
                    DocumentType = "SALE",
                    DonationId = 10002,
                    BatchName = "Test Batch",
                    DonationDate = new DateTime(2015, 3, 28, 8, 30, 0),
                    DepositDate = new DateTime(2015, 3, 28, 8, 30, 0),
                    CustomerId = "CONTRIBUTI001",
                    DonationAmount = "200.00",
                    CheckbookId = "PNC001",
                    CashAccount = "77777-031-20",
                    ReceivableAccount = "77777-031-21",
                    DistributionAccount = "77777-031-22",
                    Amount = Convert.ToDecimal(0.13),
                    ProgramId = 127,
                    ProccessFeeProgramId = 127,
                    PaymentTypeId = 8,
                    ScholarshipExpenseAccount = "19998-900-11",
                    ScholarshipPaymentTypeId = 9,
                },
            };
        }

        [Test]
        public void TestUpdateDepositToExported()
        {
            const int selectionId = 124112312;
            const int depositId = 1245;
            const bool exported = true;

            var expectedParms = new Dictionary<string, object>
            {
                {"Deposit_ID", depositId},
                {"Exported", exported},
            };
            _ministryPlatformService.Setup(mocked => mocked.UpdateRecord(7070, expectedParms, It.IsAny<string>()));
            _ministryPlatformService.Setup(mocked => mocked.RemoveSelection(selectionId, new [] {depositId}, It.IsAny<string>()));

            _fixture.UpdateDepositToExported(selectionId, depositId, "afasdfasdf");
            _ministryPlatformService.VerifyAll();
        }

        [Test]
        public void TestCompleteSendMessageFromDonor()
        {
            var pageId = 341;

            var expectedParams = new Dictionary<string, object>
            {
                {"Communication_ID", 123},
                {"Communication_Status_ID", 3}
            };

            List<Dictionary<string, object>> resultsDict = new List<Dictionary<string, object>>();
            var getResult = new Dictionary<string, object>
                {
                    { "dp_RecordID", 123 },
                    { "Communication_ID", 123 }
            };
            resultsDict.Add(getResult);

            _ministryPlatformService.Setup(mocked => mocked.GetRecordsDict(540, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(resultsDict);
            _ministryPlatformService.Setup(mocked => mocked.UpdateRecord(341, expectedParams, It.IsAny<string>()));
            _ministryPlatformService.Setup(mocked => mocked.DeleteRecord(540, It.IsAny<int>(), It.IsAny<DeleteOption[]>(), It.IsAny<string>())).Returns(1);
            _fixture.FinishSendMessageFromDonor(123,true);
            _ministryPlatformService.VerifyAll();
        }
    }
}
