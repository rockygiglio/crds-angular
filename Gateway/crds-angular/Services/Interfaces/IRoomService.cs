using System;
using System.Collections.Generic;
using crds_angular.Models.Crossroads.Events;

namespace crds_angular.Services.Interfaces
{
    public interface IRoomService
    {
        List<Room> GetRoomsByLocationId(int id, DateTime startDate, DateTime endDate);
        List<RoomLayout> GetRoomLayouts();
    }
}