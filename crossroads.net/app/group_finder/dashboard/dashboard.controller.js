(function(){
  'use strict';

  module.exports = DashboardCtrl;

  DashboardCtrl.$inject = [
    '$rootScope',
    '$scope',
    '$log',
    '$state',
    'Email',
    '$modal',
    'ImageService',
    'GroupInfo',
    'AuthenticatedPerson'
  ];

  function DashboardCtrl(
    $rootScope,
    $scope,
    $log,
    $state,
    Email,
    $modal,
    ImageService,
    GroupInfo,
    AuthenticatedPerson
  ) {

    var vm = this;

    vm.person = AuthenticatedPerson;
    vm.profileImageBaseUrl = ImageService.ProfileImageBaseURL;
    vm.profileImage = vm.profileImageBaseUrl + vm.person.contactId;
    vm.defaultImage = ImageService.DefaultProfileImage;

    vm.groups = {
      hosting: GroupInfo.getHosting(),
      participating: GroupInfo.getParticipating()
    };

    vm.displayName = function() {
      var name;
      if (vm.person) {
        name = vm.person.firstName || '';

        if (vm.person.lastName) {
          name = name + ' ' + vm.person.lastName[0] + '.';
        }
      }

      return name;
    };

    $scope.setGroup = function(group) {
      vm.group = group;
    };

    $rootScope.$on('$viewContentLoading', function(event){
      vm.group = undefined;
    });
  }

})();
