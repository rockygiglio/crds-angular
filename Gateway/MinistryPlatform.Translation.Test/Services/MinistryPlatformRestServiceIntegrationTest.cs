using System;
using System.Collections.Generic;
using System.Linq;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Models.Attributes;
using MinistryPlatform.Translation.Models.Childcare;
using MinistryPlatform.Translation.Models.Payments;
using MinistryPlatform.Translation.Repositories;
using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;

namespace MinistryPlatform.Translation.Test.Services
{
    [Category("IntegrationTests")]
    public class MinistryPlatformRestServiceIntegrationTest
    {
        private MinistryPlatformRestRepository _fixture;

        private string _authToken;

        [TestFixtureSetUp]
        public void SetupAll()
        {
            var auth = AuthenticationRepository.authenticate(Environment.GetEnvironmentVariable("API_USER"), Environment.GetEnvironmentVariable("API_PASSWORD"));
            _authToken = auth["token"].ToString();
        }

        [SetUp]
        public void SetUp()
        {
            var restClient = new RestClient(Environment.GetEnvironmentVariable("MP_REST_API_ENDPOINT"));
            _fixture = new MinistryPlatformRestRepository(restClient);
        }

        [Test]
        public void TestChildcareDashboardProcedure()
        {
            Console.WriteLine("TestCallingAStoredProcedure");
            var parms = new Dictionary<string, object>()
            {
                {"@Domain_ID", 1},
                {"@Contact_ID", 2186211}
            };
            var results = _fixture.UsingAuthenticationToken(_authToken).GetFromStoredProc<MpChildcareDashboard>("api_crds_getChildcareDashboard", parms);
            foreach (var p in results)
            {               
                Console.WriteLine("Result\t{0}", p.FirstOrDefault());
            }
        }
        
        [Test]
        public void TestChildRsvpdProcedure()
        {
            Console.WriteLine("TestCallingAStoredProcedure");
            var parms = new Dictionary<string, object>()
            {
                {"@ContactID", 100030266},
                {"@EventGroupID", 172309}
            };
            var results = _fixture.UsingAuthenticationToken(_authToken).GetFromStoredProc<MPRspvd>("api_crds_childrsvpd", parms);
            foreach (var p in results)
            {
                Console.WriteLine("Result\t{0}", p.FirstOrDefault().Rsvpd);
            }
        }

        [Test]
        public void TestChildcareEmailProcedure()
        { 
            Console.WriteLine("TestCallingAStoredProcedure");
            var parms = new Dictionary<string, object>()
            {                
                {"@DaysOut", 4}
            };
            var results = _fixture.UsingAuthenticationToken(_authToken).GetFromStoredProc<MpContact>("api_crds_ChildcareReminderEmails", parms);
            foreach (var p in results)
            {
                Console.WriteLine("Result\t{0}", p.FirstOrDefault().EmailAddress);
            }
        }

        [Test]
        public void TestChildcareRequestDatesProcedure()
        {
            Console.WriteLine("TestChildcareRequestDatesProcedure");
            var parms = new Dictionary<string, object>()
            {
                {"@ChildcareRequestID", 179}
            };
            var results = _fixture.UsingAuthenticationToken(_authToken).PostStoredProc("api_crds_DeleteDatesForChildcareRequest", parms);

            Console.WriteLine("Results\t" + results.ToString());

        }

        [Test]
        public void TestEndDateGroup()
        {
            Console.WriteLine("TestEndDateGroup");
            var groupId = 172501;
            var reasonEnded = 1;
            var fields = new Dictionary<string, object>
            {
                {"Group_ID", groupId },
                {"End_Date", DateTime.Today},
                {"Reason_Ended", reasonEnded}
            };
            _fixture.UsingAuthenticationToken(_authToken).UpdateRecord("Groups", groupId, fields);
        }

        [Test]
        public void TestTripParticipantProcedure()
        {
            Console.WriteLine("TestTripParticipantProcedure");
            var fields = new Dictionary<string, object>
            {
                {"@PledgeCampaignID", 10000000},
                {"@ContactID", 2186211 }
            };
            var results = _fixture.UsingAuthenticationToken(_authToken).GetFromStoredProc<MpPledge>("api_crds_Add_As_TripParticipant", fields);
            Console.WriteLine("Result\t" + results.ToString());
        }

        [Test]
        public void TestGetEvents()
        {
            const int eventId = 4525285;
            Console.WriteLine("Getting Event");
            var results = _fixture.UsingAuthenticationToken(_authToken).Search<MpCamp>($"Event_ID={eventId}");

            Assert.AreEqual(results.Count, 1);

            foreach (var p in results)
            {
                Console.WriteLine("Event\t{0}", p);
            }

        }

        [Test]
        public void TestSearchAllPaymentTypes()
        {
            Console.WriteLine("TestSearchAllPaymentTypes");
            var results = _fixture.UsingAuthenticationToken(_authToken).Search<MyPaymentType>();

            foreach (var p in results)
            {
                Console.WriteLine("Payment_Type\t{0}", p);
            }
        }

        [Test]
        public void TestSearchPaymentTypes()
        {
            Console.WriteLine("TestSearchPaymentTypes");
            var results = _fixture.UsingAuthenticationToken(_authToken).Search<MyPaymentType>("Payment_Type_Id > 5");

            foreach (var p in results)
            {
                Console.WriteLine("Payment_Type\t{0}", p);
            }
        }

        [Test]
        public void TestSearchPaymentTypesSelectColumns()
        {
            Console.WriteLine("TestSearchPaymentTypesSelectColumns");
            var results = _fixture.UsingAuthenticationToken(_authToken).Search<MyPaymentType>("Payment_Type_Id > 5", "Payment_Type_Id,Payment_Type");

            foreach (var p in results)
            {
                Console.WriteLine("Payment_Type\t{0}", p);
            }
        }

        [Test]
        public void TestSearchPledgesByContactAndCampaign()
        {
            Console.WriteLine("TestSearchPledgesByContactAndCampaign");
            var results = _fixture.UsingAuthenticationToken(_authToken).Search<MpPledge>("Donor_ID_Table_Contact_ID_Table.Contact_ID=2186211 AND Pledge_Campaign_ID_Table.Pledge_Campaign_ID=10000000 AND Pledge_Status_ID_Table.Pledge_Status_ID=1",
                "Pledges.Pledge_ID,Donor_ID_Table.Donor_ID,Pledge_Campaign_ID_Table.Pledge_Campaign_ID,Pledge_Campaign_ID_Table.Campaign_Name,Pledge_Campaign_ID_Table_Pledge_Campaign_Type_ID_Table.Pledge_Campaign_Type_ID,Pledge_Campaign_ID_Table_Pledge_Campaign_Type_ID_Table.Campaign_Type,Pledge_Campaign_ID_Table.Start_Date,Pledge_Campaign_ID_Table.End_Date,Pledge_Status_ID_Table.Pledge_Status_ID,Pledge_Status_ID_Table.Pledge_Status,Pledges.Total_Pledge");

            foreach (var p in results)
            {
                Console.WriteLine("CampaignName:\t{0}", p.CampaignName);
                Console.WriteLine("DonorId:\t{0}", p.DonorId);
                Console.WriteLine("PledgeId:\t{0}", p.PledgeId);
                Console.WriteLine("PledgeDonations:\t{0}", p.PledgeDonations);
            }
        }

        [Test]
        public void TestGetGoTripsWithForms()
        {
            var pledgeCampaignId = 10000000;
            var columnList = new List<string>
            {
                "Pledge_Campaigns.Pledge_Campaign_ID",
                "Pledge_Campaigns.Campaign_Name",
                "Pledge_Campaign_Type_ID_Table.Campaign_Type",
                "Pledge_Campaigns.Start_Date",
                "Pledge_Campaigns.[End_Date]",
                "Pledge_Campaigns.[Campaign_Goal]",
                "Registration_Form_Table.[Form_ID]",
                "Registration_Form_Table.[Form_Title]",
                "Pledge_Campaigns.[Registration_Start]",
                "Pledge_Campaigns.[Registration_End]",
                "Pledge_Campaigns.[Registration_Deposit]",
                "Pledge_Campaigns.[Youngest_Age_Allowed]",
                "Event_ID_Table.[Event_Start_Date]",
                "Pledge_Campaigns.[Nickname]",
                "Event_ID_Table.[Event_ID]",
                "Pledge_Campaigns.[Program_ID]",
                "Pledge_Campaigns.Maximum_Registrants"
            };

            var results = _fixture.UsingAuthenticationToken(_authToken).Search<MpPledgeCampaign>($"Pledge_Campaigns.[Pledge_Campaign_ID] = {pledgeCampaignId}", columnList);
            Assert.AreEqual(1, results.Count);
            Assert.IsNotNull(results[0].MaximumRegistrants);
        }

        [Test]
        public void TestGetPaymentType()
        {
            Console.WriteLine("TestGetPaymentType");
            var p = _fixture.UsingAuthenticationToken(_authToken).Get<MyPaymentType>(2);
            Console.WriteLine("Payment_Type\t{0}", p);
        }

        [Test]
        public void ShouldGetDataFromTableByName()
        {
            var contact = _fixture.UsingAuthenticationToken(_authToken).Get<MpContact>("Pledges", 656098, "Donor_ID_Table_Contact_ID_Table.Nickname, Donor_ID_Table_Contact_ID_Table.Last_Name");
            Assert.AreEqual("Andy", contact.Nickname);
            Assert.AreEqual("Canterbury", contact.LastName);
        }

        [Test]
        public void GradeGroupsInCurrentCampEvents()
        {
            var columnList = new List<string>
            {
                "Event_ID_Table_Event_Type_ID_Table.[Event_Type_ID]",
                "Group_ID_Table.[Group_ID]",
                "Group_ID_Table_Group_Type_ID_Table.[Group_Type_ID]"
            };

            var date = DateTime.Today;

            var filter = "Event_ID_Table_Event_Type_ID_Table.[Event_Type_ID] = 8 AND Group_ID_Table_Group_Type_ID_Table.[Group_Type_ID] = 4 " +
                         $"AND '{date}' between Event_ID_Table.[Registration_Start] and Event_ID_Table.[Registration_End]";
            var groups = _fixture.UsingAuthenticationToken(_authToken).Search<MpEventGroup>(filter, columnList);
            foreach (MpEventGroup eg in groups)
            {
                Console.WriteLine(eg);
            };
        }

        [Test]
        public void ContactNotInGradeGroup()
        {
            var storedProcOpts = new Dictionary<string, object>
            {
                {"@ContactID", 1234 },
                {"@EventID", 4525285}
            };
            var result = _fixture.UsingAuthenticationToken(_authToken).GetFromStoredProc<MpStoredProcBool>("api_crds_Grade_Group_Participant_For_Camps", storedProcOpts);
            var l = result.FirstOrDefault();
            foreach (var r in l)
            {
                Assert.IsFalse(r.isTrue);                
            }
        }

        [Test]
        public void ContactInAGradeGroup()
        {
            var storedProcOpts = new Dictionary<string, object>
            {
                {"@ContactID", 7672203},
                {"@EventID", 4525325}
            };
            var result = _fixture.UsingAuthenticationToken(_authToken).GetFromStoredProc<MpStoredProcBool>("api_crds_Grade_Group_Participant_For_Camps", storedProcOpts);
            var l = result.FirstOrDefault();
            foreach (var r in l)
            {
                Assert.IsTrue(r.isTrue);
            }
        }
        
        public void ShouldCreateARecord()
        {
            var payment = new MpPayment
            {
                PaymentTotal = 123.45,
                ContactId = 3717387,
                PaymentDate = DateTime.Now,
                PaymentTypeId = 11
            };

            var paymentDetail = new MpPaymentDetail
            {
                Payment = payment,
                PaymentAmount = 123.45,
                InvoiceDetailId = 19
            };
            var resp = _fixture.UsingAuthenticationToken(_authToken).Post(new List<MpPaymentDetail> {paymentDetail});

        }

        [Test]
        public void ShouldUpdate2GenericRecord()
        {
            var tableName = "Invoices";

            var dict = new Dictionary<string, object>();
            dict.Add("Invoice_ID",8);
            dict.Add("Invoice_Status_ID", 2);

            var dict2 = new Dictionary<string, object>();
            dict2.Add("Invoice_ID", 9);
            dict2.Add("Invoice_Status_ID", 2);

            var thelist = new List<Dictionary<string, object>>();
            thelist.Add(dict);
            thelist.Add(dict2);

            var resp = _fixture.UsingAuthenticationToken(_authToken).Put(tableName,thelist);
        }

        [Test]
        public void TestPaymentsForInvoiceProcedure()
        {
            Console.WriteLine(" TestPaymentsForInvoiceProcedure");
            var invoiceId = 1;
            var fields = new Dictionary<string, object>
            {
                {"@InvoiceId", invoiceId }
            };
            var results = _fixture.UsingAuthenticationToken(_authToken).GetFromStoredProc<MpPayment>("api_crds_PaymentsForInvoice", fields);
            Console.WriteLine("Result\t" + results.ToString());
        }

        [Test]
        public void TestGetWithFilter()
        {
            Console.WriteLine(" TestGetWithFilter");
            var invoiceId = 8;
            var fields = new Dictionary<string, object>
            {
                {"Invoice_Number", invoiceId }
            };
            var results = _fixture.UsingAuthenticationToken(_authToken).Get<MpPayment>("Payments", fields);
            Console.WriteLine("Result\t" + results.ToString());
        }

        [Test]
        public void TestGetASingleIntValue()
        {
            var contactId = 7681520;
            var eventId = 4525325;
            var tableName = "Event_Participants";
            var searchString = $"Event_ID_Table.Event_ID={eventId} AND Participant_ID_Table_Contact_ID_Table.Contact_ID={contactId}";
            var column = "Event_Participant_ID";
            var results = _fixture.UsingAuthenticationToken(_authToken).Search<int>(tableName, searchString, column);
        }

        [Test]
        public void TestGetASingleStringValue()
        {
            var contactId = 7681520;
            var eventId = 4525325;
            var tableName = "Event_Participants";
            var searchString = $"Event_ID_Table.Event_ID={eventId} AND Participant_ID_Table_Contact_ID_Table.Contact_ID={contactId}";
            var column = "Event_ID_Table.Event_Title";
            var results = _fixture.UsingAuthenticationToken(_authToken).Search<string>(tableName, searchString, column);
        }

    }

    [MpRestApiTable(Name = "Payment_Types")]
    public class MyPaymentType
    {
        [JsonProperty(PropertyName = "Payment_Type_ID")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "Payment_Type")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "Payment_Type_Code")]
        public string Code { get; set; }
        [JsonProperty(PropertyName = "__ExternalPaymentTypeID", NullValueHandling = NullValueHandling.Ignore)]
        public int? LegacyId { get; set; }

        public override string ToString()
        {
            return string.Format("Id: {0}, Name: {1}, Description: {2}, Code: {3}, LegacyId: {4}", Id, Name, Description, Code, LegacyId);
        }
    }
}
