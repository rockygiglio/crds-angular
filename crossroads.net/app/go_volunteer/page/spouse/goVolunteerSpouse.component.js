(function() {
  'use strict';

  module.exports = GoVolunteerSpouse;

  GoVolunteerSpouse.$inject = ['GoVolunteerService'];

  function GoVolunteerSpouse(GoVolunteerService) {
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
      vm.spouse = GoVolunteerService.spouse;
      vm.spouseName = spouseName;
      vm.submit = submit;

      function spouseName(){
        if (vm.spouse.preferredName !== '' && vm.spouse.preferredName !== undefined){
          return vm.spouse.preferredName + ' ' + vm.spouse.lastName;
        } else {
          return "your spouse";
        }
      }

      function submit(spouseServing){
        if(spouseServing){

        } else {
          
        }
      }
    }
  }

})();
