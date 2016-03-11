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
      vm.showLaunchSite = showLaunchSite;
      vm.showProjectPrefOne = showProjectPrefOne;
      vm.showProjectPrefTwo = showProjectPrefTwo;
      vm.showProjectPrefThree = showProjectPrefThree;
      vm.showUniqueSkills = showUniqueSkills;
      vm.showEquipment = showEquipment;
      vm.showAdditionalInfo = showAdditionalInfo;
      vm.showAvailablePrep = showAvailablePrep;
      vm.showAvailablePrepSpouse = showAvailablePrepSpouse;
      vm.showWaiver = showWaiver;
      vm.showThankYou = showThankYou;

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

      function showLaunchSite() {
        return $stateParams.page === 'launch-site';
      }

      function showProjectPrefOne() {
        return $stateParams.page === 'project-preference-one';
      }

      function showProjectPrefTwo() {
        return $stateParams.page === 'project-preference-two';
      }

      function showProjectPrefThree() {
        return $stateParams.page === 'project-preference-three';
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

      function showAvailablePrepSpouse() {
        return $stateParams.page === 'available-prep-spouse';
      }

      function showWaiver() {
        return $stateParams.page === 'waiver';
      }

      function showThankYou() {
        return $stateParams.page === 'thank-you';
      }

    }
  }

})();
