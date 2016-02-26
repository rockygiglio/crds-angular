(function(){
  'use strict';

  module.exports = HostReviewCtrl;

  HostReviewCtrl.$inject = [
    '$window',
    '$scope',
    '$state',
    'Responses',
    'Group',
    'AuthenticatedPerson',
    'GROUP_API_CONSTANTS',
    '$log',
    'GroupInvitationService',
    'GROUP_ROLE_ID_HOST',
    'LookupDefinitions'
  ];

  function HostReviewCtrl($window,
                          $scope,
                          $state,
                          Responses,
                          Group,
                          AuthenticatedPerson,
                          GROUP_API_CONSTANTS,
                          $log,
                          GroupInvitationService,
                          GROUP_ROLE_ID_HOST,
                          LookupDefinitions) {
    var vm = this;

    vm.initialize = function() {
      if (Responses.data.completed_flow !== true) {
        $state.go('group_finder.host.questions');
      }

      vm.responses = Responses.data;
      vm.host = AuthenticatedPerson;
      vm.lookup = LookupDefinitions;

      if(vm.isPrivate()) {
        vm.publish();
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

    var days = [ 'sunday', 'monday', 'tuesday', 'wednesday', 'thursday', 'friday', 'saturday' ];
    vm.publish = function() {
      vm.rejected = false;

      // Create the Group detail resource
      var group = new Group.Detail();

      // Set constants
      group.groupTypeId = GROUP_API_CONSTANTS.GROUP_TYPE_ID;
      group.ministryId = GROUP_API_CONSTANTS.MINISTRY_ID;
      group.startDate = GROUP_API_CONSTANTS.START_DATE;
      group.endDate = GROUP_API_CONSTANTS.END_DATE;

      // Group owner, name and description
      group.contactId = AuthenticatedPerson.contactId;
      group.groupName = AuthenticatedPerson.displayName();
      group.groupDescription = '';
      group.congregationId = AuthenticatedPerson.congregationId;

      // Group size and availability
      group.availableOnline = true;
      group.remainingCapacity = vm.capacity();
      group.groupFullInd = vm.capacity() <= 0;
      group.waitListInd = false;
      group.childCareInd = false;

      // When and where does the group meet
      group.meetingDayId = 1;

      group.meetingTime = '';
      group.address = {};

      if (vm.isPrivate() === false) {
        group.groupDescription = Responses.data.description;
        group.childCareInd = Responses.data.kids === 1;
        group.meetingDayId = days.indexOf(Responses.data.date_and_time.day.toLowerCase());
        group.meetingTime = Responses.data.date_and_time.time + ' ' + Responses.data.date_and_time.ampm;
        group.address = {
          addressLine1: Responses.data.location.street,
          city: Responses.data.location.city,
          state: Responses.data.location.state,
          zip: Responses.data.location.zip
        };
      }

      var attributes = [];
      if (_.has(Responses.data, 'goals')) {
        attributes.push(Responses.data.goals);
      }
      if (_.has(Responses.data, 'kids')) {
        attributes.push(Responses.data.kids);
      }

      if (_.has(Responses.data, 'pets')) {
        _.each(Responses.data.pets, function(value, pet) {
          attributes.push(pet);
        });
      }

      group.attributes = _.map(attributes, function(data) {
        return {selected: true, startDate: GROUP_API_CONSTANTS.START_DATE, attributeId: data};
      });

      // Publish the group to the API and handle the response
      $log.debug('Publishing group:', group);
      Group.Detail.save(group).$promise.then(function success(group) {

        $log.debug('Group was published successfully:', group);
        var capacity = 1;
        if (Responses.data.marital_status === '7022') {
          capacity = 2;
        }
        // User invitation service to add person to that group
        var promise = GroupInvitationService.acceptInvitation(group.groupId,
                      {capacity: capacity, groupRoleId: GROUP_ROLE_ID_HOST, attributes: group.attributes});
        promise.then(function() {
          // Invitation acceptance was successful
          vm.accepted = true;
        }, function(error) {
          // An error happened accepting the invitation
          vm.rejected = true;
        }).finally(function() {
          vm.requestPending = false;
        });

        // Created group successfully, go to confirmation page
        $state.go('group_finder.host.confirm');
      }, function error() {
        vm.rejected = true;
        $log.debug('An error occurred while publishing');
      });
    };

    vm.lookupContains = function(id, keyword) {
      return vm.lookup[id].toLowerCase().indexOf(keyword) > -1;
    };

    vm.getGroupAttributes = function() {
      var ret = [];
      if (vm.lookupContains(vm.responses.kids, 'kid')) { ret.push('kids welcome'); }

      if (vm.responses.pets) {
        _.each(vm.responses.pets, function(value, id) {
          if (value) {
            if (vm.lookupContains(id, 'dog')) { ret.push('has a dog'); }
            if (vm.lookupContains(id, 'cat')) { ret.push('has a cat'); }
          }
        });
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

    vm.capacity = function() {
      // capacity is total - filled + 1 to include the host
      return parseInt(vm.responses.total_capacity) - (parseInt(vm.responses.filled_spots) + 1);
    };

    vm.isPrivate = function() {
      return vm.responses && vm.capacity() <= 0;
    };

    // ------------------------------- //

    vm.initialize();

  }

})();
