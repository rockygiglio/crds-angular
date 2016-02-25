using System;
using System.Collections.Generic;
using crds_angular.Models.Crossroads.Attribute;
using crds_angular.Models.Crossroads.Profile;
using crds_angular.Services;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services;
using GateWayInterfaces = crds_angular.Services.Interfaces;
using Moq;
using NUnit.Framework;
using Rhino.Mocks;
using MPInterfaces = MinistryPlatform.Translation.Services.Interfaces;

namespace crds_angular.test.Services
{
    public class ContactAttributeServiceTest
    {

        private ContactAttributeService _fixture;
        private Mock<MPInterfaces.IApiUserService> _apiUserService;
        private Mock<MPInterfaces.IObjectAttributeService> _contactAttributeService;
        private Mock<GateWayInterfaces.IAttributeService> _attributeService;
        private Mock<MPInterfaces.IAttributeService> _mpAttributeService;
        private List<ContactSingleAttributeDTO> _updatedAttributes = new List<ContactSingleAttributeDTO>();
        private List<ObjectAttribute> _currentAttributes = new List<ObjectAttribute>();

        private int _fakeContactId = 2186211;
        private string _fakeToken = "afaketoken";
        private string _updatedNote = "New and Updated Notes";
        private DateTime _startDate = new DateTime(2015, 2, 21);

        [SetUp]
        public void Setup()
        {
            _mpAttributeService = new Mock<MPInterfaces.IAttributeService>(MockBehavior.Strict);
            _contactAttributeService = new Mock<MPInterfaces.IObjectAttributeService>();
            _attributeService = new Mock<GateWayInterfaces.IAttributeService>();
            _apiUserService = new Mock<MPInterfaces.IApiUserService>();
            _mpAttributeService = new Mock<MPInterfaces.IAttributeService>();

            _fixture = new ContactAttributeService(_contactAttributeService.Object, _attributeService.Object, _apiUserService.Object, _mpAttributeService.Object);
            _updatedAttributes.Add(new ContactSingleAttributeDTO
            {
                Value = new AttributeDTO
                {
                    AttributeId = 23,
                    Category = "Allergies",
                    Name = "All Allergies"
                },
                Notes = "New and Updated Notes"
 
            });  
            _currentAttributes.Add(new ObjectAttribute
            {
                AttributeTypeId = 2,
                AttributeId = 23,
                AttributeTypeName = "Allergies",
                ObjectAttributeId = 123456,
                StartDate = _startDate,
                Notes = "original notes"
            });
        }

        [Test]
        public void ShouldUpdatePreviouslySavedAttributeNotes()
        {
            var contactAttributes = new Dictionary<int, ContactAttributeTypeDTO>();

            var contactSingleAttributes = new Dictionary<int, ContactSingleAttributeDTO>
            {
                {1, new ContactSingleAttributeDTO
                    {
                        Value = new AttributeDTO
                        {
                            AttributeId = 23,
                            Category = "Allergies",
                            Name = "All Allergies"
                        },
                        Notes = _updatedNote
 
                    } 
                }
            };
            
            _contactAttributeService.Setup(x => x.GetCurrentContactAttributes(_fakeToken, _fakeContactId, It.IsAny<ObjectAttributeConfiguration>(), null)).Returns(_currentAttributes);
            _apiUserService.Setup(x => x.GetToken()).Returns(_fakeToken);
            _contactAttributeService.Setup(x => x.UpdateAttribute(_fakeToken, It.IsAny<ObjectAttribute>(), It.IsAny<ObjectAttributeConfiguration>())).Callback<string, ObjectAttribute, ObjectAttributeConfiguration>((id, actual, objectConfiguration) =>
            {
                Assert.AreEqual(actual.Notes, _updatedNote);  
                Assert.AreEqual(actual.ObjectAttributeId, 123456);
            });
            _fixture.SaveContactAttributes(_fakeContactId, contactAttributes, contactSingleAttributes);
            _apiUserService.VerifyAll();
            _attributeService.VerifyAll();
            _mpAttributeService.VerifyAll();
            _contactAttributeService.Verify(update => update.UpdateAttribute(_fakeToken, It.IsAny<ObjectAttribute>(), It.IsAny<ObjectAttributeConfiguration>()), Times.Once);
            

        }

        
    }
}
