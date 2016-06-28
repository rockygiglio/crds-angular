using System.Collections.Generic;
using System.Linq;
using crds_angular.Models.Crossroads.Events;
using crds_angular.Services.Interfaces;

namespace crds_angular.Services
{
    public class RoomService : IRoomService
    {
        private readonly MinistryPlatform.Translation.Repositories.Interfaces.IRoomRepository _roomService;
        private readonly MinistryPlatform.Translation.Repositories.Interfaces.IEquipmentRepository _equipmentService;
        private readonly MinistryPlatform.Translation.Repositories.Interfaces.IEventRepository _eventService;

        public RoomService(MinistryPlatform.Translation.Repositories.Interfaces.IRoomRepository roomService,
                           MinistryPlatform.Translation.Repositories.Interfaces.IEquipmentRepository equipmentService,
                           MinistryPlatform.Translation.Repositories.Interfaces.IEventRepository eventService)
        {
            _roomService = roomService;
            _equipmentService = equipmentService;
            _eventService = eventService;
        }

        public List<Room> GetRoomsByLocationId(int id)
        {
            var records = _roomService.GetRoomsByLocationId(id);

            return records.Select(record => new Room
            {
                BuildingId = record.BuildingId,
                Id = record.RoomId,
                LocationId = record.LocationId,
                Name = record.RoomName,
                BanquetCapacity = record.BanquetCapacity,
                Description = record.Description,
                TheaterCapacity = record.TheaterCapacity
            }).OrderBy(x => x.Name).ToList();
        }

        public List<RoomLayout> GetRoomLayouts()
        {
            var records = _roomService.GetRoomLayouts();

            return records.Select(record => new RoomLayout
            {
                Id = record.LayoutId,
                LayoutName = record.LayoutName
            }).ToList();
        }
    }
}