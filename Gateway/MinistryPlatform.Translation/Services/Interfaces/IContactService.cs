using System.Collections.Generic;
using MinistryPlatform.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IContactService
    {
        string GetContactEmail(int contactId);
        MyContact GetContactById(int contactId);
        MyContact GetContactByIdCard(string idCard);
        List<HouseholdMember> GetHouseholdFamilyMembers(int householdId);
        MyContact GetMyProfile(string token);
        int CreateContactForGuestGiver(string emailAddress, string displayName);
        int CreateContactForSponsoredChild(string firstName, string lastName, string idCard);
        int CreateContactForNewDonor(ContactDonor contactDonor);
        IList<int> GetContactIdByRoleId(int roleId, string token);
        void UpdateContact(int contactId, Dictionary<string, object> profileDictionary, Dictionary<string, object> householdDictionary, Dictionary<string, object> addressDictionary);
        void UpdateContact(int contactId, Dictionary<string, object> profileDictionary);
    }
}