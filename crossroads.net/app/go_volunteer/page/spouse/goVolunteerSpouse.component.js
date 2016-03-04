(function() {
  'use strict';

  module.exports = GoVolunteerSpouse;

  GoVolunteerSpouse.$inject = [];

  function GoVolunteerSpouse() {
    return {
      restrict: 'E',
      scope: {},
      bindToController: true,
      controller: GoVolunteerSpouseController,
      controllerAs: 'goSpouse',
      templateUrl: 'spouse/goVolunteerSpouse.template.html'   
    };
    
    function GoVolunteerSpouseController() {
      var vm = this;

    }  
  }

})();
