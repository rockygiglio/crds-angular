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


      function showProfile() {
        return $stateParams.page === 'profile'; 
      }

      function showSignin() {
        return $stateParams.page === 'signin'; 
      }
      
      function showSpouse() {
        return $stateParams.page === 'spouse';
      }

    }
  }

})();
