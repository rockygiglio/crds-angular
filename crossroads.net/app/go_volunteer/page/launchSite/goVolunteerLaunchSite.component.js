(function() {
  'use strict';

  module.exports = GoVolunteerLaunchSite;

  GoVolunteerLaunchSite.$inject = [];

  function GoVolunteerLaunchSite() {
    return {
      restrict: 'E',
      scope: {},
      bindToController: true,
      controller: GoVolunteerLaunchSiteController,
      controllerAs: 'goLaunchSite',
      templateUrl: 'launchSite/goVolunteerLaunchSite.template.html'
    };

    function GoVolunteerLaunchSiteController() {
      var vm = this;

    }
  }

})();
