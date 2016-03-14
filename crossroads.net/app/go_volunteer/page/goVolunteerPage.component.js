(function() {
  'use strict';

  module.exports = GoVolunteerPage;

  GoVolunteerPage.$inject = ['$state', '$stateParams'];

  function GoVolunteerPage($state, $stateParams) {
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

      vm.handlePageChange = handlePageChange;
      vm.showProfile = showProfile;
      vm.showSignin = showSignin;
      vm.showSpouse = showSpouse;
      vm.showOrgName = showOrgName;

      function handlePageChange(nextState) {
        if (!$stateParams.organization) {
          $state.go('go-volunteer.crossroadspage', {
           page: nextState
         });

        } else {
          $state.go('go-volunteer.page', {
            city: $stateParams.city,
            organization: $stateParams.organization,
            page: nextState
          });
        }
      }

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

    }
  }

})();
