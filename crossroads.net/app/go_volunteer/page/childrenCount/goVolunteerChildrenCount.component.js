(function() {
  'use strict';

  module.exports = GoVolunteerChildrenCount;

  GoVolunteerChildrenCount.$inject = [];

  function GoVolunteerChildrenCount() {
    return {
      restrict: 'E',
      scope: {},
      bindToController: true,
      controller: GoVolunteerChildrenCountController,
      controllerAs: 'goChildrenCount',
      templateUrl: 'childrenCount/goVolunteerChildrenCount.template.html'
    };

    function GoVolunteerChildrenCountController() {
      var vm = this;

    }
  }

})();
