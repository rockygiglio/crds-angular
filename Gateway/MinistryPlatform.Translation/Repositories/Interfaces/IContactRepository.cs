using System;
using System.Collections.Generic;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Models.MinistryPlatform.Translation.Models;

namespace MinistryPlatform.Translation.Repositories.Interfaces
{
    public interface IContactRepository
    {
        string GetContactEmail(int contactId);
        int GetContactId(string token);
        MpMyContact GetContactById(int contactId);
        MpMyContact GetContactByIdCard(string idCard);
        int GetContactIdByParticipantId(int participantId);
        List<MpHouseholdMember> GetHouseholdFamilyMembers(int householdId);
        List<MpHouseholdMember> GetOtherHouseholdMembers(int contactId);
        MpMyContact GetMyProfile(string token);
        int CreateContactForGuestGiver(string emailAddress, string displayName, string firstName = null, string lastName = null);
        int CreateContactForSponsoredChild(string firstName, string lastName, string town, string idCard);
        int CreateContactForNewDonor(MpContactDonor mpContactDonor);
        IList<int> GetContactIdByRoleId(int roleId, string token);
        void UpdateContact(int contactId, Dictionary<string, object> profileDictionary, Dictionary<string, object> householdDictionary, Dictionary<string, object> addressDictionary);
        void UpdateContact(int contactId, Dictionary<string, object> profileDictionary);
        void UpdateHouseholdAddress(int contactId, Dictionary<string, object> householdDictionary, Dictionary<string, object> addressDictionary);
        int GetContactIdByEmail(string email);
        MpMyContact GetContactByParticipantId(int participantId);
        List<Dictionary<string, object>> PrimaryContacts(bool staffOnly = false);
        MpContact CreateSimpleContact(string firstName, string lastName, string email, string dob, string mobile);
        List<MpRecordID> CreateContact(MpContact minorContact);
        MpMyContact GetContactByUserRecordId(int userRecordId);
        IObservable<MpHousehold> UpdateHousehold(MpHousehold household);
        void SetHouseholdAddress(int contactId, int householdId, int addressId);
        List<MpMyContact> GetPotentialMatchesContact(string firstName, string lastName, string email);
    }
}