using System.Collections.Generic;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Models.EventReservations;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IRoomService
    {
        int CreateRoomReservation(MpRoomReservationDto roomReservation, string token);
        List<Room> GetRoomsByLocationId(int locationId);
        List<RoomLayout> GetRoomLayouts();
        List<MpRoomReservationDto> GetRoomReservations(int eventId);
        void UpdateRoomReservation(MpRoomReservationDto roomReservation, string token);
        void DeleteRoomReservation(MpRoomReservationDto roomReservation, string token);
        void DeleteEventRoomsForEvent(int eventId, string token);
    }
}