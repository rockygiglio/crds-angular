(function() {
  'use strict';

  module.exports = GoVolunteerLaunchSite;

  GoVolunteerLaunchSite.$inject = ['GoVolunteerService'];

  function GoVolunteerLaunchSite(GoVolunteerService) {
    return {
      restrict: 'E',
      scope: {
        onSubmit: '&'
      },
      bindToController: true,
      controller: GoVolunteerLaunchSiteController,
      controllerAs: 'goLaunchSite',
      templateUrl: 'launchSite/goVolunteerLaunchSite.template.html'
    };

    function GoVolunteerLaunchSiteController() {
      var vm = this;
      vm.locations = GoVolunteerService.launchSites;
      vm.submit = submit;

      function submit(locationId) {
        GoVolunteerService.preferredLaunchSite = locationId;
        vm.onSubmit({nextState: 'project-preference-one'});
      }
    }
  }

})();
