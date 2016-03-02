(function() {
  'use strict';

  module.exports = GoVolunteerPage;

  GoVolunteerPage.$inject = [];

  function GoVolunteerPage() {
    return {
      restrict: 'E',
      scope: {},
      bindToController: true,
      controller: GoVolunteerPageController,
      controllerAs: 'page',
      templateUrl: 'page/goVolunteerPage.template.html'
    };

    function GoVolunteerPageController() {
      var vm = this;
    }
  }

})();
