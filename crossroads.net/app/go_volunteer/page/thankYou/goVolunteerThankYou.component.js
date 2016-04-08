(function() {
  'use strict';

  module.exports = GoVolunteerThankYou;

  GoVolunteerThankYou.$inject = ['GoVolunteerService', 'GoVolunteerDataService'];

  function GoVolunteerThankYou(GoVolunteerService, GoVolunteerDataService) {
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

      activate();

      function activate() {
        var dto = GoVolunteerService.getRegistrationDto();
        GoVolunteerDataService.Create.save(dto, function(result) {
          console.log('success');
        },

        function(result) {
          // $log.error(result);
          console.log(result);
        });
      }
    }
  }

})();
