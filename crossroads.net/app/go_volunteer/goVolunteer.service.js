(function() {
  'use strict';

  module.exports = GoVolunteerService;

  GoVolunteerService.$inject = ['$resource'];

  function createGroupConnector(groupConnectorId) {
    if (groupConnectorId === null || groupConnectorId === undefined) {
      return true;
    }

    return false;
  }

  function equipment(myEquipment, otherEquipment) {
    var equip = _.map(myEquipment, function(e) { return getEquipmentDto(e.equipment); });

    var other = _.map(otherEquipment, function(e) { return getEquipmentDto(e.equipment); });

    var dto = equip.concat(other);

    return dto;
  }

  function getEquipmentDto(equipment) {
    var equipmentDto = {};

    if (_.has(equipment, 'attributeId')) {
      equipmentDto.id = equipment.attributeId;
    }

    if (_.has(equipment, 'name')) {
      equipmentDto.notes = equipment.name;
    }

    return equipmentDto;
  }

  function prepWork(myPrepTime, spousePrepTime) {
    var dto = [];
    if (myPrepTime) {
      var my  = {id: myPrepTime.attributeId, spouse: false};
      dto.add(my);
    }

    if (spousePrepTime) {
      var spouse  = {id: spousePrepTime.attributeId, spouse: true};
      dto.add(spouse);
    }

    return dto;
  }

  function projectPreferences(pref1, pref2, pref3) {
    return [
      projectPreferenceDto(pref1, 1),
      projectPreferenceDto(pref2, 2),
      projectPreferenceDto(pref3, 3)
    ];
  }

  function projectPreferenceDto(preferenceId, priority) {
    return {id: preferenceId, priority: priority};
  }

  function self(person) {
    var dto = {};
    if (_.has(person, 'contactId')) {
      dto.contactId = person.contactId;
    }
  }

  function GoVolunteerService($resource) {
    var volunteerService =  {
      getRegistrationDto: function(data) {
        return {
          additionalInformation: data.additionalInformation,
          createGroupConnector: createGroupConnector(data.groupConnectorId),
          equipment: equipment(data.equipment, data.otherEquipment),
          groupConnectorId: data.groupConnectorId,
          // need something for private group connector flag
          initiativeId: 0, // how will we get this?  user doesn't input, part of CMS page?
          organizationId: data.organizationId, // really location
          preferredLaunchSiteId: data.preferredLaunchSite,
          prepWork: prepWork(data.myPrepTime, data.spousePrepTime),
          projectPreferences: projectPreferences(data.projectPrefOne, data.projectPrefTwo, data.projectPrefThree),
          self: self(data.person)
        };
      },

      // private, don't use these
      cmsInfo: {},
      childrenAttending: {
        childTwoSeven: 0,
        childEightTwelve: 0,
        childThirteenEighteen: 0
      },
      equipment: [],
      otherEquipment: [],
      launchSites: {},
      person: {
        nickName: '',
        lastName: '',
        emailAddress: '',
        dateOfBirth: null,
        mobilePhone: null
      },
      preferredLaunchSite: {},
      projectPrefOne: {},
      projectPrefTwo: {},
      projectPrefThree: {},
      privateGroup: false,
      skills: [],
      spouse: {},
      spouseAttending: false,
      myPrepTime: false,
      spousePrepTime: false,
      organization: {},
      otherOrgName: null,
      prepWork: [],

    };

    return volunteerService;
  }

})();
