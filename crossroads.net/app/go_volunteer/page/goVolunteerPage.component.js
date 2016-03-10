(function() {
  'use strict';

  module.exports = GoVolunteerPage;

  GoVolunteerPage.$inject = ['$stateParams'];

  function GoVolunteerPage($stateParams) {
    return {
      restrict: 'E',
      scope: {},
      bindToController: true,
      controller: GoVolunteerPageController,
      controllerAs: 'goVolunteerPage',
      templateUrl: 'page/goVolunteerPage.template.html'
    };

    function GoVolunteerPageController() {
      var vm = this;

      vm.showProfile = showProfile;
      vm.showSignin = showSignin;
      vm.showSpouse = showSpouse;
      vm.showOrgName = showOrgName;
      vm.showSpouseName = showSpouseName;
      vm.showChildren = showChildren;
      vm.showChildrenCount = showChildrenCount;
      vm.showGroupConnector = showGroupConnector;
      vm.showGroupFindConnector = showGroupFindConnector;
      vm.showConnectorInfo = showConnectorInfo;
      vm.showUniqueSkills = showUniqueSkills;
      vm.showEquipment = showEquipment;
      vm.showAdditionalInfo = showAdditionalInfo;
      vm.showAvailablePrep = showAvailablePrep;

      function showProfile() {
        return $stateParams.page === 'profile';
      }

      function showSignin() {
        return $stateParams.page === 'signin';
      }

      function showSpouse() {
        return $stateParams.page === 'spouse';
      }

      function showOrgName() {
        return $stateParams.page === 'name';
      }

      function showSpouseName() {
        return $stateParams.page === 'spouse-name';
      }

      function showChildren() {
        return $stateParams.page === 'children';
      }

      function showChildrenCount() {
        return $stateParams.page === 'children-count';
      }

      function showGroupConnector() {
        return $stateParams.page === 'group-connector';
      }

      function showGroupFindConnector() {
        return $stateParams.page === 'group-find-connector';
      }

      function showConnectorInfo() {
        return $stateParams.page === 'be-a-connector';
      }

      function showUniqueSkills() {
        return $stateParams.page === 'unique-skills';
      }

      function showEquipment() {
        return $stateParams.page === 'equipment';
      }

      function showAdditionalInfo() {
        return $stateParams.page === 'additional-info';
      }

      function showAvailablePrep() {
        return $stateParams.page === 'available-prep';
      }

    }
  }

})();
