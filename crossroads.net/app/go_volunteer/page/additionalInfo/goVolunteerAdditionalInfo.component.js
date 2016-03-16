(function() {
  'use strict';

  module.exports = GoVolunteerAdditionalInfo;

  GoVolunteerAdditionalInfo.$inject = [];

  function GoVolunteerAdditionalInfo() {
    return {
      restrict: 'E',
      scope: {},
      bindToController: true,
      controller: GoVolunteerAdditionalInfoController,
      controllerAs: 'goAdditionalInfo',
      templateUrl: 'additionalInfo/goVolunteerAdditionalInfo.template.html'
    };

    function GoVolunteerAdditionalInfoController() {
      var vm = this;

    }
  }

})();
