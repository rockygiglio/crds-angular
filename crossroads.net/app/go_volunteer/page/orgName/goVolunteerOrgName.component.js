(function() {
  'use strict';

  module.exports = GoVolunteerOrgName;

  GoVolunteerOrgName.$inject = [];

  function GoVolunteerOrgName() {
    return {
      restrict: 'E',
      scope: {},
      bindToController: true,
      controller: GoVolunteerOrgNameController,
      controllerAs: 'goOrgName',
      templateUrl: 'orgName/goVolunteerOrgName.template.html'
    };

    function GoVolunteerOrgNameController() {
      var vm = this;

    }
  }

})();
