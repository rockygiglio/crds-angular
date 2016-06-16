using System.Collections.Generic;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Models.EventReservations;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IEquipmentRepository
    {
        int CreateEquipmentReservation(MpEquipmentReservationDto equipmentReservation, string token);
        List<Equipment> GetEquipmentByLocationId(int locationId);
        List<MpEquipmentReservationDto> GetEquipmentReservations(int eventId, int roomId);
        void UpdateEquipmentReservation(MpEquipmentReservationDto equipmentReservation, string token);
    }
}