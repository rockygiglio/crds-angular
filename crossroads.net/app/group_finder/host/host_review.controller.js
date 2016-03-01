(function(){
  'use strict';

  module.exports = HostReviewCtrl;

  HostReviewCtrl.$inject = [
    '$window',
    '$scope',
    '$state',
    'Responses',
    'Group',
    'GroupInfo',
    'AuthenticatedPerson',
    'GROUP_API_CONSTANTS',
    '$log',
    'GroupInvitationService',
    'GROUP_ROLE_ID_HOST',
    'LookupDefinitions',
    'SERIES'
  ];

  function HostReviewCtrl($window,
                          $scope,
                          $state,
                          Responses,
                          Group,
                          GroupInfo,
                          AuthenticatedPerson,
                          GROUP_API_CONSTANTS,
                          $log,
                          GroupInvitationService,
                          GROUP_ROLE_ID_HOST,
                          LookupDefinitions,
                          SERIES) {
    var vm = this;

    vm.pending = true;
    vm.responses = Responses.data;
    vm.host = AuthenticatedPerson;
    vm.lookup = LookupDefinitions;
    vm.startOver = startOver;
    vm.publish = publish;
    vm.lookupContains = lookupContains;
    vm.getGroupAttributes = getGroupAttributes;
    vm.getGroupTime = getGroupTime;
    vm.formatTime = formatTime;
    vm.goBack = goBack;
    vm.capacity = capacity;
    vm.isPrivate = isPrivate;
    vm.initialize = initialize;

    function initialize() {
      if (Responses.data.completed_flow !== true) {
        $state.go('group_finder.host.questions');
      }

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
          contactId: AuthenticatedPerson.contactId
        }
      };

      vm.pending = false;
    }

    function startOver() {
      $scope.$parent.currentStep = 1;
      $state.go('group_finder.host.questions');
    }

    function publish() {
      var days = ['sunday', 'monday', 'tuesday', 'wednesday', 'thursday', 'friday', 'saturday'];
      vm.requestPending = true;
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
      group.groupName = moment().year() + ' ' + SERIES.title + ' ' + AuthenticatedPerson.lastName;
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
        group.groupDescription = vm.responses.description;
        group.childCareInd = vm.responses.kids === 1;

        // meetingDayId is not zero based
        group.meetingDayId = days.indexOf(vm.responses.date_and_time.day.toLowerCase()) + 1;
        group.meetingTime = vm.formatTime(vm.responses.date_and_time.time);
        group.address = {
          addressLine1: vm.responses.location.street,
          city: vm.responses.location.city,
          state: vm.responses.location.state,
          zip: vm.responses.location.zip
        };
      }

      var singleAttributes = ['gender', 'goals', 'group_type', 'kids', 'marital_status'];
      group.singleAttributes = {};
      _.each(singleAttributes, function (index) {
        var answer = this.data[index];
        var attributeTypeId = this.lookup[answer].attributeTypeId;
        group.singleAttributes[attributeTypeId] = {'attribute': {'attributeId': answer}};
      }, {data: vm.responses, lookup: vm.lookup});

      var attributes = [];
      var petAttributeTypeId = null;
      _.each(vm.responses.pets, function (hasPet, id) {
        if (!petAttributeTypeId) {
          petAttributeTypeId = this.lookup[id].attributeTypeId;
        }
        if (hasPet) {
          attributes.push({'attributeId': id, 'selected': true});
        }
      }, {lookup: vm.lookup});

      group.attributeTypes = {};
      group.attributeTypes[petAttributeTypeId] = {
        attributeTypeId: petAttributeTypeId,
        attributes: attributes
      };

      // Publish the group to the API and handle the response
      $log.debug('Publishing group:', group);
      Group.Detail.save(group).$promise
        .then(function groupPublishSuccess(group) {
          $log.debug('Group was published successfully:', group);
          var capacity = 1;
          if (Responses.data.marital_status === '7022') {
            capacity = 2;
          }

          // User invitation service to add person to that group
          return GroupInvitationService.acceptInvitation(group.groupId,
            {capacity: capacity, groupRoleId: GROUP_ROLE_ID_HOST, attributes: group.attributes});
        })
        .then(function hostInviteSuccess() {
          $log.debug("Host was added to new group, force group info reload");
          return GroupInfo.loadGroupInfo(true);
        })
        .then(function reloadGroupSuccess() {
            // Invitation acceptance was successful
            vm.accepted = true;

            // Created group successfully, go to confirmation page
            $state.go('group_finder.host.confirm');
          },
          function chainError() {
            vm.rejected = true;
            vm.requestPending = false;
            $log.debug('An error occurred while publishing');
          });
    }

    function lookupContains(id, keyword) {
      return vm.lookup[id].name.toLowerCase().indexOf(keyword) > -1;
    }

    function getGroupAttributes() {
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
    }

    function getGroupTime() {
      var dt = vm.responses.date_and_time;
      if (dt) {
        return dt['day'] + 's @ ' + vm.formatTime(dt['time']);
      }
    }

    function formatTime(time) {
      return  moment(time).format('h:mm a');
    }

    function goBack() {
      $window.history.back();
    }

    function capacity() {
      // capacity is total - filled + 1 to include the host
      return parseInt(vm.responses.total_capacity) - (parseInt(vm.responses.filled_spots));
    }

    function isPrivate() {
      return vm.responses && vm.capacity() <= 0;
    }

    // ------------------------------- //

    vm.initialize();

  }

})();
