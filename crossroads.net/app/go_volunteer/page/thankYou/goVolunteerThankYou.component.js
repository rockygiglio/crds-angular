(function() {
  'use strict';

  module.exports = GoVolunteerThankYou;

  GoVolunteerThankYou.$inject = [];

  function GoVolunteerThankYou() {
    return {
      restrict: 'E',
      scope: {},
      bindToController: true,
      controller: GoVolunteerThankYouController,
      controllerAs: 'goThankYou',
      templateUrl: 'thankYou/goVolunteerThankYou.template.html'
    };

    function GoVolunteerThankYouController() {
      var vm = this;

    }
  }

})();
