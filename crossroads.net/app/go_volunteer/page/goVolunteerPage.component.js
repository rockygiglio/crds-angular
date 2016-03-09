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

    }
  }

})();
