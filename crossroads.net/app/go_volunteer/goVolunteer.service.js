(function() {
  'use strict';

  module.exports = GoVolunteerService;

  GoVolunteerService.$inject = ['$resource'];

  function GoVolunteerService($resource) {
    var volunteerService =  {
      // private, don't use these
      cmsInfo: {},
      childrenAttending: {},
      launchSites: {},
      person: {
        nickName: '',
        lastName: '',
        emailAddress: '',
        dateOfBirth: null,
        mobilePhone: null
      },
      preferredLaunchSite: {},
      spouse: {},
      spouseAttending: false,
      organization: {},
      otherOrgName: null
    };

    return volunteerService;
  }

})();
