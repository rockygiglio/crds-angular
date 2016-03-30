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
