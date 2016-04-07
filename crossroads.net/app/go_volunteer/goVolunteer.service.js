(function() {
  'use strict';

  module.exports = GoVolunteerService;

  GoVolunteerService.$inject = ['$resource'];

  function GoVolunteerService($resource) {
    var volunteerService =  {
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
      preferredLaunchSite: null,
      projectPrefOne: null,
      projectPrefTwo: null,
      projectPrefThree: null,
      privateGroup: false,
      skills: [],
      spouse: {
        fromDb: false
      },
      spouseAttending: false,
      myPrepTime: false,
      spousePrepTime: false,
      organization: {},
      otherOrgName: null,
      prepWork: [],

      getRegistrationDto: function() {
        return registrationDto();
      }
    };

    function registrationDto() {
      return {
        additionalInformation: volunteerService.additionalInformation,
        children: children(volunteerService.childrenOptions),
        createGroupConnector: createGroupConnector(volunteerService.groupConnectorId),
        equipment: equipment(volunteerService.equipment, volunteerService.otherEquipment),
        groupConnectorId: volunteerService.groupConnectorId,
        // need something for private group connector flag
        initiativeId: 0, //TODO: how will we get this?  user doesn't input, part of CMS page?
        organizationId: volunteerService.organization.organizationId,
        preferredLaunchSiteId: preferredLaunchSite(volunteerService.preferredLaunchSite),
        prepWork: prepWork(volunteerService.myPrepTime, volunteerService.spousePrepTime),
        projectPreferences: projectPreferences(volunteerService.projectPrefOne,
                                               volunteerService.projectPrefTwo,
                                               volunteerService.projectPrefThree),
        self: personDto(volunteerService.person),
        spouse: personDto(volunteerService.spouse),
        spouseParticipation: volunteerService.spouseAttending,
        waiverSigned: true
      };
    }

    return volunteerService;
  }

  function children(options) {
    return _.map(options, function(c) {return getChildrenDto(c); });
  }

  function getChildrenDto(data) {
    var childDto = {};

    if (_.has(data, 'attributeId')) {
      childDto.id = data.attributeId;
    }

    if (_.has(data, 'value')) {
      childDto.count = data.value;
    }

    return childDto;
  }

  function createGroupConnector(groupConnectorId) {
    if (groupConnectorId === null || groupConnectorId === undefined) {
      return true;
    }

    return false;
  }

  function equipment(myEquipment, otherEquipment) {
    debugger;
    var equip = _.map(myEquipment, function(e) { return getEquipmentDto(e); });

    var other = _.map(otherEquipment, function(e) { return getEquipmentDto(e.equipment); });

    debugger;
    if (_.isEmpty(other)) {
      return equip;
    } else {
      return equip.concat(other);
    }
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

  function preferredLaunchSite(siteId) {
    if (siteId === null || siteId === undefined) {
      return 0;
    }

    return siteId;
  }

  function prepWork(myPrepTime, spousePrepTime) {
    var dto = [];
    if (myPrepTime) {
      var my  = {id: myPrepTime.attributeId, spouse: false};
      dto.push(my);
    }

    if (spousePrepTime) {
      var spouse  = {id: spousePrepTime.attributeId, spouse: true};
      dto.push(spouse);
    }

    return dto;
  }

  function projectPreferences(pref1, pref2, pref3) {
    var projectPrefs = [];
    if (pref1 != null) {
      projectPrefs.push(projectPreferenceDto(pref1, 1));
    }

    if (pref2 != null) {
      projectPrefs.push(projectPreferenceDto(pref2, 2));
    }

    if (pref3 != null) {
      projectPrefs.push(projectPreferenceDto(pref3, 3));
    }

    return projectPrefs;
  }

  function projectPreferenceDto(preferenceId, priority) {
    if (preferenceId === null || preferenceId === undefined) {
      return null;
    }

    return {id: preferenceId, priority: priority};
  }

  function personDto(person) {
    var dto = {
      lastName: person.lastName,
      dob: person.dateOfBirth,
      mobile: person.mobilePhone
    };

    if (_.has(person, 'contactId')) {
      dto.contactId = person.contactId;
    }

    if (_.has(person, 'nickName')) {
      dto.firstName = person.nickName;
    }

    if (_.has(person, 'preferredName')) {
      dto.firstName = person.preferredName;
    }

    if (_.has(person, 'emailAddress')) {
      dto.emailAddress = person.emailAddress;
    }

    if (_.has(person, 'email')) {
      dto.emailAddress = person.email;
    }

    return dto;
  }

})();
