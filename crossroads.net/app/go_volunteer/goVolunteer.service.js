(function() {
  'use strict';

  module.exports = GoVolunteerService;

  GoVolunteerService.$inject = ['$resource'];

  function GoVolunteerService($resource) {
    var volunteerService =  {
      // private, don't use these
      cmsInfo: {},
      childrenAttending: {},
      equipment: [],
      otherEquipment: [],
      person: {
        nickName: '',
        lastName: '',
        emailAddress: '',
        dateOfBirth: null,
        mobilePhone: null
      },
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
