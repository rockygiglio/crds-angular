(function() {
  'use strict';

  module.exports = GoVolunteerService;

  GoVolunteerService.$inject = [];

  function GoVolunteerService() {
    var volunteerService =  {
      // private, don't use these
      cmsInfo: {},
      person: {
        nickName: '',
        lastName: '',
        emailAddress: '',
        dateOfBirth: null,
        mobilePhone: null
      },
      spouse: {}
    };

    return volunteerService;
  }

})();
