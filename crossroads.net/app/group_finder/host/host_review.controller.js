(function(){
  'use strict';

  module.exports = HostReviewCtrl;

  HostReviewCtrl.$inject = ['$window', '$scope', '$state', 'Responses', 'Group', 'AuthenticatedPerson', '$log'];

  function HostReviewCtrl($window, $scope, $state, Responses, Group, AuthenticatedPerson, $log) {
    var vm = this;

    vm.initialize = function() {
      vm.responses = Responses.data;
      vm.host = AuthenticatedPerson;
      $log.debug("Host profile: ", vm.host);

      if(vm.isPrivate()) {
        return $state.go('group_finder.host.confirm');
      }

      var groupTitle = AuthenticatedPerson.displayName();

      vm.group = {
        groupTitle: groupTitle,
        time: vm.getGroupTime(),
        distance: '0 miles from you',
        description: vm.responses.description,
        type: vm.responses.group_type,
        attributes: vm.getGroupAttributes(),
        host: {
          contactId: $scope.person.contactId
        }
      };
    };

    vm.startOver = function() {
      $scope.$parent.currentStep = 1;
      $state.go('group_finder.host.questions');
    };

    /*
      {
        "groupName": "Sample Group",
        "groupDescription": "Sample Group Description",
        "groupTypeId": 19,
        "ministryId": 8,
        "congregationId": 1,
        "contactId": 2399608,
        "startDate": "2016-02-01T10:00:00.000Z",
        "endDate": "2016-03-01T10:00:00.000Z",
        "availableOnline": true,
        "remainingCapacity": 10,
        "groupFullInd": false,
        "waitListInd": false,
        "waitListGroupId": 0,
        "childCareInd": false,
        "minAge": 0,
        "meetingDayId": 1,
        "meetingTime": "10:00 AM",
        "groupRoleId": ,
        "address": {
          "addressLine1": "5766 Pandora Ave",
          "addressLine2": "",
          "city": "Cincinnati",
          "state": "Oh",
          "zip": "45213",
          "foreignCountry": "United States",
          "county": "",
        }
      }
     */
    vm.publish = function() {
      vm.rejected = false;

      var group = new Group.Group();
      group.groupName = AuthenticatedPerson.displayName();
      group.groupDescription = Responses.data.description;
      group.groupTypeId = 19;
      group.ministryId = 8;
      group.congregationId = AuthenticatedPerson.congregationId;
      group.contactId = AuthenticatedPerson.contactId;
      group.startDate = "2016-04-01T10:00:00.000Z";
      group.endDate = "2016-05-15T10:00:00.000Z";
      group.availableOnline = true;
      group.remainingCapacity = Responses.data.open_spots;
      group.groupFullInd = Responses.data.open_spots <= 0;
      group.waitListInd = false;
      group.childCareInd = Responses.data.kids === 1;

      // TODO Handle this as ordinal in Responses instead of day name string
      group.meetingDayId = 1;

      group.meetingTime = Responses.data.date_and_time.time + " " + Responses.data.date_and_time.ampm;
      group.address = {
        addressLine1: Responses.data.location.street,
        city: Responses.data.location.city,
        state: Responses.data.location.state,
        zip: Responses.data.location.zip
      };

      $log.debug("Publishing group:", group);
      Group.Group.save(group).$promise.then(function success(group) {
        $log.debug("Group was published successfully:", group);
      }, function error() {
        vm.rejected = true;
        $log.debug("An error occurred while publishing");
      });
      $log.debug("Group publish call initiated");

      //$state.go('group_finder.host.confirm');
    };

    vm.getGroupAttributes = function() {
      var ret = [];
      if (vm.responses.kids === '1') { ret.push('kids welcome'); }
      if (vm.responses.pets) {
        var pet_selections = _.map(Object.keys(vm.responses.pets), function(el) {
          return parseInt(el);
        });
        if (pet_selections.indexOf(0) !== -1) { ret.push('has a cat'); }
        if (pet_selections.indexOf(1) !== -1) { ret.push('has a dog'); }
      }
      return ret;
    };

    vm.getGroupTime = function() {
      var dt = vm.responses.date_and_time;
      if (dt) {
        return dt['day'] + 's @ ' + dt['time'] + dt['ampm'];
      }
    };

    vm.goBack = function() {
      $window.history.back();
    };

    vm.isPrivate = function() {
      return vm.responses && vm.responses.open_spots <= 0;
    };

    // ------------------------------- //

    vm.initialize();

  }

})();
