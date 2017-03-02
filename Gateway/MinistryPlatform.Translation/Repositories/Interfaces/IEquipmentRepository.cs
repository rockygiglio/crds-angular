using System.Collections.Generic;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Models.EventReservations;

namespace MinistryPlatform.Translation.Repositories.Interfaces
{
    public interface IEquipmentRepository
    {
        int CreateEquipmentReservation(MpEquipmentReservationDto equipmentReservation);
        List<Equipment> GetEquipmentByLocationId(int locationId);
        List<MpEquipmentReservationDto> GetEquipmentReservations(int eventId, int roomId);
        void UpdateEquipmentReservation(MpEquipmentReservationDto equipmentReservation);
    }
}