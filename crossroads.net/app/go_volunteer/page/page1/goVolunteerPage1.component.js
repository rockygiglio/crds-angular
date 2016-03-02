(function() {
  'use strict';

  module.exports = GoVolunteerPage1;

  GoVolunteerPage1.$inject = [];

  function GoVolunteerPage1() {
    return {
      restrict: 'E',
      scope: {},
      bindToController: true,
      controller: GoVolunteerPage1Controller,
      controllerAs: 'page1',
      templateUrl: 'page1/goVolunteerPage1.template.html'
    };

    function GoVolunteerPage1Controller() {
      var vm = this;
    }
  }

})();
