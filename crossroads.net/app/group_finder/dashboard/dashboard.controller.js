(function(){
  'use strict';

  module.exports = DashboardCtrl;

  DashboardCtrl.$inject = [
    '$rootScope',
    '$scope',
    '$log',
    '$state',
    'Person',
    'Email',
    '$modal',
    'ImageService',
    'GroupInfo'
  ];

  function DashboardCtrl(
    $rootScope,
    $scope,
    $log,
    $state,
    Person,
    Email,
    $modal,
    ImageService,
    GroupInfo
  ) {

    var vm = this;

    vm.profileData = { person: Person };
    vm.person = Person;
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
