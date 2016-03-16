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
      vm.availableOptions = ['Alabama', 'Alaska', 'Arizona', 'Arkansas', 'California', 'Colorado', 'Connecticut', 'Delaware', 'Florida', 'Georgia', 'Hawaii', 'Idaho', 'Illinois', 'Indiana', 'Iowa', 'Kansas', 'Kentucky', 'Louisiana', 'Maine', 'Maryland', 'Massachusetts', 'Michigan', 'Minnesota', 'Mississippi', 'Missouri', 'Montana', 'Nebraska', 'Nevada', 'New Hampshire', 'New Jersey', 'New Mexico', 'New York', 'North Dakota', 'North Carolina', 'Ohio', 'Oklahoma', 'Oregon', 'Pennsylvania', 'Rhode Island', 'South Carolina', 'South Dakota', 'Tennessee', 'Texas', 'Utah', 'Vermont', 'Virginia', 'Washington', 'West Virginia', 'Wisconsin', 'Wyoming'];
    }
  }

})();
