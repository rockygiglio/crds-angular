using System;
using System.Collections.Generic;
using crds_angular.Models.Crossroads.Events;
using crds_angular.Services;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Repositories.Interfaces;
using Moq;
using NUnit.Framework;

namespace crds_angular.test.Services
{
    public class CongregationServiceTest
    {
        private Mock<ICongregationRepository> _congregationRepo;
        private Mock<IRoomService> _roomService;
        private Mock<IEquipmentService> _equipmentService;

        private CongregationService _fixture;

        [SetUp]
        public void Setup()
        {
            _congregationRepo = new Mock<ICongregationRepository>();
            _roomService = new Mock<IRoomService>();
            _equipmentService = new Mock<IEquipmentService>();

            _fixture = new CongregationService(_congregationRepo.Object,
                                               _roomService.Object,
                                               _equipmentService.Object);
        }

        [Test]
        [ExpectedException(typeof(ApplicationException), ExpectedMessage = "Congregation Not Found")]
        public void GetRoomsShouldThrowExceptionIfNoCongregations()
        {
            _congregationRepo.Setup(m => m.GetCongregationById(1)).Returns((MpCongregation) null);

            _fixture.GetRooms(1, DateTime.Now, DateTime.Now);
        }

        [Test]
        public void GetRoomsShouldReturnRooms()
        {
            var date = DateTime.Now;
            var rooms = GetRooms();
            _congregationRepo.Setup(m => m.GetCongregationById(1)).Returns(new MpCongregation {Name = "Oakley", CongregationId = 1, LocationId = 1});

            _roomService.Setup(m => m.GetRoomsByLocationId(1, date, date)).Returns(rooms);

            var result = _fixture.GetRooms(1, date, date);
            
            Assert.AreEqual(result.Count, rooms.Count);
            Assert.AreEqual(result[0].Name, "Roomy McRoomFace");
        }

        private List<Room> GetRooms()
        {
            return new List<Room>
            {
                new Room()
                {
                    Name = "Roomy McRoomFace",
                    Id = 1,
                    Description = "This is the name that won...",
                    BanquetCapacity = 20,
                    TheaterCapacity = 50,
                    LocationId = 1,
                    BuildingId = 1, 
                    DisplayName = "Kerstanoff, Joe",
                    RoomStatus = true
                }, new Room()
                {
                    Name = "Tribute to the best room in the world",
                    Id = 2,
                    Description = "This is just a tribute",
                    BanquetCapacity = 42,
                    TheaterCapacity = 42,
                    LocationId = 1,
                    BuildingId = 1,
                    DisplayName = null,
                    RoomStatus = null
                }, new Room()
                {
                    Name = "Pending Room",
                    Id = 3,
                    Description = "This room has a pending reservation",
                    BanquetCapacity = 1,
                    TheaterCapacity = 0,
                    LocationId = 1,
                    BuildingId = 1,
                    DisplayName = "Nukem, Duke",
                    RoomStatus = false
                }
            };
        }
    }
}