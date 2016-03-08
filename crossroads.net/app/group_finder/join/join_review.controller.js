(function(){
  'use strict';

  module.exports = JoinReviewCtrl;

  JoinReviewCtrl.$inject = [
    '$scope',
    '$state',
    'Responses',
    'ZipcodeService',
    'GroupInvitationService',
    'LookupDefinitions',
    'GROUP_ID',
    'GROUP_ROLE',
    '$window',
    'Session'
  ];

  function JoinReviewCtrl(
    $scope,
    $state,
    Responses,
    ZipcodeService,
    GroupInvitationService,
    LookupDefinitions,
    GROUP_ID,
    GROUP_ROLE,
    $window,
    Session
  ) {
    var vm = this;
    vm.initialize = initialize;
    vm.goToHost = goToHost;
    vm.goToResults = goToResults;
    vm.lookup = LookupDefinitions;
    vm.goBack = goBack;
    vm.lookupContains = lookupContains;

    function initialize() {

      vm.responses = getResponses();
      vm.showUpsell = parseInt(vm.responses.prior_participation) > 2;
      vm.showResults = vm.showUpsell === false;
      vm.contactCrds = false;
      vm.rejected = false;
      var meetTime = {
        week: true,
        weekend: true
      };

      // check of invalid date_times
      _.each(vm.responses.date_time_week, function(value, id) {
        if (value && vm.lookupContains(id, 'can\'t meet')) {
          meetTime.week = false;
        }
      });

      // check of invalid date_times
      _.each(vm.responses.date_time_weekend, function(value, id) {
        if (value && vm.lookupContains(id, 'can\'t meet')) {
          meetTime.weekend = false;
        }
      });

      vm.invalidTime = (!meetTime.week && !meetTime.weekend);

      if (vm.responses.location && vm.responses.location.zip) {
        vm.zipcode = parseInt(vm.responses.location.zip);
        if (ZipcodeService.isLocalZipcode(vm.zipcode) === false) {
          vm.showUpsell = false;
          vm.showResults = false;
          vm.contactCrds = true;

          var participant = {
            capacity: 1,
            contactId: parseInt(Session.exists('userId')),
            groupRoleId: GROUP_ROLE.PARTICIPANT,
            address: {
              addressLine1: vm.responses.location.street,
              city: vm.responses.location.city,
              state: vm.responses.location.state,
              zip: vm.responses.location.zip
            },
            singleAttributes: Responses.getSingleAttributes(vm.lookup)
          };

          vm.invalidTime = false; // set as an override

          var promise = GroupInvitationService.acceptInvitation(GROUP_ID.ANYWHERE, participant);
          promise.then(function() {
            // Invitation acceptance was successful
            vm.accepted = true;
          }, function(error) {
            // An error happened accepting the invitation
            vm.rejected = true;
          });
        }
      }

      if (vm.showResults === true && vm.contactCrds === false) {
        $state.go('group_finder.join.results');
      }

      if (parseInt(vm.responses.relationship_status) === 2) {
        $scope.showInvite = true;
      }
    }
    
    function getResponses() {
      if (Responses.data.completed_flow === true) {
        sessionStorage.setItem('participant', angular.toJson(Responses.data));
      } else {
        Responses.data = angular.fromJson(sessionStorage.getItem('participant'));
      }

      return Responses.data;
    }

    function goToHost() {
      $state.go('group_finder.host');
    }

    function goToResults() {
      $state.go('group_finder.join.results');
    }

    function lookupContains(id, keyword) {
      return vm.lookup[id].name.toLowerCase().indexOf(keyword) > -1;
    }

    function goBack() {
      $window.history.back();
    }

    vm.initialize();

  }

})();
