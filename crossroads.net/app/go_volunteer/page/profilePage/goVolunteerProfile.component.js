(function() {
  'use strict';

  module.exports = VolunteerProfile;

  VolunteerProfile.$Inject = ['GoVolunteerService'];

  function VolunteerProfile(GoVolunteerService) {
    return {
      restrict: 'E',
      scope: {},
      bindToController: true,
      controller: VolunteerProfileController,
      controllerAs: 'volunteerProfile',
      templateUrl: 'profilePage/goVolunteerProfile.template.html'
    };
    
    
    function VolunteerProfileController() {
      var vm = this;
      vm.person = GoVolunteerService.person;
      console.log(GoVolunteerService.person);
    }
  }

})();
