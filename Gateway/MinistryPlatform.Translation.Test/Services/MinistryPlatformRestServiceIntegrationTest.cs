﻿using System;
using System.Collections.Generic;
using System.Linq;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Models.Attributes;
using MinistryPlatform.Translation.Models.Childcare;
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
            var results = _fixture.UsingAuthenticationToken(_authToken).PostStoredProc("api_crds_Add_As_TripParticipant", fields);
            Console.WriteLine("Results\t" + results.ToString());
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
            var results = _fixture.UsingAuthenticationToken(_authToken).Search<MpPledge>("Donor_ID_Table_Contact_ID_Table.Contact_ID=2186211 AND Pledge_Campaign_ID_Table.Pledge_Campaign_ID=10000000",
                "Pledges.Pledge_ID,Donor_ID_Table.Donor_ID,Pledge_Campaign_ID_Table.Pledge_Campaign_ID,Pledge_Campaign_ID_Table.Campaign_Name,Pledge_Campaign_ID_Table_Pledge_Campaign_Type_ID_Table.Pledge_Campaign_Type_ID,Pledge_Campaign_ID_Table_Pledge_Campaign_Type_ID_Table.Campaign_Type,Pledge_Campaign_ID_Table.Start_Date,Pledge_Campaign_ID_Table.End_Date,Pledge_Status_ID_Table.Pledge_Status_ID,Pledge_Status_ID_Table.Pledge_Status,Pledges.Total_Pledge,Donor_ID_Table_Contact_ID_Table.Contact_ID");

            foreach (var p in results)
            {
                Console.WriteLine("CampaignName:\t{0}", p.CampaignName);
                Console.WriteLine("DonorId:\t{0}", p.DonorId);
                Console.WriteLine("PledgeId:\t{0}", p.PledgeId);
                Console.WriteLine("PledgeDonations:\t{0}", p.PledgeDonations);
            }
        }

        [Test]
        public void TestGetPaymentType()
        {
            Console.WriteLine("TestGetPaymentType");
            var p = _fixture.UsingAuthenticationToken(_authToken).Get<MyPaymentType>(2);
            Console.WriteLine("Payment_Type\t{0}", p);
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
