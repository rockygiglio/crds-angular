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

  function GoVolunteerService($resource) {
    var volunteerService =  {
      getRegistrationDto: function(data) {
        return {
          additionalInformation: data.additionalInformation,
          createGroupConnector: data.groupConnectorId
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
