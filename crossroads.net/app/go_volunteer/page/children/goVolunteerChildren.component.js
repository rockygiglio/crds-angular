(function() {
  'use strict';

  module.exports = GoVolunteerChildren;

  GoVolunteerChildren.$inject = [];

  function GoVolunteerChildren() {
    return {
      restrict: 'E',
      scope: {},
      bindToController: true,
      controller: GoVolunteerChildrenController,
      controllerAs: 'goChildren',
      templateUrl: 'children/goVolunteerChildren.template.html'
    };

    function GoVolunteerChildrenController() {
      var vm = this;

    }
  }

})();
