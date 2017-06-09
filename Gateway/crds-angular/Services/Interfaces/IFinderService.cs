using System.Collections.Generic;
using System.Device.Location;
using crds_angular.Models.AwsCloudsearch;
using crds_angular.Models.Finder;
using crds_angular.Models.Crossroads;
using crds_angular.Models.Crossroads.Groups;

namespace crds_angular.Services.Interfaces
{
    public interface IFinderService
    {
        PinDto GetPinDetailsForPerson(int participantId);
        PinDto GetPinDetailsForGroup(int groupId);
        void EnablePin(int participantId);
        void DisablePin(int participantId);
        void UpdateHouseholdAddress(PinDto pin);
        AddressDTO GetAddressForIp(string ip);
        List<PinDto> GetMyPins(string token, GeoCoordinate originCoords, int contactId, string finderType);
        List<PinDto> GetMyGroupPins(string token, int[] groupTypeIds, int participantId, string finderType);
        int GetParticipantIdFromContact(int contactId);
        List<PinDto> GetPinsInBoundingBox(GeoCoordinate originCoords, string address, AwsBoundingBox boundingBox, string finderType, int contactId);
        AddressDTO RandomizeLatLong(AddressDTO address);
        GeoCoordinate GetGeoCoordsFromAddressOrLatLang(string address, string lat, string lng);
        GeoCoordinate GetGeoCoordsFromLatLong(string lat, string lng);
        void GatheringJoinRequest(string token, int gatheringId);
        Invitation InviteToGroup(string token, int gatheringId, User person, string finderFlag);
        List<GroupParticipantDTO> GetParticipantsForGroup(int groupId);
        AddressDTO GetGroupAddress(string token, int groupId);
        AddressDTO GetPersonAddress(string token, int participantId);
        PinDto UpdateGathering(PinDto pin);
        void RequestToBeHost(string token, HostRequestDto hostRequest);
        void AcceptDenyGroupInvitation(string token, int groupId, string invitationGuid, bool accept);
        void SayHi(int fromContactId, int toContactId);
        void AddUserDirectlyToGroup(User user, int groupid);
    }
}